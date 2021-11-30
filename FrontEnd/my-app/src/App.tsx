import React from 'react';
import './App.css';
import {
  BrowserRouter,
  Routes,
  Route,
  Navigate,
  Link
} from "react-router-dom";
import Dashboard from './Pages/Dashboard';
import Recommendations from './Pages/Home';


function App() {
  return (
    <BrowserRouter>
    <div>
      <h1>Header</h1>
      
      <nav>
        <ul>
          <li>
            <Link to="/">Recommendations(HOME)</Link>
          </li>
          <li>
            <Link to="/dashboard">Dashboard</Link>
          </li>
        </ul>
      </nav>

      <Routes>
          <Route path="/" element={<Recommendations/>}></Route>
          <Route path="dashboard" element={<Dashboard/>}></Route>
          <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    
      <h1>Footer</h1>
    </div>
    </BrowserRouter>
  );
}

export default App;
