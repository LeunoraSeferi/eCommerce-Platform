import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";

const Orders = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem("token");

    const fetchOrders = async () => {
      try {
        if (!token) throw new Error("Token mungon");

        let clientId = localStorage.getItem("clientId");

        // Marrim clientId nga API nëse nuk ekziston në localStorage
        if (!clientId) {
          const res = await fetch("http://localhost:5000/api/authentication/me", {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });

          if (!res.ok) throw new Error("Nuk u mor ID e klientit");
          const data = await res.json();
          clientId = data.id;
          localStorage.setItem("clientId", clientId);
        }

        const response = await fetch(
          `http://localhost:5002/api/Orders/client/${clientId}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );

        if (!response.ok) throw new Error("Nuk u morën porositë");
        const data = await response.json();
        setOrders(data);
      } catch (err) {
        console.error("Gabim:", err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  return (
    <div className="max-w-4xl mx-auto p-6">
      <h2 className="text-2xl font-bold mb-6">Porositë e Mia</h2>

      {loading ? (
        <p className="text-gray-600">Duke u ngarkuar...</p>
      ) : orders.length === 0 ? (
        <p className="text-gray-600">Ende nuk ka porosi.</p>
      ) : (
        <ul className="space-y-6">
          {orders.map((order, index) => (
            <li key={index} className="bg-white p-6 rounded-lg shadow">
              <h3 className="text-lg font-semibold mb-2">
                Porosia #{index + 1}
              </h3>
              <p className="font-medium mb-1">Produktet:</p>
              <ul className="list-disc list-inside text-sm text-gray-700 mb-4">
                {order.products?.map((p, idx) => (
                  <li key={idx}>
                    {p.name} - {p.price}€
                  </li>
                ))}
              </ul>

              <Link to={`/orders/${order.id}`}>
                <button className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition">
                  Shiko Detajet
                </button>
              </Link>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default Orders;
