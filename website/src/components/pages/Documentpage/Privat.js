import React from 'react';
import { useAuth0} from "@auth0/auth0-react";

import Aufwandserfassung from '../../../documents/Privat/Apollon_Aufwandserfassung.xlsx';

const Privat = () => {
    const { isAuthenticated } = useAuth0();
    if (isAuthenticated) {
        return(
                <div class="card mt-3" style={{textAlign: "left"}}>
                    <div class="card-body text-align-left">
                        <ul>
                            <li><a href={Aufwandserfassung} >Aufwandserfassung.xlsx</a> <span class="badge badge-dark" style={{background: "#31574B"}}>Neu</span></li>
                        </ul>
                    </div>
                </div>      
        );
    }
    return (
        <div class="alert mt-4">
            <div class="alert alert-info" role="alert">
                Sie müssen angemeldet sein, um diesen Bereich zu sehen!
            </div>
        </div>
    );
}

export default Privat;