import React, { createContext, useContext, useState, useEffect } from "react";
import { jwtDecode } from "jwt-decode";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(localStorage.getItem("token"));
  const [isReady, setIsReady] = useState(false); // 👈 ny flagga

  useEffect(() => {
    if (token) {
      try {
        const decoded = jwtDecode(token);
        setUser({ email: decoded.sub, id: decoded.UserId });
      } catch (err) {
        console.error("Invalid token:", err);
        localStorage.removeItem("token");
        setToken(null);
      }
    }
    setIsReady(true); // ✅ nu vet vi att AuthContext är klar
  }, [token]);

  const login = (newToken) => {
    localStorage.setItem("token", newToken);
    setToken(newToken);
  };

  const logout = () => {
    localStorage.removeItem("token");
    setToken(null);
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, token, login, logout, isReady }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
