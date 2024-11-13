using KhaKhau.Areas.Admin.Models;
using KhaKhau.Data;
using KhaKhau.Migrations;
using Microsoft.EntityFrameworkCore;

namespace KhaKhau.Repositories
{
    public class EFProductRepository :IProductRepository
    {
        private readonly KhaKhauContext _context;
        public EFProductRepository( KhaKhauContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // return await _context.Products.ToListAsync();
            return await _context.Products
            .Include(p => p.Category) // Include thông tin về category
            .ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            return await _context.Products.Include(p =>
            p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        //27/10 moi  them:
      
        public async Task<IEnumerable<Category>> Categories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProduct(string sTerm = "", int categoryId = 0)
        {
            if (sTerm != null)
            {
                sTerm = sTerm.ToLower();
            }

            IEnumerable<Product> products = await (from product in _context.Products
                                                  join category in _context.Categories
                                                  on product.CategoryId equals category.Id
                                                   join stock in _context.Stocks
                                                   on product.Id equals stock.Productid
                                                   into product_stocks
                                                   from productWithStocks in product_stocks.DefaultIfEmpty()
                                                   where string.IsNullOrWhiteSpace(sTerm) || (product != null && product.Name.ToLower().StartsWith(sTerm))
                                                   select new Product
                                                  {
                                                      Id = product.Id,
                                                      ImageUrl = product.ImageUrl,
                                                      Name = product.Name,
                                                      Description = product.Description,
                                                      CategoryId = product.CategoryId,
                                                      Price = product.Price,
                                                      CategoryName = category.Name,
                                                      Quantity = productWithStocks == null ? 0 : productWithStocks.Quantity
                                                  }
                         ).ToListAsync();
            if (categoryId > 0)
            {

                products = products.Where(a => a.CategoryId == categoryId).ToList();
            }
            return products;
        }
    }
}
