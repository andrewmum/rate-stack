"use client";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";
import api from "@/utils/api";
import { useEffect, useState } from "react";
import axios from "axios";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import Rating from "@/components/Rating";
import { useAuth } from "@/context/AuthContext";
import BookCard from "@/components/BookCard";
import { toast } from "sonner";
import PlaceCard from "@/components/PlaceCard";
import LinkInput from "@/components/LinkInput";

export default function Review() {
  const [name, setName] = useState("");
  const [category, setCategory] = useState("");
  const [description, setDescription] = useState("");
  const [bookSuggestions, setBookSuggestions] = useState([]);
  const [placeSuggestions, setPlaceSuggestions] = useState([]);
  const [query, setQuery] = useState("");
  const [placeQuery, setPlaceQuery] = useState("");

  const [selectedPlace, setSelectedPlace] = useState(null);
  const [selectedBook, setSelectedBook] = useState(null);
  const [open, setOpen] = useState(false);
  const [openPlace, setOpenPlace] = useState(false);
  const [selectedImage, setSelectedImage] = useState(false);
  const [rating, setRating] = useState(0);
  const [placeRating, setPlaceRating] = useState(0);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const submitRating = async () => {
    if (!selectedBook || rating === 0) {
      toast("Rating required", {
        description: "Please select a rating before submitting",
      });
      return;
    }

    setIsSubmitting(true);
    try {
      const bookId = selectedBook.id;
      await api.post("/ratings", {
        score: rating,
        externalItemId: bookId,
        itemId: 0,
        review: "",
        category: 4,
      });

      toast("Success!", {
        description: "Your rating has been submitted",
      });

      setSelectedBook(null);
      setRating(0);
      setQuery("");
    } catch (error) {
      console.error("Error submitting rating:", error);
      toast("Error", {
        description: "There was a problem submitting your rating",
      });
    } finally {
      setIsSubmitting(false);
    }
  };

  const submitPlaceRating = async () => {
    if (!selectedPlace || placeRating === 0) {
      toast("Rating required", {
        description: "Please select a rating before submitting",
      });
      return;
    }

    setIsSubmitting(true);
    try {
      const placeId = selectedPlace.id;
      await api.post("/ratings", {
        score: placeRating,
        externalItemId: placeId,
        itemId: 0,
        review: "",
        category: 0,
      });

      toast("Success!", {
        description: "Your place rating has been submitted",
      });

      setSelectedPlace(null);
      setPlaceRating(0);
      setPlaceQuery("");
    } catch (error) {
      console.error("Error submitting place rating:", error);
      toast("Error", {
        description: "There was a problem submitting your rating",
      });
    } finally {
      setIsSubmitting(false);
    }
  };

  const fetchBooks = async () => {
    if (!query) {
      setBookSuggestions([]);
      return;
    }
    try {
      const res = await axios.get(
        `https://www.googleapis.com/books/v1/volumes?q=${query}&key=${process.env.NEXT_PUBLIC_BOOKS_KEY}`
      );
      setBookSuggestions(res.data.items || []);
      setOpen(res.data.items?.length > 0);
    } catch (error) {
      console.error("Error fetching books:", error);
    }
  };

  const fetchPlaces = async () => {
    if (!placeQuery) {
      setPlaceSuggestions([]);
      return;
    }
    try {
      const res = await axios.post(
        `https://places.googleapis.com/v1/places:searchText`,
        {
          textQuery: placeQuery,
          languageCode: "en",
          includedType: "restaurant",
        },
        {
          headers: {
            "Content-Type": "application/json",
            "X-Goog-Api-Key": process.env.NEXT_PUBLIC_PLACES_KEY,
            "X-Goog-FieldMask": "*",
          },
        }
      );
      setPlaceSuggestions(res.data.places || []);
      setOpenPlace(res.data.places?.length > 0);
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    fetchPlaces();
  }, [placeQuery]);

  useEffect(() => {
    fetchBooks();
  }, [query]);

  const formatBookForCard = (volumeInfo) => {
    if (!volumeInfo) return null;
    return {
      title: volumeInfo.title || "Unknown Title",
      authors: volumeInfo.authors || ["Unknown Author"],
      description: volumeInfo.description || "No description available",
      thumbnail: volumeInfo.imageLinks?.thumbnail || null,
      publishedDate: volumeInfo.publishedDate,
    };
  };

  const onPlaceClick = async (selectedPlace) => {
    if (
      selectedPlace != null &&
      selectedPlace.photos.length &&
      selectedPlace.photos.length > 1
    ) {
      const placeUrl = `https://places.googleapis.com/v1/${selectedPlace.photos[0].name}/media?key=${process.env.NEXT_PUBLIC_PLACES_KEY}&maxHeightPx=200&maxWidthPx=200`;
      const res = await axios.get(placeUrl);
      setSelectedImage(placeUrl);
    }
  };

  return (
    <Tabs defaultValue="places" className="w-full max-w-lg mx-auto">
      <TabsList className="w-full mb-6">
        <TabsTrigger value="places" className="flex-1">
          Places
        </TabsTrigger>
        <TabsTrigger value="books" className="flex-1">
          Books
        </TabsTrigger>
        <TabsTrigger value="whatever" className="flex-1">
          Whatever you Want
        </TabsTrigger>
      </TabsList>

      <TabsContent value="books" className="py-4">
        <div className="space-y-6">
          <div>
            <h2 className="text-xl font-bold mb-3">Find a Book to Rate</h2>
            <Input
              type="text"
              placeholder="Search for books..."
              value={query}
              onChange={(e) => {
                setQuery(e.target.value);
              }}
              onFocus={() => setOpen(true)}
              className="w-full"
            />
          </div>

          {open && bookSuggestions.length > 0 && (
            <div className="relative w-full border rounded-md overflow-hidden max-h-60 bg-white dark:bg-gray-800 shadow-lg z-50">
              <div className="overflow-y-auto max-h-60">
                {bookSuggestions.map((book) => (
                  <div
                    key={book?.id}
                    className="p-2 hover:bg-gray-100 dark:hover:bg-gray-700 cursor-pointer"
                    onClick={() => {
                      setSelectedBook(book);
                      setOpen(false);
                    }}
                  >
                    <div className="flex items-center">
                      {book.volumeInfo.imageLinks?.smallThumbnail && (
                        <img
                          src={book.volumeInfo.imageLinks.smallThumbnail}
                          alt={book.volumeInfo.title}
                          className="w-10 h-14 object-cover mr-3"
                        />
                      )}
                      <div>
                        <p className="font-semibold">{book.volumeInfo.title}</p>
                        <p className="text-sm text-gray-600 dark:text-gray-400">
                          {book.volumeInfo.authors?.join(", ") ||
                            "Unknown Author"}
                        </p>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}

          {selectedBook && (
            <div className="mt-6 space-y-6">
              <BookCard
                book={formatBookForCard(selectedBook.volumeInfo)}
                showRating={false}
              />

              <div className="bg-gray-50 dark:bg-gray-800 p-4 rounded-lg">
                <h3 className="text-lg font-medium mb-4">Rate this book:</h3>
                <div className="flex items-center justify-between">
                  <Rating
                    onChange={(value) => setRating(value)}
                    value={rating}
                  />
                  <Button
                    onClick={submitRating}
                    disabled={isSubmitting}
                    className="ml-4"
                  >
                    {isSubmitting ? "Submitting..." : "Submit Rating"}
                  </Button>
                </div>
              </div>
            </div>
          )}
        </div>
      </TabsContent>

      <TabsContent value="places" className="py-4">
        <div className="space-y-6">
          <div>
            <h2 className="text-xl font-bold mb-3">Find a place to Rate</h2>
            <Input
              type="text"
              placeholder="Search for places..."
              value={placeQuery}
              onChange={(e) => {
                setPlaceQuery(e.target.value);
              }}
              onFocus={() => setOpenPlace(true)}
              className="w-full"
            />
          </div>

          {openPlace && placeSuggestions && placeSuggestions.length > 0 && (
            <div className="relative w-full border rounded-md overflow-hidden max-h-60 bg-white dark:bg-gray-800 shadow-lg z-50">
              <div className="overflow-y-auto max-h-60">
                {placeSuggestions.map((place) => (
                  <div
                    key={place?.id}
                    className="p-2 hover:bg-gray-100 dark:hover:bg-gray-700 cursor-pointer"
                    onClick={() => {
                      setSelectedPlace(place);
                      onPlaceClick(place);
                      setOpenPlace(false);
                    }}
                  >
                    <p className="font-semibold">{place.displayName?.text}</p>
                    <p className="text-sm text-gray-600 dark:text-gray-400">
                      {place.formattedAddress || place.shortFormattedAddress}
                    </p>
                  </div>
                ))}
              </div>
            </div>
          )}

          {selectedPlace && (
            <div className="mt-6 space-y-6">
              <PlaceCard
                place={selectedPlace}
                showRating={false}
                rating={placeRating}
                pUrl={selectedImage}
              />

              <div className="bg-gray-50 dark:bg-gray-800 p-4 rounded-lg">
                <h3 className="text-lg font-medium mb-4">Rate this place:</h3>
                <div className="flex items-center justify-between">
                  <Rating
                    onChange={(value) => setPlaceRating(value)}
                    value={placeRating}
                  />
                  <Button
                    onClick={submitPlaceRating}
                    disabled={isSubmitting}
                    className="ml-4"
                  >
                    {isSubmitting ? "Submitting..." : "Submit Rating"}
                  </Button>
                </div>
              </div>
            </div>
          )}
        </div>
      </TabsContent>

      <TabsContent value="whatever" className="py-4">
        <LinkInput />
      </TabsContent>
    </Tabs>
  );
}
