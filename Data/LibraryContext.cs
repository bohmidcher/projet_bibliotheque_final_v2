using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Models;

namespace projet_bibliotheque.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Loan> Loans { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=localhost;Database=BibliothequeIHEC;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true",
                    options => options.EnableRetryOnFailure())
                             .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Supprimez cette section redondante
            // modelBuilder.Entity<Member>(entity =>
            // {
            //     entity.Property(e => e.Id);
            //     entity.Property(e => e.Name);
            //     entity.Property(e => e.Email);
            // });
            
            // Configure Member
            modelBuilder.Entity<Member>()
                .HasIndex(m => m.Email)
                .IsUnique();

            modelBuilder.Entity<Member>()
                .Property(m => m.Password)
                .IsRequired()
                .HasMaxLength(100);

            // Configure Book
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique();

            // Configure Book-Author relationship
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Loan-Book relationship
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Loan-Member relationship
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Member)
                .WithMany(m => m.Loans)
                .HasForeignKey(l => l.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add some sample data
            modelBuilder.Entity<Member>().HasData(
                new Member { 
                    Id = 1, 
                    Name = "Admin", 
                    Email = "admin@ihec.tn", 
                    Password = "admin123", 
                    DateInscription = new System.DateTime(2024, 1, 1),
                    IsAdmin = true,
                    // Ajout des propriétés manquantes
                    Address = "IHEC Carthage",
                    Phone = "71234567",
                    Status = "Actif"
                }
            );

            modelBuilder.Entity<Author>().HasData(
                new Author { 
                    Id = 1, 
                    Name = "Victor Hugo", 
                    Nationality = "French", 
                    Birthdate = new System.DateTime(1802, 2, 26) 
                },
                new Author { 
                    Id = 2, 
                    Name = "William Shakespeare", 
                    Nationality = "English", 
                    Birthdate = new System.DateTime(1564, 4, 23) 
                }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { 
                    Id = 1, 
                    Title = "Les Misérables", 
                    ISBN = "9780140444308", 
                    PublicationDate = new System.DateTime(1862, 1, 1),
                    Genre = "Roman",
                    AuthorId = 1
                },
                new Book {
                    Id = 2,
                    Title = "Hamlet",
                    ISBN = "9780141396507",
                    PublicationDate = new System.DateTime(1601, 1, 1),
                    Genre = "Théâtre",
                    AuthorId = 2
                }
            );
        }
    }
}