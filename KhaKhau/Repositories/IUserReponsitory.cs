using KhaKhau.Areas.Admin.Models;
using KhaKhau.Areas.Identity.Data;

namespace KhaKhau.Repositories
{
  
        public interface IUserResponsitory
        {
            Task<IEnumerable<ApplicationUser>> GetAllAsync();
            Task<ApplicationUser> GetByIdAsync(string id);
            Task AddAsync(ApplicationUser product);
            Task UpdateAsync(ApplicationUser product);
            Task DeleteAsync(int id);
        }
    
}
