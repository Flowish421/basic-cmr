import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5203/api",
});

export default api;
