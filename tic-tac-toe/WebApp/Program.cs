using DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

connectionString = connectionString.Replace("<%location%>", FileHelper.BasePath);


// register "how to create a db when somebody asks for it"
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

//builder.Services
//.AddTransient<>(); - create new one every time
//.AddSingleton<>(); - create new one on first try, all the next requests get existing
//.AddScoped<>(); - create new one for every web request

builder.Services.AddScoped<GameRepositoryDb>();
// builder.Services.AddScoped<GameRepositoryJson>();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
} 
else 
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();