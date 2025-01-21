# ECommerce App

## Overview
This is a **full-fledged ECommerce application** built using modern technologies. It features **authentication, product management, shopping cart, order processing, and secure payments**. The backend is built with **.NET 8 Web API**, while the frontend is a **cross-platform .NET MAUI client**.
---
## Project Demo

Click the image below to watch the demo video:

[![Project Demo](https://img.youtube.com/vi/_M6Fcwj23rY/0.jpg)](https://www.youtube.com/watch?v=_M6Fcwj23rY)


---

## **Tech Stack**
### **Backend (API)**
- **.NET 8.0 Web API**
- **Entity Framework Core 9.0** (Code-first migrations, Repository pattern)
- **PostgreSQL** (Database)
- **JWT Authentication** (Token-based security)
- **Serilog** (Structured logging with sinks for Console, File, and Debug)
- **Newtonsoft.Json** (JSON serialization/deserialization)
- **Swagger (Swashbuckle)** (API documentation)
- **Dependency Injection** (Built-in DI container)
- **ASP.NET Core Middleware** (Exception handling, CORS, Authentication, Logging)
- **Task-based Asynchronous Programming** (Async/Await)

### **Client (Cross-Platform Mobile/Desktop)**
- **.NET MAUI** (Multi-platform UI framework)
- **CommunityToolkit.MVVM** (ViewModel-First development, Bindings, Messaging)
- **HttpClient & REST API Integration** (API communication)
- **SQLite** (Local DB for Caching)
- **Syncfusion UI Components** (ListView, Inputs, Charts)
- **Dependency Injection** (For services and repositories)
- **Data Binding** (ObservableCollections, Command Binding)
- **File Picker and Image Handling**
- **Shell Navigation** (Routing between Views)

---

## **Key Features**
### **Backend (ASP.NET Web API)**
#### **Authentication & Authorization**
- **JWT-based authentication**
- Secure **user registration & login**
- **User claims and role-based access**
- **Session validation API** (`GET /api/auth/session-status`)
- **Logout API** (`POST /api/auth/logout`)

#### **Product Management**
- CRUD operations (**Add, update, delete products**)
- Fetch products by category
- **Pagination and filtering**
- **Image handling** for product images

#### **Shopping Cart**
- Add/remove products
- Update **cart item quantity** (`PUT /api/cart/update/{productId}`)
- **Fetch cart summary** (`GET /api/cart/summary`)
- Clear cart

#### **Orders & Payment**
- Create and confirm orders
- **Payment simulation & validation**
- Fetch user order history

#### **Logging & Monitoring**
- **Serilog structured logging**
- File-based logs
- Console and Debug logs

#### **Middleware Implementations**
- **Global exception handling**
- **CORS policy management**
- Authentication middleware

#### **Swagger API Documentation**
- Integrated via **Swashbuckle**
- **Interactive API testing**

---

### **Client (MAUI)**
#### **Cross-Platform UI**
- **Supports Android, iOS, Windows, MacCatalyst**
- **Modern UI/UX with Syncfusion components**
- **Adaptive layouts for different screen sizes**

#### **Authentication & Session Management**
- **Secure login/logout**
- **Token-based authentication**
- **Session persistence on app restart**

#### **Product Catalog & Search**
- **View products by category**
- **Search functionality**
- **Product image display**

#### **Shopping Cart & Checkout**
- Add/remove items from cart
- Update cart item quantity
- **Proceed to checkout**
- **Payment validation & processing**

#### **Order History & Tracking**
- Fetch user order history
- View order details
- **Push notifications for order updates**

#### **MVVM Pattern**
- **CommunityToolkit.MVVM** for data binding
- **Async commands & Observables**

#### **Local Storage & Caching**
- **SQLite for offline storage**
- **Async data fetching & caching**

#### **Dark Mode Support**
- **Toggle between light & dark modes**

---

## **API Endpoints**
### **Auth**
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login (JWT)
- `GET /api/auth/me` - Get current user info
- `POST /api/auth/logout` - Logout user
- `GET /api/auth/session-status` - Check session validity

### **Cart**
- `GET /api/cart` - Get user's cart
- `POST /api/cart/add` - Add product to cart
- `PUT /api/cart/update/{productId}` - Update item quantity
- `GET /api/cart/summary` - Fetch cart total price & items count
- `DELETE /api/cart/remove` - Remove item from cart
- `DELETE /api/cart/clear` - Clear entire cart

### **Orders**
- `POST /api/orders/create` - Create a new order
- `POST /api/orders/confirm-payment/{orderId}` - Confirm order payment
- `GET /api/orders/mine` - Fetch user's order history

### **Products**
- `GET /api/products` - Get all products
- `POST /api/products` - Add a new product
- `GET /api/products/{id}` - Get product by ID
- `DELETE /api/products/{id}` - Delete product by ID
- `PUT /api/products` - Update a product
- `GET /api/products/categories` - Get all product categories
- `POST /api/categories` - Create a new category
- `DELETE /api/categories/{id}` - Delete a category

---

## **Getting Started**
### **Prerequisites**
- **.NET 8 SDK**
- **PostgreSQL Database**
- **Visual Studio 2022** (With MAUI Workloads Installed)

### **Setup Instructions**
#### **Backend (API)**
1. Clone the repository
2. Navigate to `ECommerce.Api/` and configure `appsettings.json` for database connection
3. Run migrations: `dotnet ef database update`
4. Start the API: `dotnet run`

#### **Client (MAUI App)**
1. Navigate to `Ecommerce.Client/`
2. Run `dotnet build` and `dotnet run`
3. Deploy to desired platform (Android/iOS/Windows)

---

## **Contributing**
### **Contributions are welcome!** Follow these steps:
1. **Fork the repo**
2. **Create a new branch** (`feature-xyz`)
3. **Commit and push your changes**
4. **Submit a pull request**

---

## **License**
**MIT License**. See LICENSE file for details.

