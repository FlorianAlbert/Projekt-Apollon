import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Link } from 'react-router-dom';

const LogoutButton = () => {
  const { logout } = useAuth0();
  return (
    <Link
      className="nav-link"
      onClick={() => logout({returnTo: window.location.origin,})}
      style={{color: "white"}}
    >
      <span style={{textDecoration: "underline", textDecorationColor: "#3fb3da"}}>LogOut</span>
    </Link>
  );
};

export default LogoutButton;