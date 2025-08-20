import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:5001/api/", // Change this to your backend URL
});

// Attach token automatically
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;