// components/LinkInput.js
import { useState } from "react";
import LinkPreview from "./LinkPreview";

export default function LinkInput() {
  const [inputValue, setInputValue] = useState("");
  const [url, setUrl] = useState(null);

  const isValidUrl = (string) => {
    try {
      new URL(string);
      return true;
    } catch (err) {
      return false;
    }
  };

  const handleInputChange = (e) => {
    setInputValue(e.target.value);
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (isValidUrl(inputValue)) {
      setUrl(inputValue);
    } else {
      alert("Please enter a valid URL");
    }
  };

  const handlePaste = (e) => {};

  return (
    <div className="w-full max-w-2xl mx-auto">
      <form onSubmit={handleSubmit} className="mb-4">
        <div className="flex">
          <input
            type="text"
            value={inputValue}
            onChange={handleInputChange}
            onPaste={handlePaste}
            placeholder="Paste a URL here"
            className="flex-grow p-2 border rounded-l-md focus:outline-none focus:ring-2 focus:ring-blue-300"
          />
          <button
            type="submit"
            className="bg-blue-500 text-white px-4 py-2 rounded-r-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-300"
          >
            Preview
          </button>
        </div>
      </form>

      {url && <LinkPreview url={url} />}
    </div>
  );
}
