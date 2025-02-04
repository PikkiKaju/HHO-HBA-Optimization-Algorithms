import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import App from './App';
import TestPage from './TestPage';
import StartTestPage from './StartTestPage';
import CreateReportPage from './CreateReportPage';
import TestFunction from './TestFunction'
import AddFile from './AddFile'

import './index.css';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <Router>
      <Routes>
        <Route path="/" element={<App />} />
        <Route path="/test" element={<TestPage />} />
        <Route path="/start-test" element={<StartTestPage />} />
        <Route path="/create-report" element={<CreateReportPage />} />   
        <Route path="/test-function" element={<TestFunction />} />     
        <Route path="/add-file" element={<AddFile />} />             
      </Routes>
    </Router>
  </React.StrictMode>
);