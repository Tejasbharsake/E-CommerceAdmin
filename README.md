# RahiShop – ASP.NET MVC E-Commerce Admin System

## 📌 Project Overview
RahiShop is an enterprise-grade **ASP.NET MVC e-commerce admin management system** built using a **3-tier architecture**.  
It focuses on backend administrative workflows such as seller onboarding, product approval, order processing, payments, refunds, offers, commissions, complaints, and dashboard analytics.

The project follows clean architecture principles with strict separation between UI, business logic, and data access layers.

---

## 🏗️ Architecture

### 3-Tier Structure
RahiShop Solution
│
├── GSTECommerce
│ ├── Controllers
│ ├── Views
│ │ ├── Home
│ │ ├── Admin
│ │ └── Shared
│ ├── Content
│ ├── Scripts
│ └── Web.config
│
├── GSTECommerceLibrary
│ └── Business Logic & Models
│
└── GSTECommerceHelper
└── Data Access Layer

yaml
Copy code

### Layer Responsibilities
- **UI Layer (GSTECommerce)**  
  Controllers, Views, Layouts, Partial Views, Client-side logic
- **Business Layer (GSTECommerceLibrary)**  
  Business rules, validations, domain models
- **Data Layer (GSTECommerceHelper)**  
  Database access, helpers, integrations

---

## 🎯 Core Features
- Admin authentication & profile management  
- Seller registration, verification, and management  
- Product approval & rejection workflow  
- Customer management  
- Order lifecycle management  
- Online & COD payment handling  
- Refund processing  
- Offer & commission management  
- Complaint & feedback handling  
- Dashboard with real-time analytics  
- Modular UI using partial views  

---

## 🎮 Controllers
- **AdminController**  
  Handles admin dashboard, seller, product, order, payment, refund, offer, and commission operations.
- **HomeController**  
  Manages public pages like Home, About, and Contact.

---

## 📦 Models (Business Layer)
Models are maintained in a separate class library to ensure loose coupling.

- **Admin** – Admin entity definition  
- **BALAdmin** – Business access logic for admin-related operations  

---

## 🖥️ Views Structure

### Home Views
- `Index.cshtml`
- `About.cshtml`
- `Contact.cshtml`

### Admin Views
- Dashboard  
- Seller Management & Registration  
- Product Management & Approval  
- Order Management  
- Payment & Refund Management  
- Offer & Commission Management  
- Complaint Management  
- Feedback Management  
- Profile & Password Management  

### Partial Views
- Dashboard widgets  
- Order details & invoices  
- Seller and product detail components  
- Refund modals (COD & Online)  
- Profile, payment, and commission components  

**Total `.cshtml` files:** 67

---

## 🎨 UI & Frontend Stack
- **Bootstrap 5.2.3**
- **jQuery 3.7.0**
- jQuery Validation
- jsPDF (Invoice & PDF generation)
- Modernizr
- Custom Admin Dashboard CSS

---

## ⚙️ Configuration Files
- `_ViewStart.cshtml` – Default layout configuration  
- `Web.config` – MVC, routing, and view engine configuration  
- `_Layout.cshtml` – Main site layout  
- `_AdminLayout.cshtml` – Admin dashboard layout  

---

## 📊 Dashboard Capabilities
- Active offers overview  
- Order status tracking  
- Seller & customer statistics  
- Complaint and return monitoring  
- Payment, refund, and revenue summaries  

---

## 👨‍💻 Module Ownership (Developer Tags)
- **TB** – Authentication & Profile Management  
- **SC** – Seller & Product Management  
- **AB** – Customer & Order Management  
- **PK** – Offer & Commission Management  
- **SG** – Payment & Refund Management  
- **HK** – Courier & Payment History  
- **NM** – Dashboard Components  

---

## 🚀 How to Run the Project
1. Open the solution in **Visual Studio**
2. Restore all NuGet packages
3. Configure database connection in `Web.config`
4. Build the solution
5. Run the application using **IIS Express**
6. Access the admin panel via browser

---

## 👥 Intended Users
- E-commerce administrators  
- Operations & support teams  
- Seller onboarding teams  
- Finance & payment teams  

---

## ⭐ Project Highlights
- Enterprise-level MVC architecture  
- Strong separation of concerns  
- Highly modular and maintainable UI  
- Real-world e-commerce admin workflows  
- Scalable and extensible design  

---

## 📄 License
This project is intended for **learning, internal use, and enterprise customization**.
