using KhaKhau.Areas.Admin.Models;
using KhaKhau.Data;
using Microsoft.EntityFrameworkCore;

namespace KhaKhau.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly KhaKhauContext _context;

        public EFCategoryRepository(KhaKhauContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
             return await _context.Categories.ToListAsync();
           
        }
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
            // lấy thông tin kèm theo category
            //return await _context.Products.Include(p =>
            //p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
