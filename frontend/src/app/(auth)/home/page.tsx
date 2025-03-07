"use client";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import api from "@/utils/api";
import { useEffect, useState } from "react";

interface Item {
  id: number;
  name: string;
  category: string;
  description: string;
}
export default function Home() {
  const [items, setItems] = useState<Item[]>([]);
  useEffect(() => {
    const getAllItems = async () => {
      const res = await api.get("/items/allitems");
      setItems(res);
    };
    getAllItems();
  }, []);

  let int = 0;
  return (
    <div>
      <span>Item List</span>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6 p-6">
        {items.length > 0 ? (
          items.map((item) => (
            <Card
              key={item.id}
              className="shadow-lg border border-border rounded-lg transition-transform hover:scale-105"
            >
              <CardHeader>
                <CardTitle className="text-lg font-semibold">
                  {item.name}
                </CardTitle>
                <CardDescription>{item.category}</CardDescription>
              </CardHeader>
              <CardContent className="text-sm text-gray-600">
                {item.description || "No description available"}
              </CardContent>
              <CardFooter className="text-right text-xs text-gray-500">
                Footer
              </CardFooter>
            </Card>
          ))
        ) : (
          <p className="col-span-full text-center text-gray-500">
            No items found.
          </p>
        )}
      </div>
    </div>
  );
}
