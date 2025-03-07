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
import {
  MapPin,
  Clock,
  DollarSign,
  Phone,
  Globe,
  Star,
  Navigation,
} from "lucide-react";
import { Badge } from "@/components/ui/badge";

// PlaceCard component that can be reused across the application
export default function PlaceCard({
  place, // Place details (name, address, priceLevel, etc.)
  rating, // Rating details (score, date)
  showRating = true, // Whether to show rating
  pUrl = null,
  onCardClick = null, // Optional click handler for the entire card
  className = "", // Additional class names
}) {
  // Safely extract data with defaults to avoid errors
  const name = place?.displayName?.text || place?.name || "Unknown Place";
  const address =
    place?.formattedAddress || place?.address || "Unknown Address";
  const priceLevel = place?.priceLevel || 0;
  const isOpen = place?.currentOpeningHours?.openNow;
  const openingHours = place?.currentOpeningHours?.weekdayDescriptions || [];
  const types = place?.types || [];
  const photos = place?.photos || [];
  const photoUrl = pUrl;
  const score = rating?.score || 0;
  const date = rating?.createdAt ? new Date(rating.createdAt) : null;

  // Function to display price level as dollar signs
  const renderPriceLevel = (level) => {
    return Array(4)
      .fill(0)
      .map((_, i) => (
        <DollarSign
          key={i}
          size={16}
          className={
            i < level ? "fill-green-500 text-green-500" : "text-gray-300"
          }
        />
      ));
  };

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
              {name}
            </CardTitle>
            <CardDescription className="flex items-center mt-1">
              <MapPin size={14} className="mr-1 inline" />
              <span className="line-clamp-1">{address}</span>
            </CardDescription>
          </div>
          {isOpen !== undefined && (
            <Badge variant={isOpen ? "success" : "secondary"}>
              {isOpen ? "Open Now" : "Closed"}
            </Badge>
          )}
        </div>
      </CardHeader>

      <CardContent className="pb-2 flex-grow">
        {photoUrl && (
          <div className="flex justify-center mb-3">
            <img
              src={photoUrl}
              alt={name}
              className="h-40 w-full object-cover rounded-md shadow-sm"
            />
          </div>
        )}

        <div className="space-y-3">
          {types.length > 0 && (
            <div className="flex flex-wrap gap-1">
              {types.slice(0, 3).map((type, index) => (
                <Badge key={index} variant="outline" className="capitalize">
                  {type.replace(/_/g, " ")}
                </Badge>
              ))}
            </div>
          )}

          {priceLevel > 0 && (
            <div className="flex items-center">
              <span className="text-sm text-gray-500 mr-2">Price:</span>
              <div className="flex">{renderPriceLevel(priceLevel)}</div>
            </div>
          )}

          {openingHours.length > 0 && (
            <ScrollArea className="h-24 rounded-md border-0 mt-2">
              <div className="space-y-1">
                <div className="flex items-center text-gray-500 mb-1">
                  <Clock size={14} className="mr-1" />
                  <span className="text-sm font-medium">Hours:</span>
                </div>
                {openingHours.map((hours, index) => (
                  <p
                    key={index}
                    className="text-xs text-gray-600 dark:text-gray-300"
                  >
                    {hours}
                  </p>
                ))}
              </div>
            </ScrollArea>
          )}
        </div>
      </CardContent>

      {(showRating || date) && (
        <CardFooter className="pt-4 border-t">
          <div className="flex justify-between items-center w-full">
            {showRating && (
              <div className="flex">{renderRatingStars(score)}</div>
            )}
            {place?.url && (
              <a
                href={place.url}
                target="_blank"
                rel="noopener noreferrer"
                className="text-sm text-blue-500 flex items-center hover:underline"
                onClick={(e) => e.stopPropagation()}
              >
                <Navigation size={14} className="mr-1" />
                Directions
              </a>
            )}
            {date && (
              <span className="text-xs text-gray-500">
                Visited: {date.toLocaleDateString()}
              </span>
            )}
          </div>
        </CardFooter>
      )}
    </Card>
  );
}
