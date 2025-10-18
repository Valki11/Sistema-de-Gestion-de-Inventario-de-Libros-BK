# Sistema de Gestión de Inventario de Libros – Backend

Este proyecto implementa la API principal del **Sistema de Gestión de Inventario de Libros**, desarrollada con **.NET 8**, **Entity Framework Core** y **Oracle 21c XE**.  
Gestiona la información de **usuarios, autores, libros y préstamos**, con seguridad basada en roles (Bibliotecario y Lector).

---

## Tecnologías principales

- **ASP.NET Core 8 Web API**
- **Entity Framework Core (Oracle.EntityFrameworkCore)**
- **Oracle Database 21c XE**
- **Swagger / OpenAPI**

---

##  Arquitectura

El proyecto sigue una estructura por capas:

Inventario.API/ → Endpoints y configuración del servidor
Inventario.Application/ → DTOs y lógica de aplicación
Inventario.Domain/ → Entidades del dominio (Libro, Autor, Usuario, Prestamo)
Inventario.Infrastructure/ → Contexto EF Core, repositorios y servicios


Cada capa se comunica de forma desacoplada, aplicando los principios de **Clean Architecture**.

---

##  Entidades principales

| Entidad | Descripción |
|----------|-------------|
| `Usuario` | Representa a un usuario del sistema (Bibliotecario o Lector) |
| `Autor` | Información de los autores de los libros |
| `Libro` | Inventario de libros, con control de copias disponibles |
| `Prestamo` | Registro de préstamos realizados a usuarios lectores |

---

##  Roles del sistema

- **Bibliotecario**
  - Gestiona autores, libros y usuarios.
  - Puede registrar y devolver préstamos.
- **Lector**
  - Solo puede consultar los catálogos de libros y autores.

---

##  Endpoints principales

###  Libros
| Método | Endpoint | Descripción |
|--------|-----------|-------------|
| `GET` | `/api/Libros` | Lista de libros |
| `POST` | `/api/Libros` | Crea un nuevo libro *(solo bibliotecario)* |
| `PUT` | `/api/Libros/{id}` | Actualiza información del libro *(solo bibliotecario)* |
| `DELETE` | `/api/Libros/{id}` | Elimina un libro *(solo bibliotecario)* |

###  Autores
| Método | Endpoint | Descripción |
|--------|-----------|-------------|
| `GET` | `/api/Autores` | Lista de autores |
| `POST` | `/api/Autores` | Crea un nuevo autor *(solo bibliotecario)* |
| `PUT` | `/api/Autores/{id}` | Actualiza autor *(solo bibliotecario)* |
| `DELETE` | `/api/Autores/{id}` | Elimina autor *(solo bibliotecario)* |

###  Usuarios
| Método | Endpoint | Descripción |
|--------|-----------|-------------|
| `GET` | `/api/Usuarios` | Lista de usuarios |
| `POST` | `/api/Usuarios` | Crea usuario *(solo bibliotecario)* |
| `PUT` | `/api/Usuarios/{id}` | Actualiza usuario *(solo bibliotecario)* |

###  Préstamos
| Método | Endpoint | Descripción |
|--------|-----------|-------------|
| `POST` | `/api/Prestamos` | Crea préstamo *(solo bibliotecario)* |
| `POST` | `/api/Prestamos/{id}/devolver` | Devuelve préstamo *(solo bibliotecario)* |

---

##  Reglas de negocio

- Un **libro** no puede ser prestado si no tiene copias disponibles.  
- Solo un **usuario con rol LECTOR** puede recibir préstamos.  
- Al **prestar**, se descuenta una copia; al **devolver**, se repone.  
- Los **IDs** de las entidades se generan mediante **secuencias Oracle + triggers**.

---

## ️ Configuración de la base de datos

Ejemplo de conexión (`appsettings.json`):

```json
"ConnectionStrings": {
  "OracleDb": "User Id=BIBLIOTECA_USER;Password=admin123;Data Source=localhost:1521/XEPDB1;"
}


## Ejecucion
cd Inventario.API
dotnet build
dotnet run

La API se levanta en:
http://localhost:5188/swagger