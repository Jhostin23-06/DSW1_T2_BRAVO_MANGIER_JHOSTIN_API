using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Persistence.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            // Nombre de la tabla
            builder.ToTable("Books");

            // Clave primaria
            builder.HasKey(e => e.Id);

            // Propiedades
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Author)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.ISBN)
                .IsRequired()
                .HasMaxLength(20);

            // Índice único para ISBN
            builder.HasIndex(e => e.ISBN)
                .IsUnique();

            builder.Property(e => e.Stock)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")  // ← Cambiado a (6)
                .ValueGeneratedOnAdd();

            // Relaciones
            builder.HasMany(e => e.Loans)
                .WithOne(e => e.Book)
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuraciones adicionales
            builder.Property(e => e.Title)
                .HasAnnotation("Description", "Título del libro");

            builder.Property(e => e.Author)
                .HasAnnotation("Description", "Autor del libro");

            builder.Property(e => e.ISBN)
                .HasAnnotation("Description", "Código único del libro");
        }
    }
}
