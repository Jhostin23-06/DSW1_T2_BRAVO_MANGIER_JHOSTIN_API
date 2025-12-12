# 📚 Sistema de Gestión de Biblioteca - Backend API

## 📋 Descripción
API RESTful para gestión de biblioteca universitaria desarrollada con ASP.NET Core 8.0, utilizando Arquitectura Hexagonal (Puertos y Adaptadores), Entity Framework Core con MySQL, y patrones Repository y Unit of Work.

## 🏗️ Arquitectura del Proyecto

### Library.API/           # Capa de Presentación (Controllers, Program.cs)
### Library.Application/   # Capa de Aplicación (Services, DTOs, Interfaces)
### Library.Domain/        # Capa de Dominio (Entities, Ports, Exceptions)
### Library.Infrastructure/# Capa de Infraestructura (Repositories, DbContext, Config)

## ⚙️ Requisitos Previos
### Software necesario:
- .NET 8.0 SDK
- MySQL Server 8.0+
- Git
### IDE (a elección):
- Visual Studio 2022+
- Visual Studio Code

### Cuentas necesarias:
MySQL instalado y corriendo

## 🚀 Configuración Inicial
1. Clonar el repositorio

```
git clone https://github.com/Jhostin23-06/DSW1_T2_BRAVO_MANGIER_JHOSTIN_API.git
cd DSW1_T2_BRAVO_MANGIER_JHOSTIN_API
```

2. Configurar variables de entorno
   
Crear el archivo .env con tus credenciales de MySQL en la carpeta principal Library.API

Ejemplo: 

```
DB_HOST=localhost
DB_PORT=3306
DB_NAME=LibraryDB
DB_USER=root
DB_PASSWORD=tu_contraseña_aqui
```

## 🔧 Instalación y Ejecución

### Opción A: Usando Visual Studio 2022+

Abrir Library.API.sln en Visual Studio

Restaurar paquetes NuGet (se hace automáticamente)

Presionar F5 o Ctrl+F5 para ejecutar

### Opción B: Usando Visual Studio Code

```

Abrir carpeta en VS Code

code .

Abrir terminal integrada (Ctrl + `)

Restaurar paquetes

dotnet restore

Ejecutar proyecto

dotnet run --project Library.API

```

### Opción C: Usando línea de comandos (Recomendado para visual studio code ejecutando las migraciones)

```
Navegar a la carpeta del proyecto
cd Library.API

1. Restaurar dependencias
dotnet restore

2. Aplicar migraciones
dotnet ef database update --project ../Library.Infrastructure --startup-project .

3. Ejecutar la aplicación
dotnet run

4. Abrir navegador en (de acuerdo a la url que te indique): 
http://localhost:5000 (Example)
https://localhost:5001 (Example)
```

### Crear migraciones (si es necesario)

```

Desde la carpeta Library.Infrastructure

cd Library.Infrastructure

Crear nueva migración

dotnet ef migrations add InitialCreate --startup-project ../Library.API

Aplicar migración

dotnet ef database update --startup-project ../Library.API

```

## 🌐 Endpoints de la API

### 📚 Libros
Método	    Endpoint	                      Descripción
  GET	      /api/books	                Listar todos los libros
  GET	      /api/books/available	  Libros disponibles para préstamo
  GET	      /api/books/{id}            	Obtener libro por ID
  GET	      /api/books/isbn/{isbn}	   Obtener libro por ISBN
  POST	    /api/books	                  Crear nuevo libro
  PUT	      /api/books/{id}	              Actualizar libro
  DELETE	  /api/books/{id}	                Eliminar libro
### 📝 Préstamos
Método	            Endpoint	                        Descripción
GET	              /api/loans	                  Listar todos los préstamos
GET	              /api/loans/active	                Préstamos activos
GET	              /api/loans/student/{name}	      Préstamos por estudiante
GET	              /api/loans/book/{id}	            Préstamos por libro
POST	            /api/loans	                      Crear nuevo préstamo
PUT	              /api/loans/{id}/return	           Devolver préstamo
DELETE	          /api/loans/{id}	                  Eliminar préstamo


## 👥 Autores

Jhostin Bravo Mangier

Curso: Desarrollo de Servicios Web I

Cibertec

## 📄 Licencia

Este proyecto es para fines educativos como parte de la evaluación T2 del curso DSW1.

