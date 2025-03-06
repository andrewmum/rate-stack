// utils/api.ts

/**
 * API Response interface
 */
export interface ApiResponse<T = any> {
  data?: T;
  error?: string;
  status: number;
}

/**
 * Request options extending the fetch RequestInit
 */
export interface RequestOptions extends RequestInit {
  headers?: HeadersInit;
}

const API_BASE_URL =
  process.env.NEXT_PUBLIC_API_URL || "https://localhost:7161/api";

/**
 * Generic API client for handling requests to your C# backend
 */
const api = {
  /**
   * Make a GET request
   * @param endpoint - The API endpoint (without the /api prefix)
   * @param options - Additional fetch options
   * @returns Promise with the response data
   */
  async get<T = any>(
    endpoint: string,
    options: RequestOptions = {}
  ): Promise<T> {
    return this.request<T>(endpoint, { ...options, method: "GET" });
  },

  /**
   * Make a POST request
   * @param endpoint - The API endpoint (without the /api prefix)
   * @param data - The data to send
   * @param options - Additional fetch options
   * @returns Promise with the response data
   */
  async post<T = any>(
    endpoint: string,
    data: any,
    options: RequestOptions = {}
  ): Promise<T> {
    return this.request<T>(endpoint, {
      ...options,
      method: "POST",
      body: JSON.stringify(data),
    });
  },

  /**
   * Make a PUT request
   * @param endpoint - The API endpoint (without the /api prefix)
   * @param data - The data to send
   * @param options - Additional fetch options
   * @returns Promise with the response data
   */
  async put<T = any>(
    endpoint: string,
    data: any,
    options: RequestOptions = {}
  ): Promise<T> {
    return this.request<T>(endpoint, {
      ...options,
      method: "PUT",
      body: JSON.stringify(data),
    });
  },

  /**
   * Make a DELETE request
   * @param endpoint - The API endpoint (without the /api prefix)
   * @param options - Additional fetch options
   * @returns Promise with the response data
   */
  async delete<T = any>(
    endpoint: string,
    options: RequestOptions = {}
  ): Promise<T> {
    return this.request<T>(endpoint, { ...options, method: "DELETE" });
  },

  /**
   * Base request method
   * @param endpoint - The API endpoint (without the /api prefix)
   * @param options - Fetch options
   * @returns Promise with the response data
   */
  async request<T = any>(
    endpoint: string,
    options: RequestOptions = {}
  ): Promise<T> {
    const headers = {
      "Content-Type": "application/json",
      ...options.headers,
    };

    const config = {
      ...options,
      headers,
    };

    // Make sure the endpoint starts with a slash
    const normalizedEndpoint = endpoint.startsWith("/")
      ? endpoint
      : `/${endpoint}`;
    const url = `${API_BASE_URL}${normalizedEndpoint}`;

    try {
      const response = await fetch(url, config);
      const data = await response.json();

      if (!response.ok) {
        throw new Error(data.message || "Something went wrong");
      }

      return data as T;
    } catch (error) {
      console.error("API request failed:", error);
      throw error;
    }
  },
};

export default api;
