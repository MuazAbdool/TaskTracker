import { useEffect, useState } from "react";
import { TaskApi } from "../api/tasks";
import { useNavigate } from "react-router-dom";
import type { Task } from "../api/types";

export  function TaskListPage() {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [searchQuery, setSearchQuery] = useState("");
  const [sortAsc, setSortAsc] = useState(true);

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();

  const fetchTasks = async () => {
    try {
      setLoading(true);
      setError(null);

      const response = await TaskApi.getAll(
        searchQuery || undefined,
        sortAsc ? "dueDate:asc" : "dueDate:desc"
      );

      setTasks(response.tasks); // backend returns array
    } catch (err: any) {
      setError("Failed to load tasks");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTasks();
  }, [searchQuery, sortAsc]);

    const handleDelete = async (id: number) => {
      try {
        setLoading(true);
        setError(null);
        await TaskApi.delete(id);
        fetchTasks();
      } catch (err: any) {
        setError("Failed to delete task");
      } finally {
        setLoading(false);
      }
    };
return (
  <div className="space-y-6">
    <h1 className="text-3xl font-bold text-gray-800">Tasks</h1>

    {/* Search + Sort + Create */}
    <div className="flex flex-wrap gap-3 items-center">
      <input
        className="border border-gray-300 p-2 rounded-lg w-64 focus:ring-2 focus:ring-blue-500 focus:outline-none"
        placeholder="Search tasks..."
        value={searchQuery}
        onChange={(e) => setSearchQuery(e.target.value)}
      />

      <select
        className="border border-gray-300 p-2 rounded-lg focus:ring-2 focus:ring-blue-500"
        value={sortAsc ? "asc" : "desc"}
        onChange={(e) => setSortAsc(e.target.value === "asc")}
      >
        <option value="asc">Due Date (Ascending)</option>
        <option value="desc">Due Date (Descending)</option>
      </select>

      <button
        className="bg-blue-600 text-white px-4 py-2 rounded-lg shadow hover:bg-blue-700 transition"
        onClick={() => navigate("/tasks/new")}
      >
        + Create Task
      </button>
    </div>

    {loading && <p className="text-gray-600">Loading...</p>}
    {error && <p className="text-red-500">{error}</p>}

    {/* Empty state */}
    {!loading && tasks.length === 0 && (
      <div className="text-gray-500 text-lg py-10 text-center">
        No tasks found.
      </div>
    )}

    {/* Task List */}
    <ul className="space-y-4">
      {tasks.map((task) => (
        <li
          key={task.id}
          className="p-5 border bg-white rounded-xl shadow-sm hover:shadow-md transition flex justify-between items-center"
        >
          <div>
            <p className="text-lg font-semibold">{task.title}</p>

            {task.description && (
              <p className="text-sm text-gray-600 mt-1 line-clamp-2">
                {task.description}
              </p>
            )}

            {task.dueDate && (
              <p className="text-sm text-gray-500 mt-2">
                <span className="font-medium text-gray-700">Due:</span>{" "}
                {task.dueDate.split("T")[0]}
              </p>
            )}
          </div>

          <div className="flex gap-2">
            <button
              className="bg-yellow-500 text-white px-3 py-1 rounded-lg hover:bg-yellow-600 transition"
              onClick={() => navigate(`/tasks/${task.id}/edit`)}
            >
              Edit
            </button>

            <button
              className="bg-red-600 text-white px-3 py-1 rounded-lg hover:bg-red-700 transition"
              onClick={() => handleDelete(task.id)}
            >
              Delete
            </button>
          </div>
        </li>
      ))}
    </ul>
  </div>
);

//   return (
//     <div className="p-6 space-y-6">
//       <h1 className="text-2xl font-bold">Tasks</h1>
//       <div className="flex gap-4">
//         <input
//           className="border p-2 rounded w-64"
//           placeholder="Search..."
//           value={searchQuery}
//           onChange={(e) => setSearchQuery(e.target.value)}
//         />

//         <select
//           className="border p-2 rounded"
//           value={sortAsc ? "asc" : "desc"}
//           onChange={(e) => setSortAsc(e.target.value === "asc")}
//         >
//           <option value="asc">Due Date Asc</option>
//           <option value="desc">Due Date Desc</option>
//         </select>

//         <button
//           className="bg-blue-600 text-white px-4 py-2 rounded"
//           onClick={() => navigate("/tasks/new")}
//         >
//           + Create Task
//         </button>
//       </div>

//       {loading && <p>Loading...</p>}
//       {error && <p className="text-red-500">{error}</p>}

//       {/* <ul className="border rounded divide-y">
//         {tasks.map((task) => (
//           <li
//             key={task.id}
//             className="p-4 hover:bg-gray-100 cursor-pointer"
//             onClick={() => navigate(`/tasks/${task.id}/edit`)}
//           >
//             <div className="font-semibold">{task.title}</div>
//             <div className="text-sm text-gray-600">{task.description}</div>
//           </li>
//         ))}
//       </ul> */}
//  {/* Task list */}
//       <ul className="space-y-4">
//         {tasks.map((task) => (
//           <li
//             key={task.id}
//             className="border p-4 rounded flex justify-between items-center"
//           >
//             <div>
//               <p className="font-semibold">{task.title}</p>
//               {task.dueDate && (
//                 <p className="text-sm text-gray-600">
//                   Due: {task.dueDate.split("T")[0]}
//                 </p>
//               )}
//             </div>

//             <div className="flex gap-2">
//               <button
//                 className="bg-yellow-500 text-white px-3 py-1 rounded"
//                 onClick={() => navigate(`/tasks/${task.id}/edit`)}
//               >
//                 Edit
//               </button>

//               <button
//                 className="bg-red-600 text-white px-3 py-1 rounded"
//                 onClick={() => handleDelete(task.id)}
//               >
//                 Delete
//               </button>
//             </div>
//           </li>
//         ))}
//       </ul>
      
//     </div>
//   );
}
