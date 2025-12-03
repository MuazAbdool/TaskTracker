import { useEffect, useState } from 'react'
import { TaskApi } from './api/tasks'
import type { ProblemDetails, Task } from './api/types'
import './App.css'
function App() {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    TaskApi.getAll()
      .then(setTasks)
      .catch((err: ProblemDetails) => setError(err.detail ?? "Unknown error"));
  }, []);

  if (error) return <div>Error: {error}</div>;
  return (
    <ul>
      {tasks.map((t) => (
        <li key={t.id}>{t.title}</li>
      ))}
    </ul>
  );
  
}

export default App
