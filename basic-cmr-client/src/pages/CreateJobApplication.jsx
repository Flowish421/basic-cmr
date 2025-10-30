import React, { useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { createJobApplication } from "../services/JobApplicationService";

const CreateJobApplication = () => {
  const { token } = useContext(AuthContext);
  const [formData, setFormData] = useState({
    position: "",
    company: "",
    status: "Applied",
    notes: "",
    jobLink: ""
  });

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await createJobApplication(formData, token);
      setFormData({ position: "", company: "", status: "Applied", notes: "", jobLink: "" });
    } catch (err) {
      console.error(err);
    }
  };

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
      />

      <input
        type="text"
        placeholder="Företag"
        value={formData.company}
        onChange={(e) => setFormData({ ...formData, company: e.target.value })}
        className="border p-2 w-full rounded"
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
        placeholder="Länk till jobbannons (valfritt)"
        value={formData.jobLink}
        onChange={(e) => setFormData({ ...formData, jobLink: e.target.value })}
        className="border p-2 w-full rounded"
      />

      <button
        type="submit"
        className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
      >
        Spara ansökan
      </button>
    </form>
  );
};

export default CreateJobApplication;
