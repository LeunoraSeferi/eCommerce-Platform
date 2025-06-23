import { createContext, useContext, useState } from "react";

// Krijo context-in për autentikim
const AuthContext = createContext();

// Provider për të mbështjellë aplikacionin
export const AuthProvider = ({ children }) => {
  // Merr token-in nga localStorage nëse ekziston (ruhet pas login)
  const [token, setToken] = useState(localStorage.getItem("token") || null);

  // Funksioni për login – ruan tokenin në localStorage dhe state
  const login = (newToken) => {
    localStorage.setItem("token", newToken);
    setToken(newToken);
  };

  // Funksioni për logout – pastron tokenin nga localStorage dhe state
  const logout = () => {
    localStorage.removeItem("token");
    setToken(null);
  };

  return (
    <AuthContext.Provider value={{ token, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

// Hook për të përdorur context-in
export const useAuth = () => useContext(AuthContext);
