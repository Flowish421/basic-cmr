import React, { useEffect, useState, useContext } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";
import { toast } from "react-toastify";
import { AuthContext } from "../context/AuthContext";

export default function EditJobApplication() {
  const { id } = useParams();
  const { token } = useContext(AuthContext);
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    company: "",
    position: "",
    status: "",
    notes: "",
    jobLink: "",
  });
  const [loading, setLoading] = useState(true);

  // Hämta befintlig ansökan
  useEffect(() => {
    const fetchJob = async () => {
      try {
        const res = await axios.get(`http://localhost:5203/api/jobapplications/${id}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setFormData(res.data);
      } catch (err) {
        toast.error("Kunde inte hämta ansökan ");
      } finally {
        setLoading(false);
      }
    };
    fetchJob();
  }, [id, token]);

  // Hantera ändringar
  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  // Spara ändringar
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.put(
        `http://localhost:5203/api/jobapplications/${id}`,
        formData,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      toast.success("Ansökan uppdaterad ");
      navigate("/dashboard");
    } catch (err) {
      toast.error("Kunde inte uppdatera ansökan ");
    }
  };

  if (loading) return <div className="text-center p-6">Laddar...</div>;

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 p-6">
      <div className="bg-white rounded-xl shadow-lg p-6 w-full max-w-lg">
        <h1 className="text-2xl font-bold mb-4">✏️ Redigera ansökan</h1>

        <form onSubmit={handleSubmit} className="space-y-4">
          <input
            type="text"
            name="company"
            value={formData.company}
            onChange={handleChange}
            placeholder="Företag"
            className="w-full p-3 border rounded"
          />
          <input
            type="text"
            name="position"
            value={formData.position}
            onChange={handleChange}
            placeholder="Position"
            className="w-full p-3 border rounded"
          />
          <select
            name="status"
            value={formData.status}
            onChange={handleChange}
            className="w-full p-3 border rounded"
          >
            <option value="">Välj status</option>
            <option value="Applied">Applied</option>
            <option value="Interview">Interview</option>
            <option value="Offer">Offer</option>
            <option value="Rejected">Rejected</option>
          </select>
          <input
            type="text"
            name="jobLink"
            value={formData.jobLink || ""}
            onChange={handleChange}
            placeholder="Jobblänk"
            className="w-full p-3 border rounded"
          />
          <textarea
            name="notes"
            value={formData.notes || ""}
            onChange={handleChange}
            placeholder="Anteckningar"
            className="w-full p-3 border rounded"
          />

          <button
            type="submit"
            className="w-full bg-blue-600 hover:bg-blue-700 text-white py-3 rounded font-semibold"
          >
            Spara ändringar
          </button>

          <button
            type="button"
            onClick={() => navigate("/dashboard")}
            className="w-full bg-gray-400 hover:bg-gray-500 text-white py-3 rounded font-semibold"
          >
            Tillbaka
          </button>
        </form>
      </div>
    </div>
  );
}
