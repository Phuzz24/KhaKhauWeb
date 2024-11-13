using Microsoft.EntityFrameworkCore;
using KhaKhau.Data;
using Microsoft.AspNetCore.Authentication;
using KhaKhau.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using KhaKhau.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("KhaKhauContextConnection") ?? throw new InvalidOperationException("Connection string 'KhaKhauContextConnection' not found.");
builder.Services.AddDbContext<KhaKhauContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<KhaKhauContext>();
//builder.Services
//    .AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<KhaKhauContext>()
//    .AddDefaultUI()
//    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
builder.Services.AddScoped<IUserResponsitory, EFUserRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IUserOrderRepository , UserOrderRepository>();
builder.Services.AddScoped<IStockRepository ,StockRepository>();


//them vao 19/10/24: sql dk, dn


// Add services to the container.

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddMvc();


builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["App:GoogleClientId"];
                    options.ClientSecret = builder.Configuration["App:GoogleClientSecret"];
                    // Map the external picture claim to the internally used image claim
                    options.ClaimActions.MapJsonKey("image", "picture");
            
                })
                .AddFacebook(options =>
                {
                    options.AppId = builder.Configuration["App:FacebookClientId"];
                    options.ClientSecret = builder.Configuration["App:FacebookClientSecret"];
                });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//can phai dung thu tu Authen trc Author
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapRazorPages();

//phan vung admin:
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=ProductManager}/{action=Index}/{id?}"
    );
});
//dieu huong phan vung cho trang 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
