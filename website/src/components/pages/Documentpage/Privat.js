import React from 'react';
import { useAuth0} from "@auth0/auth0-react";

import Aufwandserfassung from '../../../documents/Privat/Apollon_Aufwandserfassung.xlsx';
import StatusberichtKW10 from '../../../documents/Privat/Apollon_Statusbericht_KW10.pdf';
import StatusberichtKW11 from '../../../documents/Privat/Apollon_Statusbericht_KW11.pdf';


const Privat = () => {
    const { isAuthenticated } = useAuth0();
    if (isAuthenticated) {
        return(
                <div class="card mt-3" style={{textAlign: "left"}}>
                    <div class="card-body text-align-left">
                        <h6>Allgemein:</h6>
                        <ul>
                            <li><a href={Aufwandserfassung} >Aufwandserfassung.xlsx</a> <span class="badge badge-dark" style={{background: "#31574B"}}>Neu</span></li>
                        </ul>
                        <h6>Statusberichte:</h6>
                        <ul>
                            <li><a href={StatusberichtKW10} >Statusbericht_KW10.pdf</a></li>
                            <li><a href={StatusberichtKW11} >Statusbericht_KW11.pdf</a> <span class="badge badge-dark" style={{background: "#31574B"}}>Neu</span></li>
                        </ul>
                    </div>
                </div>      
        );
    }
    return (
        <div class="alert mt-4">
            <div class="alert alert-info" role="alert">
                Sie müssen angemeldet sein, um diesen Bereich zusehen!
            </div>
        </div>
    );
}

export default Privat;