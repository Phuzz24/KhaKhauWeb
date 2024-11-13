using KhaKhau.Areas.Admin.Models;
using KhaKhau.Areas.Identity.Data;
using KhaKhau.Data;
using Microsoft.EntityFrameworkCore;

namespace KhaKhau.Repositories
{
    public class EFUserRepository : IUserResponsitory   
    {
        private readonly KhaKhauContext _context;
        public EFUserRepository(KhaKhauContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();

        }
        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
            // lấy thông tin kèm theo category
            //return await _context.Products.Include(p =>
            //p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(ApplicationUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
