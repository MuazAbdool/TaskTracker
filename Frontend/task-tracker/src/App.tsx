import { BrowserRouter, Routes, Route } from "react-router-dom";
import{ TaskListPage} from "./pages/TaskListPage";
import {TaskFormPage} from "./pages/TaskFormPage";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<TaskListPage />} />
        <Route path="/tasks/new" element={<TaskFormPage mode="create" />} />
        <Route path="/tasks/:id/edit" element={<TaskFormPage mode="edit" />} />
      </Routes>
    </BrowserRouter>
  );
}
