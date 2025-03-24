# 🚘Vehicle Management API

## Descrição
This API was developed to be a minimal api, and to provide vehicle administration functionalities.

## 🛠️ Technologies
- **.NET 8**
- **ASP.NET Core**
- **Entity Framework Core**
- **SQL Server**
- **Swagger**

## How to run

### **1. 📦 Installation**
```bash
git clone https://github.com/luisstarlino/.NET-MinimalVehiclesAPI
cd .NET-MinimalVehiclesAPI
```

### **2. Database configuration**
Make sure you have SQL Server installed and configure the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR;Database=DB_NAME;User Id=YOUR_ID;Password=YOUR_PASSWORD;"
}
```

### **3. Perform Migrations and Update the Database**
```bash
dotnet ef database update
```

### **4. Running the Server**
```bash
dotnet run
```
The API will be available at `http://localhost:5000` (or the port configured in `launchSettings.json`).

## **Endpoints**

### **Authentication**
#### **Login**
`POST /admin/login`
- **Body:**
  ```json
  {
    "email": "admin@example.com",
    "password": "123456"
  }
  ```
- **Response:**
  ```json
  {
    "token": "jwt-token-gerado"
  }
  ```

### **Vehicle**
#### **Criar um Veículo**
`POST /vehicles`
- **Headers:** `Authorization: Bearer <token>`
- **Body:**
  ```json
  {
    "model": "Toyota Corolla",
    "year": 2022,
    "color": "Black",
    "price": 75000
  }
  ```
- **Response:**
  ```json
  {
    "id": 1,
    "model": "Toyota Corolla",
    "year": 2022,
    "color": "Black",
    "price": 75000
  }
  ```

#### **Update**
`PUT /vehicles/{id}`
- **Headers:** `Authorization: Bearer <token>`
- **Body:**
  ```json
  {
    "model": "Toyota Corolla",
    "year": 2023,
    "color": "White",
    "price": 78000
  }
  ```
- **Resposta:**
  ```json
   {
    "model": "Toyota Corolla",
    "year": 2023,
    "color": "White",
    "price": 78000
  }
  ```

## **Authentication and Security**
- The API uses **JWT (JSON Web Token)** for authentication.
- All protected routes require a valid token in the header `Authorization: Bearer <token>`.
