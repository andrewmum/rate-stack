"use client";

import { Skeleton } from "@/components/ui/skeleton";
import api from "@/utils/api";
import { Book } from "lucide-react";
import { useEffect, useState } from "react";
import BookCard from "@/components/BookCard";
import PlaceCard from "@/components/PlaceCard";

export default function MyRatings() {
  const [ratedItems, setRatedItems] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const getMyRatings = async () => {
      try {
        const res = await api.get(`ratings/my-ratings`);
        setRatedItems(res);
      } catch (error) {
        console.error("Error fetching ratings:", error);
      } finally {
        setLoading(false);
      }
    };
    getMyRatings();
  }, []);

  // Skeleton loader card
  const SkeletonCard = () => (
    <div className="overflow-hidden shadow-md rounded-md">
      <div className="p-4 pb-2">
        <Skeleton className="h-6 w-3/4 mb-2" />
        <Skeleton className="h-4 w-1/2" />
      </div>
      <div className="p-4 pb-2">
        <Skeleton className="h-[180px] w-full rounded-md" />
      </div>
      <div className="p-4 pt-6 border-t">
        <Skeleton className="h-5 w-24" />
      </div>
    </div>
  );

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="flex items-center justify-between mb-8">
          <h1 className="text-3xl font-bold">My Ratings</h1>
        </div>
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
          {[1, 2, 3, 4].map((i) => (
            <SkeletonCard key={i} />
          ))}
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex items-center justify-between mb-8">
        <h1 className="text-3xl font-bold text-gray-800 dark:text-gray-100">
          My Rating Journey
        </h1>
        <div className="text-sm text-gray-500 dark:text-gray-400">
          {ratedItems.length} {ratedItems.length === 1 ? "book" : "books"} rated
        </div>
      </div>

      {ratedItems.length === 0 ? (
        <div className="flex flex-col items-center justify-center py-16 text-center">
          <Book size={64} className="text-gray-400 mb-4" />
          <h2 className="text-xl font-semibold mb-2">No ratings yet</h2>
          <p className="text-gray-500 max-w-md">
            You haven't rated any books yet. Start exploring and rating books to
            see them here!
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
          {ratedItems.map((item) =>
            item.category == "Book" ? (
              <BookCard
                key={item?.ratingId}
                book={item?.item}
                rating={{
                  score: item.score,
                  date: item?.date || new Date(),
                }}
              />
            ) : item.category == "Resturaunt" ? (
              <PlaceCard
                key={item?.ratingId}
                place={item?.item}
                pUrl={item?.item.thumbnail}
                rating={item}
                showRating={true}
              />
            ) : (
              <></>
            )
          )}
        </div>
      )}
    </div>
  );
}
