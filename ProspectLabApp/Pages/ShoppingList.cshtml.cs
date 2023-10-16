using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProspectLabApp.Pages.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Azure;
using static ProspectLabApp.Pages.CartManager;

namespace ProspectLabApp.Pages
{
    public class ShoppingListModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly HttpRequest _request;
        private readonly HttpResponse _response;
        public readonly CartManager _cartManager;

        public string Message { get; set; }

        public ShoppingListModel(AppDbContext context, IHttpContextAccessor httpContextAccessor, CartManager cartManager)
        {
            _context = context;
            _request = httpContextAccessor.HttpContext.Request;
            _response = httpContextAccessor.HttpContext.Response;
            _cartManager = cartManager;
        }
        public List<Product> Products { get; set; }
        public void OnGet()
        {
            Products = new List<Product>();

            if (_request.Cookies.ContainsKey("cart"))
            {
                var cartCookie = _request.Cookies["cart"];
                var cartProducts = JsonConvert.DeserializeObject<List<CartProduct>>(cartCookie);
                var cartProductIds = cartProducts.Select(p => p.ProductId).ToList();

                var products = _context.Products.Where(p => cartProductIds.Contains(p.Id)).ToList();
                if (products != null)
                {
                    Products.AddRange(products);
                }
            }
        }

        [ValidateAntiForgeryToken]
        public void OnPostReduceQuantity(Guid productId)
        {
            _cartManager.ReduceQuantity(productId);
        }

        [ValidateAntiForgeryToken]
        public void OnPostIncreaseQuantity(Guid productId)
        {
            _cartManager.IncreaseQuantity(productId);
        }

        [ValidateAntiForgeryToken]
        public void OnPostDeleteProduct(Guid productId)
        {
            _cartManager.DeleteProduct(productId);
        }

        [ValidateAntiForgeryToken]
        public void OnPostDeleteProducts(Guid[] productIds)
        {
            _cartManager.DeleteProducts(productIds);
        }
        public JsonResult OnGetGetCartCount()
        {
            var cartProducts = _cartManager.GetCartProducts();
            int count = cartProducts.Count;
            return new JsonResult(count);
        }
    }
}
