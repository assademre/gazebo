import React from 'react';
import NavBar from './NavBar';
import Footer from './Footer/Footer';

const Layout = ({ children }) => {
    return (
        <div>
            <NavBar />
            <main>{children}</main>
            <Footer/>
        </div>
    );
};

export default Layout;