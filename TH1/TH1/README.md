# E-commerce API with Design Patterns

This project is an ASP.NET Core Web API for a simple e-commerce system. It demonstrates the use of several design patterns within a clean architecture.

## Features

### User
- Register/Login (JWT Authentication)
- View product list
- View product details
- Create an order
- View order history

### Admin
- Manage Products (CRUD)
- Manage Orders (View all)
- Manage Users (Not Implemented)
- System Logging

## Technology Stack

- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- JWT Authentication

## Design Patterns Used

### 1. Singleton
- **`LoggerService`**: A single instance of this service is used throughout the application to write log messages to a file (`log.txt`). It's implemented using a thread-safe lazy initialization.
- **Location**: `TH1/Patterns/Singleton/LoggerService.cs`
- **Usage**: Injected and used in controllers and services to log important events.

### 2. Factory Method
- **`PaymentServiceFactory`**: This pattern is used to create different types of payment services (`Cash`, `Paypal`, `VNPay`) based on user input.
- **Location**: `TH1/Patterns/FactoryMethod/`
- **Usage**: In the `OrderService`, a specific payment factory is chosen based on the `paymentMethod` string in the order request.

### 3. Abstract Factory
- **`INotificationFactory`**: This pattern is used to create families of related objects. In this project, it creates notification services (`Email` or `SMS`).
- **Location**: `TH1/Patterns/AbstractFactory/`
- **Usage**: In the `AuthService` and `OrderService`, a notification factory is used to create and send notifications for events like registration and order confirmation. The default is `EmailNotificationFactory` but can be changed in `Program.cs`.

### 4. Builder
- **`OrderBuilder`**: This pattern is used to construct a complex `Order` object step by step.
- **Location**: `TH1/Patterns/Builder/`
- **Usage**: In the `OrderService`, the `OrderBuilder` is used to assemble the `Order` object from customer information, order items, payment method, and shipping address.

## Project Structure

```
Project
 ├── Controllers
 ├── Services
 ├── Repositories
 ├── Models
 ├── DTOs
 ├── Patterns
 │   ├── Singleton
 │   ├── FactoryMethod
 │   ├── AbstractFactory
 │   └── Builder
 └── Data
```

## How to Run the Project

### 1. Prerequisites
- .NET 8 SDK
- SQL Server

### 2. Install NuGet Packages
You need to install the required packages. Open a terminal in the `TH1` directory and run the following commands:
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Swashbuckle.AspNetCore
dotnet add package BCrypt.Net-Next
```

### 3. Database Setup
- **Connection String**: Open `TH1/appsettings.json` and make sure the `DefaultConnection` string points to your SQL Server instance.
- **Migrations**: Run the following commands in the terminal in the `TH1` directory to create and apply the database migrations:
```bash
dotnet ef migrations add InitialCreate --project TH1.csproj --startup-project TH1.csproj
dotnet ef database update --project TH1.csproj --startup-project TH1.csproj
```

### 4. Run the Application
Run the project from your IDE or by using the following command in the `TH1` directory:
```bash
dotnet run
```
The API will be available at `https://localhost:7081` or `http://localhost:5107`. You can access the Swagger UI at `https://localhost:7081/swagger`.

## Sample API Requests (Postman)

You can import the following JSON into Postman to test the API.

(See `postman_collection.json` file)
