import { apiRequest } from "./apiRequest";
import type { Task, TaskListResponse } from "./types";


export const TaskApi = {
  getAll: (q?: string, sort?: "dueDate:asc" | "dueDate:desc") =>
    apiRequest<TaskListResponse>({
      url: "/tasks",
      method: "GET",
      params: { q: q ?? "", sort },
    }),

  getById: (id: number) =>
    apiRequest<Task>({
      url: `/tasks/${id}`,
      method: "GET",
    }),

  create: (task: Omit<Task, "id" | "createdAt">) =>
    apiRequest<Task>({
      url: "/tasks",
      method: "POST",
      data: task,
    }),

  update: (id: number, task: Partial<Omit<Task, "createdAt">>) =>
    apiRequest<Task>({
      url: `/tasks/${id}`,
      method: "PUT",
      data: task,
    }),

  delete: (id: number) =>
    apiRequest<void>({
      url: `/tasks/${id}`,
      method: "DELETE",
    }),
};
