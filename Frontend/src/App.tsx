import React from "react";
import { useNavigate } from "react-router-dom";
import "./App.css";
import { useEffect, useState } from "react";

const App: React.FC = () => {
  const navigate = useNavigate();
  const [message, setMessage] = useState("");

  return (
    <div
      className="App"
      style={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh",
      }}
    >
      <h1 style={{ fontWeight: "bold", textAlign: "center" }}>
        Testing Algorithms HBA and HHO
      </h1>
      <table
        border="1"
        style={{
          textAlign: "center",
          padding: "10px",
          borderCollapse: "collapse",
          width: "50%",
        }}
      >
        <tbody>
          <tr>
            <td>
              <button onClick={() => navigate("/test")}>
                <strong>Run a test based on algorithm</strong>
              </button>
            </td>
          </tr>
          <tr>
            <td>
              <button onClick={() => navigate("/test-function")}>
                <strong>Run a test based on function</strong>
              </button>
            </td>
          </tr>
          <tr>
            <td>
              <button onClick={() => navigate("/add-file")}>
                <strong>Add file</strong>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  );
};

export default App;
