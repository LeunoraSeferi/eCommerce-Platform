import React, { useEffect, useState } from "react";
import { FaShoppingCart } from "react-icons/fa";
import { Link, useNavigate } from "react-router-dom";
import Login from "../../components/Login/Login";
import "./Header.css";

function Header() {
  const [showLogin, setShowLogin] = useState(false);
  const [cartItems, setCartItems] = useState([]);
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const items = JSON.parse(localStorage.getItem("cartItems")) || [];
    setCartItems(items);

    const token = localStorage.getItem("token");
    setIsLoggedIn(!!token);
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("cartItems"); // opsionale
    setIsLoggedIn(false);
    window.location.reload(); // rifresko UI-në
  };

  return (
    <header className="header">
      <div className="logo">
     <img src="/images/logo.jpg" alt="Logo" className="logo-image" />
    </div>


      

      <div className="user-actions">
        <div className="cart-container">
          <button className="cart-icon" onClick={() => navigate("/cart")}>
            <FaShoppingCart size={20} />
            {cartItems.length > 0 && (
              <span className="cart-count">{cartItems.length}</span>
            )}
          </button>
        </div>

        {isLoggedIn ? (
          <button className="login-button" onClick={handleLogout}>Dil</button>
        ) : (
          <button className="login-button" onClick={() => setShowLogin(true)}>Login</button>
        )}
      </div>

      {showLogin && !isLoggedIn && (
        <div className="login-modal">
          <div className="login-backdrop" onClick={() => setShowLogin(false)} />
          <div className="login-box">
            <Login
              
              onLoginSuccess={() => {
                setIsLoggedIn(true);
                setShowLogin(false);
                navigate("/products"); 
              }}
            />
          </div>
        </div>
      )}
    </header>
  );
}

export default Header;
