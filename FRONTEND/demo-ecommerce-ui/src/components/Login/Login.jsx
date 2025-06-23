import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import * as jwt_decode from "jwt-decode";

import "./Login.css";

const Login = ({ onLoginSuccess }) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleLogin = async () => {
    try {
      const response = await fetch("http://localhost:5000/api/Authentication/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      const data = await response.json();

      if (response.ok && data.flag && data.message) {
        localStorage.setItem("token", data.message);

        // ✅ Dekodimi i token-it për të nxjerrë clientId ose nameid
        try {
          const decoded = jwt_decode.jwtDecode(data.message);

          const clientId = decoded?.nameid || decoded?.clientId || decoded?.ClientId;
          if (clientId) {
            localStorage.setItem("clientId", clientId);
          }
        } catch (decodeError) {
          console.warn("Nuk u arrit të dekodohet token-i:", decodeError);
        }

        alert("Kyçja me sukses!");
        if (onLoginSuccess) onLoginSuccess();
        navigate("/products");
      } else {
        alert("Email ose fjalëkalim i pasaktë.");
      }
    } catch (error) {
      alert("Gabim gjatë lidhjes me serverin.");
    }
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h2>Kyçu</h2>
        <input
          type="email"
          placeholder="Email-i"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <input
          type="password"
          placeholder="Fjalëkalimi"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <button onClick={handleLogin}>Login</button>
      </div>
    </div>
  );
};

export default Login;
