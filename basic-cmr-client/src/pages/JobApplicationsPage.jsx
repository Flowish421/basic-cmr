import React, { useEffect, useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { getAllJobApplications, deleteJobApplication } from "../services/JobApplicationService";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

export default function JobApplicationsPage() {
  const { token, logout } = useContext(AuthContext);
  const [applications, setApplications] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    if (!token) {
      navigate("/login");
      return;
    }

    const fetchData = async () => {
      try {
        const data = await getAllJobApplications(token);
        setApplications(data);
      } catch (err) {
        console.error("Fel vid hämtning:", err);
        toast.error("Kunde inte hämta ansökningar ❌");
      }
    };
    fetchData();
  }, [token, navigate]);

  const handleDelete = async (id) => {
    if (window.confirm("Vill du verkligen ta bort den här ansökan?")) {
      try {
        await deleteJobApplication(id, token);
        setApplications(applications.filter((a) => a.id !== id));
        toast.success("Ansökan raderad ✅");
      } catch (err) {
        toast.error("Kunde inte radera ansökan ❌");
      }
    }
  };

  return (
    <div className="min-h-screen bg-gray-100 p-6">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-800">Mina jobbansökningar</h1>
        <div className="space-x-2">
          <button
            onClick={() => navigate("/create")}
            className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded"
          >
            ➕ Ny ansökan
          </button>
          <button
            onClick={logout}
            className="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded"
          >
            Logga ut
          </button>
        </div>
      </div>

      <div className="bg-white p-6 rounded-lg shadow">
        <table className="min-w-full text-left border-collapse">
          <thead>
            <tr className="border-b">
              <th className="p-3">Företag</th>
              <th className="p-3">Position</th>
              <th className="p-3">Status</th>
              <th className="p-3">Datum</th>
              <th className="p-3 text-right">Åtgärder</th>
            </tr>
          </thead>
          <tbody>
            {applications.length > 0 ? (
              applications.map((app) => (
                <tr key={app.id} className="border-b hover:bg-gray-50">
                  <td className="p-3">{app.company}</td>
                  <td className="p-3">{app.position}</td>
                  <td className="p-3">{app.status}</td>
                  <td className="p-3">
                    {new Date(app.appliedAt).toLocaleDateString("sv-SE")}
                  </td>
                  <td className="p-3 text-right space-x-2">
                    <button
                      onClick={() => navigate(`/edit/${app.id}`)}
                      className="bg-yellow-500 hover:bg-yellow-600 text-white px-3 py-1 rounded"
                    >
                      ✏️ Redigera
                    </button>
                    <button
                      onClick={() => handleDelete(app.id)}
                      className="bg-red-600 hover:bg-red-700 text-white px-3 py-1 rounded"
                    >
                      🗑️ Radera
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="5" className="text-center text-gray-500 py-4">
                  Inga ansökningar ännu.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
