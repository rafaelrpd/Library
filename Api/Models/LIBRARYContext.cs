using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Api.Models
{
    public partial class LIBRARYContext : DbContext
    {
        public LIBRARYContext()
        {
        }

        public LIBRARYContext(DbContextOptions<LIBRARYContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<BorrowedBook> BorrowedBooks { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DISCOVOADOR;Initial Catalog=LIBRARY;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("AUTHOR");

                entity.Property(e => e.AuthorId).HasColumnName("AUTHOR ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("date")
                    .HasColumnName("REGISTRATION DATE")
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Isbn)
                    .HasName("PK_BOOK_ID");

                entity.ToTable("BOOK");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("ISBN")
                    .IsFixedLength();

                entity.Property(e => e.AuthorId).HasColumnName("AUTHOR ID");

                entity.Property(e => e.CategoryId).HasColumnName("CATEGORY ID");

                entity.Property(e => e.Quantity).HasColumnName("QUANTITY");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AUTHOR_ID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CATEGORY_ID");
            });

            modelBuilder.Entity<BorrowedBook>(entity =>
            {
                entity.ToTable("BORROWED BOOK");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BookId)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("BOOK ID")
                    .IsFixedLength();

                entity.Property(e => e.BorrowedDate)
                    .HasColumnType("date")
                    .HasColumnName("BORROWED DATE");

                entity.Property(e => e.ClientId)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("CLIENT ID")
                    .IsFixedLength();

                entity.Property(e => e.LimitDate)
                    .HasColumnType("date")
                    .HasColumnName("LIMIT DATE");

                entity.Property(e => e.ReturnedDate)
                    .HasColumnType("date")
                    .HasColumnName("RETURNED DATE");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BorrowedBooks)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BOOK_ID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.BorrowedBooks)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CLIENT_ID");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("CATEGORY");

                entity.Property(e => e.CategoryId).HasColumnName("CATEGORY ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Cpf)
                    .HasName("PK_CLIENT_ID");

                entity.ToTable("CLIENT");

                entity.Property(e => e.Cpf)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("CPF")
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("date")
                    .HasColumnName("REGISTRATION DATE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
