import "./App.css";
import { Routes, Route, Navigate } from "react-router-dom";
import Dashboard from "./Pages/Dashboard";
import Recommendations from "./Pages/Home";
import Layout from "./Components/layout";
import SearchAutoComplete from "./Components/search-bar";
import MakeARecommendation from "./Components/make-recommendation";
import MakeARequest from "./Pages/Home/Components/make-request";
import PostFilter from "./Pages/Dashboard/Components/post-filter";

function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<Recommendations />} />
        <Route path="dashboard" element={<Dashboard />} />
        <Route path="*" element={<Navigate to="/" />} />
      </Route>
    </Routes>
  );
}

export default App;
