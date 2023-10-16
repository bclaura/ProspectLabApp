using Newtonsoft.Json;

namespace ProspectLabApp.Pages
{
    public class CartManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppDbContext _appDbContext;

        public CartManager(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;

            var httpContext = _contextAccessor.HttpContext;
            if (!httpContext.Request.Cookies.ContainsKey("cart"))
            {
                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddHours(2),
                    IsEssential = true,
                    Path = "/"
                };
                httpContext.Response.Cookies.Append("cart", "[]", options);
            }
        }

        public List<CartProduct> GetCartProducts()
        {
            var httpContext = _contextAccessor.HttpContext;

            if (httpContext.Request.Cookies.ContainsKey("cart"))
            {
                var cartCookie = httpContext.Request.Cookies["cart"];
                return JsonConvert.DeserializeObject<List<CartProduct>>(cartCookie);
            }

            return new List<CartProduct>();
        }

        public void SaveCartProducts(List<CartProduct> cartProducts)
        {
            var httpContext = _contextAccessor.HttpContext;

            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(2),
                IsEssential = true,
                Path = "/"
            };

            var cartData = JsonConvert.SerializeObject(cartProducts);
            httpContext.Response.Cookies.Append("cart", cartData, options);
        }


        public void ReduceQuantity(/*AppDbContext appDbContext,*/ Guid productId)
        {
            var cartProducts = GetCartProducts();

            var cartProduct = cartProducts.FirstOrDefault(cp => cp.ProductId == productId);
            if (cartProduct != null && cartProduct.Q > 1)
            {
                cartProduct.Q--;
                SaveCartProducts(cartProducts);
            }
        }

        public void AddToCart(Guid productId)
        {
            var cartProducts = GetCartProducts();
            var existingCartProduct = cartProducts.FirstOrDefault(p => p.ProductId == productId);

            if (existingCartProduct == null)
            {
                cartProducts.Add(new CartProduct { ProductId = productId, Q = 1 });
            }
            else
            {
                existingCartProduct.Q++;
            }

            SaveCartProducts(cartProducts);

        }

        public void IncreaseQuantity(/*AppDbContext appDbContext,*/ Guid productId)
        {
            var cartProducts = GetCartProducts();

            var cartProduct = cartProducts.FirstOrDefault(cp => cp.ProductId == productId);
            if (cartProduct != null)
            {
                cartProduct.Q++;
                SaveCartProducts(cartProducts);
            }
        }

        public void DeleteProduct(Guid productId)
        {
            var cartProducts = GetCartProducts();
            var cartProduct = cartProducts.FirstOrDefault(p => p.ProductId == productId);
            if(cartProducts != null)
            {
                cartProducts.Remove(cartProduct);
                SaveCartProducts(cartProducts);
            }
        }

        public void DeleteProducts(Guid[] productIds)
        {
            var cartProducts = GetCartProducts();
            foreach (var productId in productIds)
            {
                var cartProduct = cartProducts.FirstOrDefault(p => p.ProductId == productId);
                if (cartProduct != null)
                {
                    cartProducts.Remove(cartProduct);
                }
            }
            SaveCartProducts(cartProducts);
        }

    }
}
