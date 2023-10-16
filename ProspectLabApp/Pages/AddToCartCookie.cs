using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace ProspectLabApp.Pages
{
    public class AddToCartCookie
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddToCartCookie(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddToCart(Product product)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var products = new List<Product>();
            if(httpContext.Request.Cookies.ContainsKey("cart"))
            {
                var cartCookie = httpContext.Request.Cookies["cart"];
                products = JsonConvert.DeserializeObject<List<Product>>(cartCookie);    
            }

            var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);
            if(existingProduct != null)
            {
                existingProduct.Amount++;
            }
            else
            {
                products.Add(product);
            }

            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(2),
                IsEssential = true,
                Path = "/"
            };

            httpContext.Response.Cookies.Append("cart", JsonConvert.SerializeObject(products), options);
        }
    }
}
