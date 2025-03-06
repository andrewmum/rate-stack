"use client";
import { useState } from "react";
import { useAuth } from "../context/AuthContext";

const Login = () => {
  const { user, signInWithGoogle, logout } = useAuth();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const handleGoogleSignIn = async () => {
    try {
      setIsLoading(true);
      setError(null);
      await signInWithGoogle();
    } catch (err) {
      setError("Failed to sign in with Google");
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex flex-col items-center justify-center p-4">
      {!user ? (
        <>
          <button
            onClick={handleGoogleSignIn}
            disabled={isLoading}
            className="px-4 py-2 border flex gap-2 bg-white rounded-lg text-gray-700 hover:border-slate-400 hover:shadow transition duration-150"
          >
            <img
              src="https://www.gstatic.com/firebasejs/ui/2.0.0/images/auth/google.svg"
              className="w-6 h-6"
              alt="Google logo"
            />
            <span>{isLoading ? "Signing in..." : "Sign in with Google"}</span>
          </button>
          {error && <p className="mt-2 text-red-500">{error}</p>}
        </>
      ) : (
        <></>
      )}
    </div>
  );
};

export default Login;
