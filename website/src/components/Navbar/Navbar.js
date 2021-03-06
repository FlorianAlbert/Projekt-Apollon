import React from 'react';
import { Link } from 'react-router-dom';
import './Navbar.css';
import Logo from "../../images/harp.svg"
import AuthNav from "../Auth/AuthNav";

const Navbar = (props) => {
    return (
        <nav className="navbar sticky-top navbar-expand-lg navbar-dark bg-dark">
            <div className="container">
                <Link className="navbar-brand" to="/"> <img
                    src={Logo}
                    width="40"
                    height="40"
                    className=" align-middle"
                    alt="logo"
                    style={{marginRight: 8}}
                    />
                       Projekt <span style={{color: "#3fb3da"}}>Apollon</span>
                </Link>
                <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarResponsive" aria-controls="navbarResponsive" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarResponsive">
                    <ul className="navbar-nav ms-auto">
                        <li class="nav-item">
                            <Link className="nav-link" to="/documents"><span style={{textDecoration: "underline", textDecorationColor: "#3fb3da"}}>Dokumente</span></Link>
                        </li>
                        <AuthNav/>
                    </ul>
                </div>
                
            </div>
        </nav>
    )
}

export default Navbar;