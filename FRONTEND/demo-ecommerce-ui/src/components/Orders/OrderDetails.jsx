// src/components/Orders/OrderDetails.jsx

import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

const OrderDetails = () => {
  const { id } = useParams();
  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem("token");

    fetch(`http://localhost:5002/api/Orders/details/${id}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
      .then((res) => {
        if (!res.ok) throw new Error("Nuk u morën detajet e porosisë");
        return res.json();
      })
      .then((data) => {
        setOrder(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error(err);
        setLoading(false);
      });
  }, [id]);

  if (loading)
    return (
      <div className="text-center mt-10 text-lg text-gray-700">
        Po ngarkohen të dhënat...
      </div>
    );

  if (!order)
    return (
      <div className="text-center mt-10 text-red-600">
        Porosia nuk u gjet.
      </div>
    );

  return (
    <div className="max-w-2xl mx-auto mt-10 p-6 bg-white rounded-lg shadow-md">
      <h2 className="text-2xl font-bold mb-4 text-blue-600">
        Detajet e Porosisë #{order.orderId}
      </h2>

      <div className="space-y-2 text-gray-700">
        <p>
          <span className="font-semibold">Emri i klientit:</span> {order.name}
        </p>
        <p>
          <span className="font-semibold">Email:</span> {order.email}
        </p>
        <p>
          <span className="font-semibold">Telefoni:</span>{" "}
          {order.telephoneNumber}
        </p>
        <p>
          <span className="font-semibold">Adresa:</span> {order.address}
        </p>
      </div>

      <hr className="my-6 border-t-2 border-gray-200" />

      <h3 className="text-xl font-semibold mb-3 text-gray-800">Produkti</h3>
      <div className="space-y-2 text-gray-700">
        <p>
          <span className="font-semibold">Emri:</span> {order.productName}
        </p>
        <p>
          <span className="font-semibold">Sasia:</span> {order.purchaseQuantity}
        </p>
        <p>
          <span className="font-semibold">Çmimi për njësi:</span>{" "}
          {order.unitPrice}€
        </p>
        <p>
          <span className="font-semibold">Totali:</span> {order.totalPrice}€
        </p>
        <p>
          <span className="font-semibold">Data e Porosisë:</span>{" "}
          {new Date(order.orderedDate).toLocaleString()}
        </p>
      </div>
    </div>
  );
};

export default OrderDetails;
