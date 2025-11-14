using Microsoft.EntityFrameworkCore;
using QuickLists.Data;
using QuickLists.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the DbContext
builder.Services.AddDbContext<QuickListsContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("QuickLists")
                ?? "Data Source=quicklists.db"));

// Register your DI service
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Ensure DB is created/migrated at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<QuickListsContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tasks}/{action=Index}/{id?}");

app.Run();
