import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import axios from "axios";
import { toast } from "react-toastify";

export default function Register() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    email: "",
    password: "",
    confirmPassword: "",
  });
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (formData.password !== formData.confirmPassword) {
      toast.error("Lösenorden matchar inte ❌");
      return;
    }

    setLoading(true);
    try {
      await axios.post("http://localhost:5203/api/auth/register", {
        email: formData.email,
        password: formData.password,
      });
      toast.success("Konto skapat! Du kan nu logga in ✅");
      navigate("/login");
    } catch (err) {
      toast.error("Kunde inte registrera konto ❌");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-indigo-600 to-purple-700">
      <div className="bg-white shadow-xl rounded-lg p-8 w-full max-w-md">
        <h1 className="text-3xl font-bold text-center text-gray-800 mb-6">
          Registrera konto
        </h1>

        <form onSubmit={handleSubmit} className="space-y-4">
          <input
            type="email"
            placeholder="E-postadress"
            value={formData.email}
            onChange={(e) =>
              setFormData({ ...formData, email: e.target.value })
            }
            required
            className="w-full p-3 border border-gray-300 rounded focus:ring-2 focus:ring-indigo-500"
          />

          <input
            type="password"
            placeholder="Lösenord"
            value={formData.password}
            onChange={(e) =>
              setFormData({ ...formData, password: e.target.value })
            }
            required
            className="w-full p-3 border border-gray-300 rounded focus:ring-2 focus:ring-indigo-500"
          />

          <input
            type="password"
            placeholder="Bekräfta lösenord"
            value={formData.confirmPassword}
            onChange={(e) =>
              setFormData({ ...formData, confirmPassword: e.target.value })
            }
            required
            className="w-full p-3 border border-gray-300 rounded focus:ring-2 focus:ring-indigo-500"
          />

          <button
            type="submit"
            disabled={loading}
            className={`w-full bg-indigo-600 hover:bg-indigo-700 text-white py-3 rounded font-medium transition ${
              loading ? "opacity-70 cursor-not-allowed" : ""
            }`}
          >
            {loading ? "Registrerar..." : "Skapa konto"}
          </button>
        </form>

        <p className="text-sm text-gray-600 text-center mt-4">
          Har du redan ett konto?{" "}
          <Link
            to="/login"
            className="text-indigo-600 hover:text-indigo-800 font-medium"
          >
            Logga in här
          </Link>
        </p>
      </div>
    </div>
  );
}
