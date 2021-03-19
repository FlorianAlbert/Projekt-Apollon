import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Link } from 'react-router-dom';

const LoginButton = () => {
  const { loginWithRedirect } = useAuth0();
  return (
    <Link class="nav-link" onClick={() => loginWithRedirect()}>
        <span style={{textDecoration: "underline", textDecorationColor: "#3fb3da"}}>Login</span>
    </Link>
  );
};

export default LoginButton;