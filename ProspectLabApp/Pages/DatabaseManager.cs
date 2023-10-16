using Microsoft.EntityFrameworkCore;
using ProspectLabApp.Pages.Entities;

namespace ProspectLabApp.Pages
{
    public class DatabaseManager
    {
        private readonly AppDbContext _context;

        public DatabaseManager(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> GetProducts(List<Guid> productIds)
        {
            return _context.Products.Where(p => productIds.Contains(p.Id)).ToList();
        }
    }
}
