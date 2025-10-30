import React, { useEffect, useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";
import { getDashboardData } from "../services/DashboardService";
import { toast } from "react-toastify";


export default function Dashboard() {
  const { token, logout } = useContext(AuthContext);
  const navigate = useNavigate();
  const [data, setData] = useState(null);
  

  useEffect(() => {
    if (!token) {
      navigate("/login");
      return;
    }

    const fetchData = async () => {
      try {
        const result = await getDashboardData(token);
        setData(result);
      } catch (err) {
        console.error(err);
        toast.error("Kunde inte hämta dashboard-data ❌");
      }
    };
    fetchData();
  }, [token, navigate]);

  if (!data) return <div className="p-6 text-center">Laddar dashboard...</div>;

  return (
    <div className="min-h-screen bg-gray-700 p-6">
      {/*  Header */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-black">Dashboard</h1>
        <div className="space-x-2">
          <button
            onClick={() => navigate("/create")}
            className="bg-blue-600 hover:bg-blue-700 text-black px-4 py-2 rounded"
          >
            +Ny ansökan
          </button>
          <button
            onClick={logout}
            className="bg-red-500 hover:bg-red-600 text-black px-4 py-2 rounded"
          >
            Logga ut
          </button>
        </div>
      </div>

      {/* alla Statistik UI under här*/}

      {/* Totalt */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 mb-6">
        <div className="bg-black p-4 rounded-lg shadow text-center">
          <h2 className="font-semibold text-purple-500">Totalt</h2>
          <p className="text-3xl font-bold text-pink-500">{data.totalApplications}</p>
        </div>
        {/* Godkända */}
        <div className="bg-black p-4 rounded-lg shadow text-center">
          <h2 className="font-semibold text-gray-600">Godkända</h2>
          <p className="text-3xl font-bold text-green-600">{data.accepted}</p>
        </div>
        {/* Väntande */}
        <div className="bg-black p-4 rounded-lg shadow text-center">
          <h2 className="font-semibold text-purple-400">Väntande</h2>
          <p className="text-3xl font-bold text-yellow-600">{data.pending}</p>
        </div>
        {/* avslag */}
        <div className="bg-black p-4 rounded-lg shadow text-center">
          <h2 className="font-semibold text-purple-600">Avslagna</h2>
          <p className="text-3xl font-bold text-red-600">{data.rejected}</p>
        </div>
      </div>

      {/* Lista över senaste ansökningar */}
      <div className="bg-red-200 p-6 rounded-lg shadow">
        <h2 className="text-xl font-semibold mb-4">Senaste ansökningar</h2>
        <table className="min-w-full text-left border-collapse">
          <thead>
            <tr className="border-b">
              <th className="p-3">Företag</th>
              <th className="p-3">Position</th>
              <th className="p-3">Status</th>
              <th className="p-3">Datum</th>
            </tr>
          </thead>
          <tbody>
            {data.recentApplications.length > 0 ? (
              data.recentApplications.map((app) => (
                <tr key={app.id} className="border-b hover:bg-gray-50">
                  <td className="p-3">{app.company}</td>
                  <td className="p-3">{app.position}</td>
                  <td className="p-3">{app.status}</td>
                  <td className="p-3">
                    {new Date(app.appliedAt).toLocaleDateString("sv-SE")}
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="4" className="text-center text-gray-500 py-4">
                  Inga ansökningar än.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
