# Parcel Delivery API

A .NET Web API for managing parcel deliveries, orders, and department classification.

## Overview

This project provides endpoints to manage:
- **Parcels**: Tracks weight, value, approval status, and recipient details.
- **Orders**: Groups parcels for shipment.
- **Departments**: Dynamically classifies parcels based on weight limits (e.g., Mail, Regular, Heavy).
- **Approvals**: Automated approval logic based on parcel value.

## Features

- **Dynamic Department Classification**: 
  - Departments are stored in the database with configurable weight limits.
  - Parcels are automatically assigned to a department upon approval based on their weight.
  - Default departments: Mail (< 1kg), Regular (< 10kg), Heavy (> 10kg), Insurance (Manual).
- **Automated Approvals**: Parcels below a certain value threshold are automatically approved.
- **Order Management**: Create orders with multiple parcels and recipients.
- **Recipient Management**: Stores recipient details and addresses.

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server/) (LocalDB or full instance)
- A tool for API testing (e.g., Postman, curl, or the built-in Swagger UI)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd parcelDeliveryAPI
```

### 2. Configure Database Connection

Update the `appsettings.json` file with your SQL Server connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ParcelDeliveryDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 3. Apply Migrations

This project uses Entity Framework Core. To set up the database schema and seed initial data (Departments):

```bash
dotnet ef database update
```

*(Note: If you encounter issues with Department seeding, ensure your database is clean or migrations are applied sequentially.)*

### 4. Run the Application

```bash
dotnet run
```

The API will start (default port is typically `http://localhost:5000` or `https://localhost:5001`).

### 5. Verify Installation

Open your browser and navigate to the Swagger UI to explore the API endpoints:

```
https://localhost:7084/swagger/index.html
```
*(Check the terminal output for the exact port number)*

## API Endpoints

### Departments
- `GET /api/Department`: List all departments and their weight limits.
- `POST /api/Department`: Create a new department.
- `PUT /api/Department/{id}`: Update department rules.

### Parcels
- `GET /api/Parcels`: List all parcels.
- `PATCH /api/Parcels/{id}/approval`: Update approval status (triggers department classification).

### Orders
- `POST /api/Orders`: Create a new order with parcels.
- `GET /api/Orders/{id}`: Get order details.

## Project Structure

- `Controllers/`: API endpoints.
- `Data/`: EF Core DbContext and configuration.
- `Models/`: Entity definitions (Parcel, Order, Department, etc.).
- `DTO/`: Data Transfer Objects for API requests/responses.
- `Services/`: Business logic (ParcelClassifier, ApprovalClassifier).
- `DAO/`: Data Access Objects for database interactions.
