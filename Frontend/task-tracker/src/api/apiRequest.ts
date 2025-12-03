import axios from "axios";

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_TASKS_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export function apiRequest<T>(config: {
  url: string;
  method: "GET" | "POST" | "PUT" | "DELETE";
  params?: any;
  data?: any;
}) {
  return apiClient(config).then(res => res.data as T);
}
