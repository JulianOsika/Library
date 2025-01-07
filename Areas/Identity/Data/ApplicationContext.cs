using Library.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Library.Models;
using System.Reflection.Emit;

namespace Library.Data;

public class ApplicationContext : IdentityDbContext<User>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Konfiguracja dla Loan -> Book (One-to-One)
        builder.Entity<Loan>()
            .HasOne(l => l.Book)
            .WithOne(b => b.Loan)
            .HasForeignKey<Loan>(l => l.BookId); // Opcjonalnie: definiowanie zachowania przy usuwaniu

        // Konfiguracja dla Loan -> User (Many-to-One)
        builder.Entity<Loan>()
            .HasOne(l => l.User)
            .WithMany(u => u.Loans)
            .HasForeignKey(l => l.UserId);
    }
    

    public DbSet<Library.Models.Book> Book { get; set; } = default!;

    public DbSet<Library.Models.Loan> Loan { get; set; } = default!;
}
