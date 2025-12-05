import { BrowserRouter, Routes, Route } from "react-router-dom";
import { TaskListPage } from "./pages/TaskListPage";
import { TaskFormPage } from "./pages/TaskFormPage";
import { Layout } from "./components/Layout";

export default function App() {
  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          <Route path="/" element={<TaskListPage />} />
          <Route path="/tasks/new" element={<TaskFormPage mode="create" />} />
          <Route path="/tasks/:id/edit" element={<TaskFormPage mode="edit" />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}
