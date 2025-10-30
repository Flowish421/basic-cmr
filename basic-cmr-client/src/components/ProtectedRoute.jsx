import React, { useContext } from "react";
import { Navigate } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";

export default function ProtectedRoute({ children }) {
  const { token, isReady } = useContext(AuthContext);

  // Vänta tills AuthContext är färdigladdad
  if (!isReady) return <div>Laddar...</div>;

  // Om ingen token → skicka till login
  return token ? children : <Navigate to="/login" replace />;
}
