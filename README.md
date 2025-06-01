# Evenza Backend - Event Management API

A robust and scalable backend API for the Evenza event management platform, built with ASP.NET Core and SQL Server. Provides comprehensive event management capabilities with secure authentication and role-based access control.

## 🚀 Features

### Core Functionality
- **Event Management**: Full CRUD operations for events with detailed information
- **User Management**: Complete user registration, authentication, and profile management
- **Registration System**: Handle event registrations with capacity management
- **Admin Panel**: Administrative controls for platform management

### API Endpoints
- **Authentication**: Login, register, password management
- **Events**: Create, read, update, delete events
- **Users**: User profiles, settings, registration history
- **Registrations**: Event registration management
- **Admin**: Administrative functions and user management

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core 9.0+
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **API Documentation**: Swagger/OpenAPI
- **Validation**: FluentValidation
- **Mapping**: AutoMapper

## 🔐 Security Features

- JWT-based authentication and authorization
- Role-based access control (Admin/User roles)
- Password hashing with BCrypt
- CORS configuration for secure cross-origin requests
- Input validation and sanitization
- SQL injection protection via Entity Framework

## 📊 Database Design

- User management with role assignments
- Event scheduling and capacity management
- Registration tracking with timestamps
- Relational data integrity with foreign keys
- Optimized queries for performance

## 🏗️ Architecture

- Clean Architecture principles
- Repository pattern implementation
- Dependency injection container
- Layered application structure
- RESTful API design standards

## 📈 Performance

- Async/await patterns for scalability
- Efficient database queries
- Response caching where appropriate
- Optimized Entity Framework configurations

## 🔧 Configuration

- Environment-based configuration
- Database connection string management
- JWT secret key configuration
- CORS policy setup
