using iText.Commons.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using ProspectLabApp.Pages.Entities;
using Microsoft.AspNetCore.Http;

namespace ProspectLabApp.Pages
{
    public class OffersModel : PageModel
    {
        private readonly AppDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly CartManager _cartManager;
        public OffersModel(IWebHostEnvironment webHostEnvironment, AppDbContext context, CartManager cartManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _cartManager = cartManager;
        }
        public IList<Product>? Products { get; set; }

        public string ProductsJson { get; set; }

        public async Task OnGetAsync()
        {
            string unfilteredList = _webHostEnvironment.WebRootPath + "/file/leaflet.txt";
            string excludedPath = _webHostEnvironment.WebRootPath + "/file/excluded.txt";

            string pdfFolder = _webHostEnvironment.WebRootPath + "/pdf";
            string[] pdfFiles = Directory.GetFiles(pdfFolder, "*.pdf");
            if (pdfFiles.Length > 0)
            {
                string pdfFile = pdfFiles[0];
                string leafletName = Path.GetFileNameWithoutExtension(pdfFile);

                List<Product> products = ProductExtractor.ExtractProducts(unfilteredList, excludedPath, leafletName);
                SaveToDatabase(products);
            }

            Products = await _context.Products.OrderBy(p => p.Title).ToListAsync();
            ProductsJson = JsonConvert.SerializeObject(Products);
        }

        private void SaveToDatabase(List<Product> products)
        {
            string newLeafletName = products[0].LeafletName;

            var oldProducts = _context.Products.Where(p => p.LeafletName != newLeafletName);
            _context.Products.RemoveRange(oldProducts);

            foreach (var product in products)
            {
                var existingProduct = _context.Products.FirstOrDefault(p => p.LeafletName == product.LeafletName);

                if (existingProduct != null)
                {
                    continue;
                }
                else
                {
                    _context.Products.Add(product);
                }
            }
            _context.SaveChanges();
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostAddToCart(Guid productId)
        {
            if(!ModelState.IsValid)
            {
                throw new BadHttpRequestException("Error");
            }
            else
            {
                var product = _context.Products.Find(productId);
                _cartManager.AddToCart(product.Id);
            }

            return RedirectToPage("Offers");
        }
        
        public JsonResult OnGetGetCartCount()
        {
            var cartProducts = _cartManager.GetCartProducts();
            
            return new JsonResult(cartProducts.Count);
        }
    }
}
