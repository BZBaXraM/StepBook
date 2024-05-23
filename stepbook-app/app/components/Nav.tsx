"use client";

import { useState, useEffect } from "react";
import Link from "next/link";
import { AuthService } from "../services/auth-service";
import { useRouter } from "next/navigation";

const Nav = () => {
    const service = new AuthService();
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const router = useRouter();

    useEffect(() => {
        // Check if the user is already logged in
        const token = localStorage.getItem("accessToken");
        if (token) {
            setIsLoggedIn(true);
        }
    }, []);

    const logout = async () => {
        try {
            await service.logout();
            console.log("Logout successful");
            setIsLoggedIn(false);
            localStorage.removeItem("accessToken");
            router.push("/"); // Redirect to login page after logout
        } catch (error) {
            console.error("Logout failed:", error);
        }
    };

    return (
        <nav className="bg-gray-800">
            <div className="mx-auto max-w-7xl px-2 sm:px-6 lg:px-8">
                <div className="relative flex h-16 items-center justify-between">
                    <div className="flex flex-1 items-center justify-center sm:items-stretch sm:justify-start">
                        <div className="hidden sm:ml-6 sm:block">
                            <div className="flex space-x-4">
                                {isLoggedIn ? (
                                    <>
                                        <Link
                                            href="/mathces"
                                            className="text-gray-300 hover:bg-gray-700 hover:text-white rounded-md px-3 py-2 text-sm font-medium"
                                        >
                                            Mathces
                                        </Link>
                                        <Link
                                            href="/lists"
                                            className="text-gray-300 hover:bg-gray-700 hover:text-white rounded-md px-3 py-2 text-sm font-medium"
                                        >
                                            Lists
                                        </Link>
                                        <Link
                                            href="/messages"
                                            className="text-gray-300 hover:bg-gray-700 hover:text-white rounded-md px-3 py-2 text-sm font-medium"
                                        >
                                            Messages
                                        </Link>
                                        <button
                                            onClick={logout}
                                            className="text-gray-300 hover:bg-gray-700 hover:text-white rounded-md px-3 py-2 text-sm font-medium"
                                        >
                                            Logout
                                        </button>
                                    </>
                                ) : (
                                    <>
                                        <Link
                                            href="/login"
                                            className="text-gray-300 hover:bg-gray-700 hover:text-white rounded-md px-3 py-2 text-sm font-medium"
                                        >
                                            Login
                                        </Link>
                                    </>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </nav>
    );
};

export default Nav;
