import React, { useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { createJobApplication } from "../services/JobApplicationService";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

const CreateJobApplication = () => {
  const { token } = useContext(AuthContext);
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    position: "",
    company: "",
    status: "Applied",
    notes: "",
    jobLink: "",
  });
  const [isSubmitted, setIsSubmitted] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await createJobApplication(formData, token);
      setIsSubmitted(true);
      toast.success("Ansökan skapades!");
      setTimeout(() => navigate("/dashboard"), 2000);
    } catch (err) {
      console.error(err);
      toast.error("Kunde inte skapa ansökan.");
    }
  };

  if (isSubmitted) {
    return (
      <div className="text-center mt-20">
        <h2 className="text-2xl font-bold text-green-600 mb-4">
          Ansökan skapades!
        </h2>
        <p className="text-gray-600 mb-6">
          Du skickas automatiskt tillbaka till dashboard...
        </p>
        <button
          onClick={() => navigate("/dashboard")}
          className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2 rounded"
        >
          Tillbaka till Dashboard nu
        </button>
      </div>
    );
  }

  return (
    <form
      onSubmit={handleSubmit}
      className="bg-white p-6 rounded-lg shadow-md space-y-4 max-w-lg mx-auto mt-10"
    >
      <h2 className="text-xl font-bold mb-2">Ny Jobbansökan</h2>

      <input
        type="text"
        placeholder="Jobbtitel"
        value={formData.position}
        onChange={(e) => setFormData({ ...formData, position: e.target.value })}
        className="border p-2 w-full rounded"
        required
      />

      <input
        type="text"
        placeholder="Företag"
        value={formData.company}
        onChange={(e) => setFormData({ ...formData, company: e.target.value })}
        className="border p-2 w-full rounded"
        required
      />

      <select
        value={formData.status}
        onChange={(e) => setFormData({ ...formData, status: e.target.value })}
        className="border p-2 w-full rounded"
      >
        <option value="Applied">Applied</option>
        <option value="Interview">Interview</option>
        <option value="Offer">Offer</option>
        <option value="Rejected">Rejected</option>
      </select>

      <textarea
        placeholder="Anteckningar"
        value={formData.notes}
        onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
        className="border p-2 w-full rounded"
      />

      <input
        type="text"
        placeholder="Länk till jobbannons"
        value={formData.jobLink}
        onChange={(e) => setFormData({ ...formData, jobLink: e.target.value })}
        className="border p-2 w-full rounded"
      />

      <div className="flex justify-between mt-6">
        <button
          type="button"
          onClick={() => navigate("/dashboard")}
          className="bg-gray-500 hover:bg-gray-600 text-white px-4 py-2 rounded"
        >
          ← Tillbaka
        </button>

        <button
          type="submit"
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          Spara ansökan
        </button>
      </div>
    </form>
  );
};

export default CreateJobApplication;
