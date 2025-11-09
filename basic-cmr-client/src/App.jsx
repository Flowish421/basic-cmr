// src/App.jsx
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";

// Sidor
import Register from "./pages/Register";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import CreateJobApplication from "./pages/CreateJobApplication";
import EditJobApplication from "./pages/EditJobApplication";
import JobApplicationsPage from "./pages/JobApplicationsPage"; 

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

          {/* Publika routes */}
          <Route path="/register" element={<Register />} />
          <Route path="/login" element={<Login />} />

          {/* Default redirect till login */}
          <Route path="/" element={<Navigate to="/login" replace />} />

          {/* Skyddade routes */}
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

          <Route
            path="/edit/:id"
            element={
              <ProtectedRoute>
                <EditJobApplication />
              </ProtectedRoute>
            }
          />

          <Route
            path="/applications"
            element={
              <ProtectedRoute>
                <JobApplicationsPage />
              </ProtectedRoute>
            }
          />

          {/* 404 fallback */}
          <Route
            path="*"
            element={<h1 className="text-center mt-20 text-2xl font-bold">404 - Sidan finns inte</h1>}
          />
        </Routes>

        {/* Toastify-container (global notifieringsyta) */}
        <ToastContainer position="top-right" autoClose={3500} newestOnTop />
      </Router>
    </AuthProvider>
  );
}
