import Image from "next/image";
import Nav from "./components/Nav";

export default function Home() {
    return (
        <div>
            <Nav />
            <p className="text-center text-4xl">Welcome to StepBook</p>
        </div>
    );
}
