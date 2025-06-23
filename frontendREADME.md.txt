frontend/README.md

# 🛍️ eCommerce Frontend – React + Vite + Tailwind CSS

## 📌 Overview

This is the **frontend** of a full-stack eCommerce web application, developed using **React**, **Vite**, and **Tailwind CSS**. The frontend communicates with multiple backend microservices built in ASP.NET Core via RESTful APIs. It provides a responsive, modern user interface for browsing products, user authentication, order placement, and more.

---

## ⚙️ Technologies Used

- ✅ React 18 (Functional Components + Hooks)
- ✅ Vite (for lightning-fast development)
- ✅ Tailwind CSS (utility-first responsive design)
- ✅ Axios (for HTTP requests)
- ✅ React Router (client-side routing)
- ✅ LocalStorage (for token handling)
- ✅ Fetch or Axios for REST API communication

---

## 📁 Folder Structure
/src
/pages → All main pages (Home.jsx, Product.jsx, Login.jsx, Cart.jsx, etc.)
/components → Reusable UI components (Navbar, Footer, ProductCard, etc.)
/services → API calls to backend microservices (auth, products, orders)
/assets → Images, icons, static content
/styles → Global/custom styles (optional)
App.jsx → App layout and routes
main.jsx → React root entry point
vite.config.js → Vite configuration
package.json → Project metadata and scripts

## 🚀 Setup Instructions

### ✅ Prerequisites

- Node.js (v18 or higher)
- npm or yarn
- Visual Studio Code (recommended)

---

### 🧪 Running the Frontend

1. **Clone the repository**
```bash
git clone https://github.com/yourusername/ecommerce-frontend.git
cd ecommerce-frontend

2.Install dependencies

npm install

3.Configure API endpoints
Edit the base URLs inside src/services/ files (e.g., authService.js, productService.js, orderService.js):
// Example for ProductService
const BASE_URL = 'https://localhost:5002/api/products';

4.Start the development server
npm run dev
5.Visit in browser
http://localhost:5173

 Features
User Registration & Login (with JWT)

Role-based views (Admin/User)

Product Browsing

Shopping Cart (Client-side)

Place Orders via OrderService

Responsive Layout with Tailwind

Error Handling & Loading States


6.Authentication

Login connects to AuthService and stores JWT token in LocalStorage

Token is sent with each protected request (Authorization header)

Routes are protected based on login state (optional route guards)

7.API Integration
AuthService: Login / Register

ProductService: Get all products / Get by ID

OrderService: Place orders / View history

All services are called via axios in src/services/ files.

8.Testing (Manual)
Use browser dev tools or Swagger to verify backend endpoints

Use the frontend UI to test login, view products, and place orders

Notes
Ensure CORS is enabled in all backend microservices for http://localhost:5173

Backend must be running for full functionality

Summary
This frontend is fully integrated with a .NET-based microservices backend and provides a clean, modern, responsive UI for a complete eCommerce experience. Built with scalable architecture and modern tooling, it's ready for development, testing, and deployment.

