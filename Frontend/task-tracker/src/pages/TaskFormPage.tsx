import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { TaskApi } from "../api/tasks";
import type { Task } from "../api/types";

export function TaskFormPage({ mode }: { mode: "create" | "edit" }) {
  const navigate = useNavigate();
  const { id } = useParams();

  // form fields
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [status, setStatus] = useState(0);
  const [priority, setPriority] = useState(0);
  const [dueDate, setDueDate] = useState<string | null>(null);

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Load task if editing
  useEffect(() => {
    if (mode === "edit" && id) {
      setLoading(true);
      TaskApi.getById(Number(id))
        .then((task) => {
          setTitle(task.title);
          setDescription(task.description);
          setStatus(task.status);
          setPriority(task.priority);
           const formatted = task.dueDate
          ? task.dueDate.substring(0, 10)
          : null;
          setDueDate(formatted);
        })
        .catch(() => setError("Task not found"))
        .finally(() => setLoading(false));
    }
  }, [mode, id]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      if (mode === "create") {
        await TaskApi.create({
          title,
          description,
          status,
          priority,
          dueDate,
        });
      } else {
        await TaskApi.update(Number(id), {
          id: Number(id),
          title,
          description,
          status,
          priority,
          dueDate,
        });
      }

      navigate("/");
    } catch (err) {
      setError("Failed to save task");
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <p className="p-6">Loading...</p>;

return (
  <form
    onSubmit={handleSubmit}
    className="bg-white shadow-md rounded-xl p-6 space-y-6 max-w-xl mx-auto"
  >
    <h1 className="text-3xl font-bold text-gray-800">
      {mode === "create" ? "Create Task" : "Edit Task"}
    </h1>

    {error && <p className="text-red-500">{error}</p>}

    {/* Title */}
    <div>
      <label className="block font-semibold mb-1 text-gray-700">Title</label>
      <input
        className="border border-gray-300 p-3 rounded-lg w-full focus:ring-2 focus:ring-blue-500 focus:outline-none"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        required
      />
    </div>

    {/* Description */}
    <div>
      <label className="block font-semibold mb-1 text-gray-700">Description</label>
      <textarea
        className="border border-gray-300 p-3 rounded-lg w-full h-28 resize-none focus:ring-2 focus:ring-blue-500 focus:outline-none"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
      />
    </div>

    {/* Status */}
    <div>
      <label className="block font-semibold mb-1 text-gray-700">Status</label>
      <select
        className="border border-gray-300 p-3 rounded-lg w-full focus:ring-2 focus:ring-blue-500 focus:outline-none"
        value={status}
        onChange={(e) => setStatus(Number(e.target.value))}
      >
        <option value={0}>Pending</option>
        <option value={1}>In Progress</option>
        <option value={2}>Completed</option>
      </select>
    </div>

    {/* Priority */}
    <div>
      <label className="block font-semibold mb-1 text-gray-700">Priority</label>
      <select
        className="border border-gray-300 p-3 rounded-lg w-full focus:ring-2 focus:ring-blue-500 focus:outline-none"
        value={priority}
        onChange={(e) => setPriority(Number(e.target.value))}
      >
        <option value={0}>Low</option>
        <option value={1}>Medium</option>
        <option value={2}>High</option>
      </select>
    </div>

    {/* Due Date */}
    <div>
      <label className="block font-semibold mb-1 text-gray-700">Due Date</label>
      <input
        type="date"
        className="border border-gray-300 p-3 rounded-lg w-full focus:ring-2 focus:ring-blue-500 focus:outline-none"
        value={dueDate ?? ""}
        onChange={(e) => setDueDate(e.target.value || null)}
      />
    </div>

    {/* Submit Button */}
    <button
      type="submit"
      className="bg-blue-600 text-white px-6 py-3 rounded-lg w-full shadow hover:bg-blue-700 transition"
    >
      {mode === "create" ? "Create Task" : "Save Changes"}
    </button>
  </form>
);

}
