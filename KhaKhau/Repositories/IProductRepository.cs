using Humanizer.Localisation;
using KhaKhau.Areas.Admin.Models;

namespace KhaKhau.Repositories
{
    public interface  IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetProduct(string sTerm = "", int genreId = 0);
        Task<IEnumerable<Category>> Categories();
    }
}
