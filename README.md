# 📱 Mobile Shop in ASP.NET Core MVC

An eCommerce web application built with **ASP.NET Core MVC** for buying and selling mobile phones and accessories. 
💼 This project includes user and admin panels, full **cart management**, and **Stripe payment integration** 💳.

---

## 🚀 Features

### 👤 User Panel
- 🛒 Add to Cart, Remove, Increase/Decrease Quantity
- 🔍 Product Filtering and Search
- 💳 Secure Checkout with **Stripe Payment Gateway**
- 📄 Order Summary & Receipt
- 📝 Ratings and Reviews (Site-Wide)
- 🔐 User Registration & Login

### 🛠️ Admin Panel
- 📦 Product Management (CRUD)
- 🗂️ Category & Brand Management
- 🛒 Order Management
- 👤 User Listing and Management
- 📊 Dashboard Analytics (Optional)

---

## 🧩 Technologies Used

- 🧱 ASP.NET Core MVC
- 💾 SQL Server
- 🎨 Bootstrap 5
- 🧠 Entity Framework Core
- 🔐 Cookie Authentication
- 🛍️ Stripe Payment Integration
- 📦 Session Management

---

### 🔧 **1. Project Type**
- 🧩 **ASP.NET Core MVC Web Application**
- 🛠️ Based on **Model-View-Controller (MVC)** architecture
- 🐱‍🏍 Backend: **C#**
- 🖼️ Frontend: **Razor Views (.cshtml)** + **Bootstrap**
- 🗃️ Database: **SQL Server**

---

### 💡 **2. Key Features**

| 🌟 Feature              | 💬 Description |
|------------------------|----------------|
| 📱 Mobile Listing       | Show all mobile products with image, price, etc. |
| 🔎 Search & Filters     | Filter mobiles by brand, name, or price. |
| 🛒 Add to Cart          | Add selected mobiles to cart (stored in session). |
| 💳 Checkout & Payment   | Integrated with **Stripe** for secure payments. |
| 🔐 User Auth            | Login/Logout using **cookies & session**. |
| 🧑‍💼 Admin Panel         | Admin can manage products, categories, and orders. |

---

### 🔌 **3. APIs Used**

#### ✅ **Stripe API** 🧾
- 🧷 Used for payment gateway during checkout.
- 💵 Secure online payment processing.
- 📦 Integrated via **Stripe.NET SDK**.

#### ⚠️ (Optional) **Razorpay API** 💳
- 📂 A folder exists but integration seems **incomplete** or **optional**.

---

### 📦 **4. Third-Party Libraries & Tools**

| 📚 Library | 🔍 Purpose |
|------------|-----------|
| `Stripe.net` | Stripe payment gateway integration |
| `Microsoft.EntityFrameworkCore` | ORM to connect to SQL Server |
| `Microsoft.AspNetCore.Session` | Manage cart and user sessions |
| `Microsoft.AspNetCore.Authentication.Cookies` | User login/logout via cookies |
| `Bootstrap` | UI framework for responsive design |
| `Font Awesome` (optional) | Icons and symbols |
| `jQuery` (optional) | For frontend scripting (AJAX, animations, etc.) |

---

### 🗃️ **5. Database & ORM**

- 🛢️ **SQL Server** used to store:
  - 📦 Products
  - 🛍️ Orders
  - 👤 Users
  - 🏷️ Categories
- 🧠 Managed using **Entity Framework Core (EF Core)**.
- 🧱 Migrations used to evolve database structure.

---

### 🔐 **6. Authentication & Authorization**

- 🧑 User login via **cookies**
- 📛 Admin and Normal User roles supported
- 🧾 Secure session management

---

### 🧑‍💼 **7. Admin Panel Features**

| 🧰 Feature | 📝 Description |
|-----------|----------------|
| ➕ Add Product | Insert new mobiles with name, price, image |
| ♻️ Update Product | Modify existing mobile info |
| ❌ Delete Product | Remove mobiles from the store |
| 📃 Order List | View all placed orders by customers |
| 📂 Manage Categories | Add or edit categories like Samsung, iPhone, etc. |

---

### 🎨 **8. UI Design & Layout**

- 📄 **Razor Views (.cshtml)** used for frontend pages
- 🎨 **Bootstrap 5** for layout and responsiveness
- 🧭 Navigation in `_Layout.cshtml`
- 🛍️ Pages like `Home`, `Products`, `Cart`, `Checkout`, `Login`, `Admin`

---

### 🧠 **9. Session Management**

- 🧰 Used to maintain:
  - 🛒 Cart data
  - 👤 Logged-in user info
- 💡 Setup in `Startup.cs`:

```csharp
services.AddSession();
app.UseSession();
```

---

### 📁 **10. Project Folder Structure**

| 📂 Folder | 📌 Contents |
|----------|-------------|
| `Controllers/` | All MVC logic (Admin, Home, Cart, etc.) |
| `Models/` | C# classes like Mobile, Order, User |
| `Views/` | All `.cshtml` Razor pages |
| `wwwroot/` | CSS, JS, images |
| `Data/` or `Context/` | EF Core `DbContext` |
| `Stripe/` | Stripe helper/config classes |
| `Razorpay/` | (Optional) Razorpay integration folder |

---


![image](https://github.com/user-attachments/assets/66e5d50b-3763-418b-b3af-6bc126a0a4c7)

![image](https://github.com/user-attachments/assets/db945eb7-62f0-4831-99c0-540fe9142ef0)

![image](https://github.com/user-attachments/assets/4b3aae1d-e380-4abb-8896-f3fe2b698bd4)

![image](https://github.com/user-attachments/assets/de0abc01-db6f-4889-b5e9-8833253c95a3)

![image](https://github.com/user-attachments/assets/a5727153-c2b1-433b-8981-6543036ecd00)

![image](https://github.com/user-attachments/assets/37f00d39-81de-4e47-a3bc-09fdb82c9db2)

![image](https://github.com/user-attachments/assets/715d1913-e6aa-4283-9db7-02ad83098a1a)

![image](https://github.com/user-attachments/assets/2945af28-84b6-4ab5-bec4-233b4db9ae5d)

![image](https://github.com/user-attachments/assets/5a4750ef-fd38-448d-aecf-69917a359519)

![image](https://github.com/user-attachments/assets/1161b87d-317f-45fa-94a3-1161f7927710)

![image](https://github.com/user-attachments/assets/0b7c8b0e-3eab-41cd-a534-2aa9334c3de5)

![image](https://github.com/user-attachments/assets/45d29841-8648-4ff9-bf20-5f9dc59533ab)


