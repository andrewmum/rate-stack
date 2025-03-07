import { useState, useEffect } from "react";
import Image from "next/image";
import api from "@/utils/api";

export default function LinkPreview({ url }) {
  const [preview, setPreview] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!url) return;

    const fetchPreview = async () => {
      try {
        setLoading(true);
        setError(null);

        // Call your API route to fetch metadata
        const response = await api.get(
          `/linkpreview?url=${encodeURIComponent(url)}`
        );

        setPreview(response);
      } catch (err) {
        console.error("Error fetching link preview:", err);
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchPreview();
  }, [url]);

  if (loading) {
    return (
      <div className="p-4 border rounded-md bg-gray-50">Loading preview...</div>
    );
  }

  if (error) {
    return (
      <div className="p-4 border rounded-md bg-red-50 text-red-500">
        Failed to load preview
      </div>
    );
  }

  if (!preview) {
    return null;
  }

  return (
    <a
      href={url}
      target="_blank"
      rel="noopener noreferrer"
      className="block no-underline"
    >
      <div className="border rounded-lg overflow-hidden shadow-sm hover:shadow-md transition-shadow">
        {preview.image && (
          <div className="relative w-full h-48">
            <Image
              src={preview.image}
              alt={preview.title || "Link preview"}
              layout="fill"
              objectFit="cover"
              unoptimized
            />
          </div>
        )}
        <div className="p-4">
          {preview.title && (
            <h3 className="font-medium text-lg mb-1 line-clamp-2">
              {preview.title}
            </h3>
          )}
          {preview.description && (
            <p className="text-gray-600 text-sm line-clamp-3">
              {preview.description}
            </p>
          )}
          <div className="mt-2 flex items-center text-xs text-gray-500">
            {preview.favicon && (
              <Image
                src={preview.favicon}
                width={16}
                height={16}
                alt=""
                className="mr-2"
                unoptimized
              />
            )}
            <span className="truncate">{new URL(url).hostname}</span>
          </div>
        </div>
      </div>
    </a>
  );
}
