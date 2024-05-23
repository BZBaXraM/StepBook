import axios from "axios";

type Login = {
    email: string;
    password: string;
};

export class AuthService {
    async login({ email, password }: Login) {
        try {
            const response = await fetch(
                "https://localhost:7035/api/Auth/login",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({ email, password }),
                }
            );
            if (!response.ok) {
                throw new Error(`Error: ${response.statusText}`);
            }
            const data = await response.json();
            if (typeof window !== "undefined") {
                window.localStorage.setItem("accessToken", data.token);
            }
            return data;
        } catch (error) {
            console.error("Error during login:", error);
            throw error;
        }
    }

    async register({ email, password }: Login) {
        try {
            const response = await axios.post(
                "https://localhost:7035/api/auth/register",
                {
                    email,
                    password,
                },
                {
                    httpsAgent: new (require("https").Agent)({
                        rejectUnauthorized: false,
                    }),
                }
            );
            return response.data;
        } catch (error) {
            console.error("Error during registration:", error);
            throw error;
        }
    }

    async logout() {
        if (typeof window !== "undefined") {
            window.localStorage.removeItem("accessToken");
        }
    }
}
