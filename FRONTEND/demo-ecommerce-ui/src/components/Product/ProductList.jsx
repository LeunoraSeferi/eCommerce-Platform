import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "./ProductList.css";

const getImage = (name) => {
  const lower = name.toLowerCase();
  if (lower.includes("makeup")) return "/images/makeup.jpg";
  if (lower.includes("iphone")) return "/images/iphone16.webp";
  if (lower.includes("laptop")) return "/images/laptop.jpg";
  if (lower.includes("skincare")) return "/images/skincare.jpg";
  if (lower.includes("bags")) return "/images/bags.jpg";
  if (lower.includes("hoodie")) return "/images/hoodie.jpg";
  if (lower.includes("sunglasses")) return "/images/sunglasses.webp";
  if (lower.includes("smartwatch")) return "/images/smartwatch.webp";
  if (lower.includes("samba")) return "/images/samba.jpg";
  return "/images/default.png";
};

const ProductList = () => {
  const [products, setProducts] = useState([]);
  const [cart, setCart] = useState(
    JSON.parse(localStorage.getItem("cartItems")) || []
  );
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) {
      navigate("/login");
      return;
    }

    fetch("http://localhost:5003/api/Products")
      .then((res) => res.json())
      .then((data) => {
        if (Array.isArray(data)) setProducts(data);
      });
  }, [navigate]);

  const addToCart = (product) => {
    const exists = cart.find((item) => item.id === product.id);
    if (exists) {
      alert("Ky produkt është tashmë në shportë.");
      return;
    }

    const productWithImage = {
      ...product,
      imageUrl: getImage(product.name),
    };

    const updated = [...cart, productWithImage];
    setCart(updated);
    localStorage.setItem("cartItems", JSON.stringify(updated));
    alert("Produkti u shtua në shportë!");
  };

  return (
    <div className="products-container">
      {products.map((p) => (
        <div className="product-card" key={p.id}>
          <img src={getImage(p.name)} alt={p.name} />
          <h3>{p.name}</h3>
          <p>Çmimi: {p.price}€</p>
          <button onClick={() => addToCart(p)}>Shto në Shportë</button>
        </div>
      ))}
    </div>
  );
};

export default ProductList;
