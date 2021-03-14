import React from 'react';
import CookieConsent from "react-cookie-consent";
import './Cookies.css'

const Cookies = () => {
    return (
        <React.Fragment>
            <CookieConsent
                location="bottom"
                buttonText="Akzeptieren"
                cookieName="CookieAbfrage"
                style={{ background: "#212529" }}
                buttonStyle={{ color: "white", fontSize: "13px", background: "#3fb3da" }}
                buttonClasses="btn btn-primary"
                expires={150}
            >
                Wir verwenden Cookies um ihre Benutzung zu erleichtern.{" "}
                <span style={{ fontSize: "13px" }}>Durch die weitere Nutzung unserer Webseite sind Sie mit der Verwendung von Cookies einverstanden.</span>
            </CookieConsent>
        </React.Fragment>
    )
}

export default Cookies;