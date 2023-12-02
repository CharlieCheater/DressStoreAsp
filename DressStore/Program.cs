using DressStore.Infrastructure;
using DressStore.Infrastructure.Data;
using DressStore.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Подключаем сервис по работе с Ef Core,
// строка подключения находится в appsettings.json
builder.Services.AddDbContext<ApplicationContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("Db")));
// Подключаем сервис аутентификации
// Убираю требования к паролю и подтверждению аккаунта по умолчанию
// Указываю, что данные о пользователе будут храниться в Бд, указанной в ApplicationContext
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
{
    opt.SignIn.RequireConfirmedAccount = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 6;
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationContext>();
// Настройка длительности хранения данных о пользователе

builder.Services.ConfigureApplicationCookie(config =>
{
    config.ExpireTimeSpan = TimeSpan.FromHours(8);
    config.SlidingExpiration = true;
});

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// Использую свой класс для первичной инициализации БД
using(var scope = app.Services.CreateScope())
{
    await DbInitializer.InitAsync(scope.ServiceProvider);
}
app.Run();
