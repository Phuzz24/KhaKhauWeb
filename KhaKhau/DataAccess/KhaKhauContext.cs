using KhaKhau.Areas.Admin.Models;
using KhaKhau.Areas.Identity.Data;
using KhaKhau.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KhaKhau.Data;

public class KhaKhauContext : IdentityDbContext<ApplicationUser>
{
	public KhaKhauContext(DbContextOptions<KhaKhauContext> options)
        : base(options)
    {

    }
	public virtual DbSet<Product> Products { get; set; }
	public virtual DbSet<Category> Categories { get; set; }
	public virtual DbSet<ProductImage> ProductImages { get; set; }
	//shopping cart
	public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

	public virtual DbSet<CartDetail> CartDetails { get; set; }
	//public DbSet<CartItem> CartItem { get; set; }  
	public virtual DbSet<Order> Orders { get; set; }
	public virtual DbSet<OrderDetail> OrderDetails { get; set; }
	public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

	//update 
	public virtual DbSet<Stock> Stocks { get; set; }	


	//protected override void OnModelCreating(ModelBuilder builder)
	//{
	//    base.OnModelCreating(builder);
	//    var admin = new IdentityRole("admin");
	//    admin.NormalizedName = "admin";  
	//    var user = new IdentityRole("user");
	//    user.NormalizedName = "user";  
	//    var employee = new IdentityRole("employee");
	//    employee.NormalizedName = "employee";
	//    builder.Entity<IdentityRole>().HasData(admin, user, employee);
	//}
}
