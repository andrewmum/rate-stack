// pages/api/[...path].ts
import type { NextApiRequest, NextApiResponse } from "next";

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse
) {
  const { path } = req.query;
  const pathArray = Array.isArray(path) ? path : [path];
  const apiUrl = process.env.API_URL || "https://localhost:7161/api";
  const url = `${apiUrl}/${pathArray.join("/")}`;

  try {
    const response = await fetch(url, {
      method: req.method,
      headers: {
        "Content-Type": "application/json",
        ...(req.headers.authorization && {
          Authorization: req.headers.authorization as string,
        }),
      },
      body:
        req.method !== "GET" && req.method !== "HEAD"
          ? JSON.stringify(req.body)
          : undefined,
    });

    const data = await response.json();

    res.status(response.status).json(data);
  } catch (error) {
    console.error("API proxy error:", error);
    res.status(500).json({ error: "Failed to fetch data from API" });
  }
}
