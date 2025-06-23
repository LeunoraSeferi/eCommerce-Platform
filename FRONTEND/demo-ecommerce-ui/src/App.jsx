import { BrowserRouter, Routes, Route } from "react-router-dom";
import Login from "./components/Login/Login";
import ProductList from "./components/Product/ProductList";
import Cart from "./components/Cart/Cart";
import Header from "./components/Header/Header"; // SHTO KËTË
import Orders from "./components/Orders/Orders"; // Importo
import OrderDetails from "./components/Orders/OrderDetails";


function App() {
  return (
    <BrowserRouter>
      <Header /> {/* SHTO HEADERIN KËTU */}
      <Routes>
        <Route path="/orders" element={<Orders />} />
        <Route path="/orders/:id" element={<OrderDetails />} />
        <Route path="/" element={<Login />} />
        <Route path="/products" element={<ProductList />} />
        <Route path="/cart" element={<Cart />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
