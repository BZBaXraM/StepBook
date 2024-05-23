import axios from "axios";

type User = {
    id: number;
    email: string;
};

const fetchUsers = async () => {
    const response = await axios.get<User[]>(
        "https://localhost:7035/api/users",
        {
            httpsAgent: new (require("https").Agent)({
                rejectUnauthorized: false,
            }),
        }
    );
    return response.data;
};

export const Users = async () => {
    const users = await fetchUsers();
    return (
        <div className="flex flex-col items-center">
            <h1 className="text-2xl font-bold mb-4">Users</h1>
            <ul className="grid grid-cols-1 gap-4">
                {users.map((user) => (
                    <li key={user.id} className={"bg-slate-950 p-5 border"}>
                        <span>{user.email}</span>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default Users;
