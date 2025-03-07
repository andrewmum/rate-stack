import { useState } from "react";
import { Star } from "lucide-react";

export default function Rating({ maxStars = 5, onChange, stars = 0 }) {
  const [rating, setRating] = useState(0);
  const [hover, setHover] = useState(0);

  return (
    <div className="flex space-x-1">
      {[...Array(maxStars)].map((_, index) => {
        const starValue = index + 1;
        return (
          <Star
            key={starValue}
            className={`w-6 h-6 cursor-pointer transition-colors ${
              (hover || rating) >= starValue
                ? "text-yellow-400"
                : "text-gray-300"
            }`}
            onMouseEnter={() => setHover(starValue)}
            onMouseLeave={() => setHover(0)}
            onClick={() => {
              setRating(starValue);
              if (onChange) onChange(starValue);
            }}
          />
        );
      })}
    </div>
  );
}
