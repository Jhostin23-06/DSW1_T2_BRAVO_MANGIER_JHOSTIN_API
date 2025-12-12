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
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            // Nombre de la tabla
            builder.ToTable("Loans");

            // Clave primaria
            builder.HasKey(e => e.Id);

            // Propiedades
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.BookId)
                .IsRequired();

            builder.Property(e => e.StudentName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.LoanDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")  // ← Cambiado a (6)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ReturnDate)
                .IsRequired(false);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasConversion(
                    v => v.ToString(),
                    v => v);

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")  // ← Cambiado a (6)
                .ValueGeneratedOnAdd();

            // Índices
            builder.HasIndex(e => e.BookId)
                .HasDatabaseName("IX_Loans_BookId");

            builder.HasIndex(e => e.StudentName)
                .HasDatabaseName("IX_Loans_StudentName");

            builder.HasIndex(e => e.Status)
                .HasDatabaseName("IX_Loans_Status");

            // Índice compuesto para búsquedas frecuentes
            builder.HasIndex(e => new { e.BookId, e.StudentName, e.Status })
                .HasDatabaseName("IX_Loans_BookId_StudentName_Status")
                .HasFilter("[Status] = 'Active'");

            // Relaciones
            builder.HasOne(e => e.Book)
                .WithMany(e => e.Loans)
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuraciones adicionales
            builder.Property(e => e.StudentName)
                .HasAnnotation("Description", "Nombre del estudiante que solicita el préstamo");

            builder.Property(e => e.Status)
                .HasAnnotation("Description", "Estado del préstamo: Active o Returned");

            builder.Property(e => e.LoanDate)
                .HasAnnotation("Description", "Fecha en que se realizó el préstamo");

            builder.Property(e => e.ReturnDate)
                .HasAnnotation("Description", "Fecha en que se devolvió el libro");
        }
    }
}
