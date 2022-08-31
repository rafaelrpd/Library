﻿// <auto-generated />
using System;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(LIBRARYContext))]
    [Migration("20220831144846_BorrowedBooksFixDates")]
    partial class BorrowedBooksFixDates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Api.Models.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AUTHOR ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorId"), 1L, 1);

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("NAME");

                    b.Property<DateTime?>("RegistrationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("REGISTRATION DATE")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.HasKey("AuthorId");

                    b.ToTable("AUTHOR", (string)null);
                });

            modelBuilder.Entity("Api.Models.Book", b =>
                {
                    b.Property<string>("Isbn")
                        .HasMaxLength(13)
                        .IsUnicode(false)
                        .HasColumnType("char(13)")
                        .HasColumnName("ISBN")
                        .IsFixedLength();

                    b.Property<int>("AuthorId")
                        .HasColumnType("int")
                        .HasColumnName("AUTHOR ID");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("CATEGORY ID");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("QUANTITY");

                    b.Property<string>("Title")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("TITLE");

                    b.HasKey("Isbn")
                        .HasName("PK_BOOK_ID");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.ToTable("BOOK", (string)null);
                });

            modelBuilder.Entity("Api.Models.BorrowedBook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("BookId")
                        .IsRequired()
                        .HasMaxLength(13)
                        .IsUnicode(false)
                        .HasColumnType("char(13)")
                        .HasColumnName("BOOK ID")
                        .IsFixedLength();

                    b.Property<DateTime?>("BorrowedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("BORROWED DATE")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(11)
                        .IsUnicode(false)
                        .HasColumnType("char(11)")
                        .HasColumnName("CLIENT ID")
                        .IsFixedLength();

                    b.Property<DateTime?>("LimitDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("LIMIT DATE")
                        .HasDefaultValueSql("DATEADD(DAY, 30, GETUTCDATE())");

                    b.Property<DateTime?>("ReturnedDate")
                        .HasColumnType("date")
                        .HasColumnName("RETURNED DATE");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("ClientId");

                    b.ToTable("BORROWED BOOK", (string)null);
                });

            modelBuilder.Entity("Api.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CATEGORY ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"), 1L, 1);

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("NAME");

                    b.HasKey("CategoryId");

                    b.ToTable("CATEGORY", (string)null);
                });

            modelBuilder.Entity("Api.Models.Client", b =>
                {
                    b.Property<string>("Cpf")
                        .HasMaxLength(11)
                        .IsUnicode(false)
                        .HasColumnType("char(11)")
                        .HasColumnName("CPF")
                        .IsFixedLength();

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("NAME");

                    b.Property<DateTime?>("RegistrationDate")
                        .HasColumnType("date")
                        .HasColumnName("REGISTRATION DATE");

                    b.HasKey("Cpf")
                        .HasName("PK_CLIENT_ID");

                    b.ToTable("CLIENT", (string)null);
                });

            modelBuilder.Entity("Api.Models.Book", b =>
                {
                    b.HasOne("Api.Models.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .IsRequired()
                        .HasConstraintName("FK_AUTHOR_ID");

                    b.HasOne("Api.Models.Category", "Category")
                        .WithMany("Books")
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("FK_CATEGORY_ID");

                    b.Navigation("Author");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Api.Models.BorrowedBook", b =>
                {
                    b.HasOne("Api.Models.Book", "Book")
                        .WithMany("BorrowedBooks")
                        .HasForeignKey("BookId")
                        .IsRequired()
                        .HasConstraintName("FK_BOOK_ID");

                    b.HasOne("Api.Models.Client", "Client")
                        .WithMany("BorrowedBooks")
                        .HasForeignKey("ClientId")
                        .IsRequired()
                        .HasConstraintName("FK_CLIENT_ID");

                    b.Navigation("Book");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Api.Models.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("Api.Models.Book", b =>
                {
                    b.Navigation("BorrowedBooks");
                });

            modelBuilder.Entity("Api.Models.Category", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("Api.Models.Client", b =>
                {
                    b.Navigation("BorrowedBooks");
                });
#pragma warning restore 612, 618
        }
    }
}
