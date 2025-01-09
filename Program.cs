using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Areas.Identity.Data;
using Library.Models;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationContextConnection' not found.");;

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddIdentity<Library.Areas.Identity.Data.User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationContext>()
.AddDefaultTokenProviders();

static async Task SeedRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<Library.Areas.Identity.Data.User>>();
    var dbContext = serviceProvider.GetRequiredService<ApplicationContext>();

    var roles = new[] { "Admin", "Employee", "Customer" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var adminEmail = "admin@library.com";
    var adminPassword = "Admin123!";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var newAdmin = new Library.Areas.Identity.Data.User
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var createAdminResult = await userManager.CreateAsync(newAdmin, adminPassword);

        if (createAdminResult.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }

    // Tworzenie użytkowników testowych
    var testUsers = new[]
    {
        new User { UserName = "customer1@mail.com", Email = "customer1@mail.com", EmailConfirmed = true },
        new User { UserName = "customer2@mail.com", Email = "customer2@mail.com", EmailConfirmed = true },
        new User { UserName = "customer3@mail.com", Email = "customer3@mail.com", EmailConfirmed = true },
    };

    foreach (var testUser in testUsers)
    {
        var existingUser = await userManager.FindByEmailAsync(testUser.Email);

        if (existingUser == null)
        {
            var result = await userManager.CreateAsync(testUser, "Password123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(testUser, "Customer");
                Console.WriteLine($"Użytkownik {testUser.Email} został pomyślnie utworzony z rolą Customer.");
            }
            else
            {
                Console.WriteLine($"Błąd podczas tworzenia użytkownika {testUser.Email}:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine($"Użytkownik {testUser.Email} już istnieje. Pomijanie...");
        }

    }

    // Tworzenie gatunków literackich
    if (!await dbContext.Genre.AnyAsync())
    {
        var genres = new[]
        {
            new Genre { Name = "Other" },
            new Genre { Name = "Comedy" },
            new Genre { Name = "Fantasy" },
            new Genre { Name = "Science Fiction" },
            new Genre { Name = "Fiction" },
            };

        dbContext.Genre.AddRange(genres);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("Gatunki literackie zostały utworzone.");
    }

    // Tworzenie książek
    if (!await dbContext.Book.AnyAsync())
    {
        var genres = await dbContext.Genre.ToListAsync();
        var books = new[]
        {
            new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", GenreId = genres.FirstOrDefault(g => g.Name == "Fiction")?.Id ?? 1},
            new Book { Title = "1984", Author = "George Orwell", GenreId = genres.FirstOrDefault(g => g.Name == "Fiction")?.Id ?? 1},
            new Book { Title = "Harry Potter", Author = "J.K. Rowling", GenreId = genres.FirstOrDefault(g => g.Name == "Fiction")?.Id ?? 1 },
            new Book { Title = "The Catcher in the Rye", Author = "J.D. Salinger", GenreId = genres.FirstOrDefault(g => g.Name == "Fiction")?.Id ?? 1},
            new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", GenreId = genres.FirstOrDefault(g => g.Name == "Fiction")?.Id ?? 1},
         };

        dbContext.Book.AddRange(books);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("Przykładowe książki zostały utworzone.");
    }
}


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRoles(services);
}

app.MapRazorPages();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
