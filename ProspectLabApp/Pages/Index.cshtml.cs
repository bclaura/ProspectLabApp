
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace ProspectLabApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly CartManager _cartManager;

        public IndexModel(IWebHostEnvironment webHostEnvironment, CartManager cartManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _cartManager = cartManager;
        }



        public string? Text { get; set; }


        public void OnGet()
        {
            string pdfFolder = _webHostEnvironment.WebRootPath + "/pdf";
            string[] pdfFiles = Directory.GetFiles(pdfFolder, "*.pdf");
            if(pdfFiles.Length > 0)
            {
                string path = pdfFiles[0];
                Text = PdfExtractor.pdfText(path);

                string filePath = _webHostEnvironment.WebRootPath + "/file/leaflet.txt";

                System.IO.File.WriteAllText(filePath, Text);
            }

        }
    }
}