// src/components/Cart.jsx

import React, { useEffect, useState } from "react";
import * as jwt_decode from "jwt-decode"; // ✅

import "./Cart.css";

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

const getClientIdFromToken = () => {
  const token = localStorage.getItem("token");
  if (!token) return null;
  try {
    const decoded = jwt_decode.jwtDecode(token); 
    return decoded?.nameid || decoded?.clientId || decoded?.ClientId || null;
  } catch (error) {
    console.error("Gabim në dekodimin e token-it:", error);
    return null;
  }
};

function Cart() {
  const [cartItems, setCartItems] = useState([]);

  useEffect(() => {
    const items = JSON.parse(localStorage.getItem("cartItems")) || [];
    setCartItems(items);
  }, []);

  const handleRemove = (index) => {
    const updated = [...cartItems];
    updated.splice(index, 1);
    setCartItems(updated);
    localStorage.setItem("cartItems", JSON.stringify(updated));
  };

  const total = cartItems.reduce((sum, item) => sum + item.price, 0);

  const sendOrder = async () => {
    const token = localStorage.getItem("token");
    const clientId = getClientIdFromToken();

    if (!token) {
      alert("Ju lutem kyçuni para porosisë!");
      return;
    }

    if (!clientId) {
      alert("ID e klientit nuk u gjet në token!");
      return;
    }

    if (cartItems.length === 0) {
      alert("Shporta është bosh!");
      return;
    }

    try {
      for (const item of cartItems) {
        const payload = {
          productId: item.id,
          clientId: parseInt(clientId),
          purchaseQuantity: 1,
          orderedDate: new Date().toISOString()
        };

        const response = await fetch("http://localhost:5002/api/Orders", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`
          },
          body: JSON.stringify(payload)
        });

        if (!response.ok) {
          const message = await response.text();
          throw new Error(`Gabim për ${item.name}: ${message}`);
        }
      }

      alert("Të gjitha porositë u dërguan me sukses!");
      localStorage.removeItem("cartItems");
      setCartItems([]);
    } catch (error) {
      console.error("Gabim gjatë dërgimit:", error.message);
      alert("Disa porosi dështuan. Kontrollo konzolën për detaje.");
    }
  };

  return (
    <div className="cart-container">
      <h2 className="cart-title">Shporta</h2>

      {cartItems.length === 0 ? (
        <p>Shporta është bosh.</p>
      ) : (
        <>
          <div className="cart-grid">
            {cartItems.map((item, index) => (
              <div className="cart-card" key={index}>
                <img
                  src={item.imageUrl || getImage(item.name)}
                  alt={item.name}
                  className="cart-image"
                />
                <div className="cart-details">
                  <h3>{item.name}</h3>
                  <p>{item.price}€</p>
                  <button
                    className="remove-button"
                    onClick={() => handleRemove(index)}
                  >
                    Fshi
                  </button>
                </div>
              </div>
            ))}
          </div>

          <div className="cart-summary">
            <h3>Total: {total.toFixed(2)}€</h3>
            <button className="checkout-button" onClick={sendOrder}>
              Dërgo Porosi
            </button>
          </div>
        </>
      )}
    </div>
  );
}

export default Cart;
