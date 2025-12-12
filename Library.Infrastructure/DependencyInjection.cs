using Library.Application.Interfaces.Book;
using Library.Application.Interfaces.Loan;
using Library.Application.Mappings;
using Library.Application.Services.Book;
using Library.Application.Services.Loan;
using Library.Domain.Ports.Out;
using Library.Infrastructure.Persistence.Context;
using Library.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Obtener variables de entorno
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
            var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "LibraryDB";
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";

            // Construir connection string
            var connectionString = $"Server={host};" +
                                 $"Port={port};" +
                                 $"Database={database};" +
                                 $"User={user};" +
                                 $"Password={password};" +
                                 "Charset=utf8mb4;";

            // Registrar DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
                )
            );

            // Registrar Repositorios
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            // Registrar UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
