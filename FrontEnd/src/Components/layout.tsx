import { Outlet } from "react-router-dom";
import ResponsiveAppBar from "./app-bar";

function Layout() {
    return (
        <>
            <ResponsiveAppBar/>

            <Outlet/>

            <h2>Footer</h2>
        </>
    );
}

export default Layout;