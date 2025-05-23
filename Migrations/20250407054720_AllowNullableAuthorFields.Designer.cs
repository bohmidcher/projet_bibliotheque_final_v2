﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using projet_bibliotheque.Data;

#nullable disable

namespace projet_bibliotheque.Migrations
{
    [DbContext(typeof(LibraryContext))]
    [Migration("20250407054720_AllowNullableAuthorFields")]
    partial class AllowNullableAuthorFields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("projet_bibliotheque.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nationality")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Birthdate = new DateTime(1802, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Victor Hugo",
                            Nationality = "French"
                        },
                        new
                        {
                            Id = 2,
                            Birthdate = new DateTime(1564, 4, 23, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "William Shakespeare",
                            Nationality = "English"
                        });
                });

            modelBuilder.Entity("projet_bibliotheque.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ISBN")
                        .IsUnique();

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = 1,
                            Genre = "Roman",
                            ISBN = "9780140444308",
                            PublicationDate = new DateTime(1862, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Les Misérables"
                        },
                        new
                        {
                            Id = 2,
                            AuthorId = 2,
                            Genre = "Théâtre",
                            ISBN = "9780141396507",
                            PublicationDate = new DateTime(1601, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Hamlet"
                        });
                });

            modelBuilder.Entity("projet_bibliotheque.Models.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LoanDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("MemberId");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("projet_bibliotheque.Models.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Members");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@ihec.tn",
                            JoinDate = new DateTime(2025, 4, 7, 6, 47, 20, 186, DateTimeKind.Local).AddTicks(3196),
                            Name = "Admin",
                            Password = "admin123"
                        });
                });

            modelBuilder.Entity("projet_bibliotheque.Models.Book", b =>
                {
                    b.HasOne("projet_bibliotheque.Models.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("projet_bibliotheque.Models.Loan", b =>
                {
                    b.HasOne("projet_bibliotheque.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("projet_bibliotheque.Models.Member", "Member")
                        .WithMany("Loans")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("projet_bibliotheque.Models.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("projet_bibliotheque.Models.Member", b =>
                {
                    b.Navigation("Loans");
                });
#pragma warning restore 612, 618
        }
    }
}
