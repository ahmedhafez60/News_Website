using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

using Microsoft.AspNetCore.Identity.UI.Services;
using WebApplication1.Services;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddDbContext<NewContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NewsDb")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<NewContext>()
    .AddDefaultUI() // أضف هذه لتفادي أي مشاكل في واجهات Identity
    .AddDefaultTokenProviders();

var app = builder.Build();




// بعد var app = builder.Build();
// ضع هذا الكود قبل app.Run(); في ملف Program.cs مباشرة
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // 1. إنشاء دور Admin لو مش موجود
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // 2. اكتب هنا الإيميل الحقيقي اللي بتسجل به دخول حالياً (مثلاً إيميلك الثاني أو الأول)
    var adminEmail = "ahmedhafez1920@gmail.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    // 3. إعطاء الصلاحية للمستخدم
    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}






app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
