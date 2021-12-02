import './App.css';
import {
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import Dashboard from './Pages/Dashboard';
import Recommendations from './Pages/Home';
import Layout from './Components/layout';


function App() {
  return (
      <Routes>
        <Route path="/" element= {<Layout/>}>
          <Route index element= {<Recommendations/>}/>
          <Route path="dashboard" element={<Dashboard/>}/>
          <Route path="*" element={<Navigate to="/" />}/>
        </Route>  
      </Routes>
  );
}

export default App;
