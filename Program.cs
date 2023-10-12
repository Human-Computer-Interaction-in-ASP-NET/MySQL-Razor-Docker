using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeedIdentity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var host = builder.Configuration["DBHOST"] ?? "localhost";
var port = builder.Configuration["DBPORT"] ?? "3333";
var user = builder.Configuration["DBUSER"] ?? "root";
var password = builder.Configuration["DBPASSWORD"] ?? "secret";
var db = builder.Configuration["DBNAME"] ?? "test-db";

string connectionString = $"server={host}; userid={user}; pwd={password};"
        + $"port={port}; database={db};SslMode=none;allowpublickeyretrieval=True;";

var serverVersion = new MySqlServerVersion(new Version(10,7,3));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, serverVersion));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(
options => {
    options.Stores.MaxLengthForKeys = 128;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddRoles<IdentityRole>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// using (var scope = app.Services.CreateScope()) {
//     var services = scope.ServiceProvider;

//     var context = services.GetRequiredService<ApplicationDbContext>();    
//     context.Database.Migrate();

//     var userMgr = services.GetRequiredService<UserManager<IdentityUser>>();  
//     var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();  

//     IdentitySeedData.Initialize(context, userMgr, roleMgr).Wait();
// }

// this code applies any outsnading migrations and creates the database if it does not exist
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();    
    context.Database.Migrate();
}

app.Run();
