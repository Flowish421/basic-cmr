import React, { useState, useContext } from "react";
import { useNavigate, Link } from "react-router-dom";
import axios from "axios";
import { toast } from "react-toastify";
import { AuthContext } from "../context/AuthContext";

export default function Login() {
  const { login } = useContext(AuthContext);
  const navigate = useNavigate();
  const [formData, setFormData] = useState({ email: "", password: "" });
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      const res = await axios.post("http://localhost:5203/api/auth/login", formData);
      login(res.data.token);
      toast.success("Inloggning lyckades ✅");
      navigate("/dashboard");
    } catch (err) {
      toast.error("Fel e-post eller lösenord ❌");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-500 to-indigo-700">
      <div className="bg-white shadow-xl rounded-lg p-8 w-full max-w-md">
        <h1 className="text-3xl font-bold text-center text-gray-800 mb-6">
          Logga in
        </h1>

        <form onSubmit={handleSubmit} className="space-y-4">
          <input
            type="email"
            placeholder="E-post"
            value={formData.email}
            onChange={(e) => setFormData({ ...formData, email: e.target.value })}
            required
            className="w-full p-3 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500"
          />
          <input
            type="password"
            placeholder="Lösenord"
            value={formData.password}
            onChange={(e) =>
              setFormData({ ...formData, password: e.target.value })
            }
            required
            className="w-full p-3 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500"
          />
          <button
            type="submit"
            disabled={loading}
            className={`w-full bg-blue-600 hover:bg-blue-700 text-white py-3 rounded font-medium transition ${
              loading ? "opacity-70 cursor-not-allowed" : ""
            }`}
          >
            {loading ? "Loggar in..." : "Logga in"}
          </button>
        </form>

        <p className="text-sm text-gray-600 text-center mt-4">
          Har du inget konto?{" "}
          <Link
            to="/register"
            className="text-blue-600 hover:text-blue-800 font-medium"
          >
            Registrera dig här
          </Link>
        </p>
      </div>
    </div>
  );
}
