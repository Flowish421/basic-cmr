import axios from "axios";
import { toast } from "react-toastify";

// Getalljobb här hämtar vi alla jobb
import api from "./api";

export const getAllJobApplications = async (token) => {
  const res = await api.get("/JobApplications", {
    headers: { Authorization: `Bearer ${token}` },
  });
  return res.data;
};

export const deleteJobApplication = async (id, token) => {
  await api.delete(`/JobApplications/${id}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
};


const API_URL = "http://localhost:5203/api/jobapplications";

export const createJobApplication = async (data, token) => {
  try {
    const response = await axios.post(API_URL, data, {
      headers: { Authorization: `Bearer ${token}` },
    });
    toast.success("Ansökan sparad ✅");
    return response.data;
  } catch (error) {
    console.error("Fel vid skapande av ansökan:", error);
    toast.error("Kunde inte spara ansökan ❌");
    throw error;
  }
};
