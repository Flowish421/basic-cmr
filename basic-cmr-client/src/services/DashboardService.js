import axios from "axios";

const API_URL = "http://localhost:5203/api/dashboard";

export const getDashboardData = async (token) => {
  const response = await axios.get(API_URL, {
    headers: { Authorization: `Bearer ${token}` },
  });
  return response.data;
};
