// src/App.jsx
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import Register from "./pages/Register";


// Sidor
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import CreateJobApplication from "./pages/CreateJobApplication";

// Skyddad route
import ProtectedRoute from "./components/ProtectedRoute";

// Toastify för notifieringar
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

export default function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>

          {/* Default redirect till /Registera */}
          <Route path="/register" element={<Register />} />

          {/* Default redirect till /login */}
          <Route path="/" element={<Navigate to="/login" replace />} />

          {/*  Publika routes */}
          <Route path="/login" element={<Login />} />

          {/* 🧭 Skyddade routes */}
          <Route
            path="/dashboard"
            element={
              <ProtectedRoute>
                <Dashboard />
              </ProtectedRoute>
            }
          />

          <Route
            path="/create"
            element={
              <ProtectedRoute>
                <CreateJobApplication />
              </ProtectedRoute>
            }
          />

          {/*  404 fallback */}
          <Route path="*" element={<h1 className="text-center mt-20">404 - Sidan finns inte</h1>} />
        </Routes>

        {/* Toastify-container (global notifieringsyta) */}
        <ToastContainer position="top-right" autoClose={3500} newestOnTop />
      </Router>
    </AuthProvider>
  );
}
