"use client";

import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Calendar, Star, User } from "lucide-react";

export default function BookCard({
  book, // Book details (title, authors, description, thumbnail, etc.)
  rating, // Rating details (score, date)
  showRating = true, // Whether to show rating
  onCardClick = null, // Optional click handler for the entire card
  className = "", // Additional class names
}) {
  const title = book?.title || "Unknown Title";
  const author = book?.authors?.[0] || "Unknown Author";
  const description = book?.description || "No description available.";
  const thumbnail = book?.thumbnail || null;
  const year = book?.publishedDate
    ? new Date(book.publishedDate).getFullYear()
    : null;
  const score = rating?.score || 0;
  const date = rating?.date ? new Date(rating.date) : null;

  // Function to render rating stars
  const renderRatingStars = (score) => {
    return Array(5)
      .fill(0)
      .map((_, i) => (
        <Star
          key={i}
          size={18}
          className={
            i < score ? "fill-yellow-400 text-yellow-400" : "text-gray-300"
          }
        />
      ));
  };

  return (
    <Card
      className={`overflow-hidden shadow-md hover:shadow-lg transition-shadow duration-300 flex flex-col ${className}`}
      onClick={onCardClick}
      role={onCardClick ? "button" : undefined}
      tabIndex={onCardClick ? 0 : undefined}
    >
      <CardHeader className="pb-2">
        <div className="flex justify-between items-start">
          <div>
            <CardTitle className="text-lg line-clamp-1 font-bold">
              {title}
            </CardTitle>
            <CardDescription className="flex items-center mt-1">
              <User size={14} className="mr-1 inline" />
              {author}
            </CardDescription>
          </div>
          {year && (
            <div className="bg-blue-100 dark:bg-blue-900 text-blue-800 dark:text-blue-100 px-2 py-1 rounded-full text-xs">
              {year}
            </div>
          )}
        </div>
      </CardHeader>

      <CardContent className="pb-2 flex-grow">
        {thumbnail && (
          <div className="flex justify-center mb-3">
            <img
              src={thumbnail}
              alt={title}
              className="h-40 object-contain rounded-md shadow-sm"
            />
          </div>
        )}

        <ScrollArea className="h-24 rounded-md border-0">
          <p className="text-sm text-gray-600 dark:text-gray-300 leading-relaxed">
            {description
              ? description.length > 200
                ? `${description.substring(0, 200)}...`
                : description
              : "No description available."}
          </p>
        </ScrollArea>
      </CardContent>

      {(showRating || date) && (
        <CardFooter className="pt-4 border-t">
          <div className="flex justify-between items-center w-full">
            {showRating && (
              <div className="flex">{renderRatingStars(score)}</div>
            )}
            {date && (
              <span className="text-xs text-gray-500 flex items-center">
                <Calendar size={14} className="mr-1" />
                {date.toLocaleDateString(undefined, {
                  month: "short",
                  day: "numeric",
                })}
              </span>
            )}
          </div>
        </CardFooter>
      )}
    </Card>
  );
}
