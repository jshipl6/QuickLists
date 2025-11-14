using Microsoft.EntityFrameworkCore;
using QuickLists.Data;        
using QuickLists.Services;   

var builder = WebApplication.CreateBuilder(args);

// MVC (controllers with views)
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<QuickListsContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("QuickLists")
        ?? "Data Source=quicklists.db"));

// DI: application service for task logic
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Error handling + HSTS in non-dev
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Default route: Tasks/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tasks}/{action=Index}/{id?}");

app.Run();
