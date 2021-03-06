import React from 'react';
import { useAuth0} from "@auth0/auth0-react";

import Aufwandserfassung from '../../../documents/Privat/Apollon_Aufwandserfassung.xlsx';
import UserStories from '../../../documents/Privat/Apollon_09052021_UserStories.pdf';

import StatusberichtKW10 from '../../../documents/Privat/Apollon_Statusbericht_KW10.pdf';
import StatusberichtKW11 from '../../../documents/Privat/Apollon_Statusbericht_KW11.pdf';
import StatusberichtKW12 from '../../../documents/Privat/Apollon_Statusbericht_KW12.pdf';
import StatusberichtKW13 from '../../../documents/Privat/Apollon_Statusbericht_KW13.pdf';
import StatusberichtKW14 from '../../../documents/Privat/Apollon_Statusbericht_KW14.pdf';
import StatusberichtKW15 from '../../../documents/Privat/Apollon_Statusbericht_KW15.pdf';
import StatusberichtKW16 from '../../../documents/Privat/Apollon_Statusbericht_KW16.pdf';
import StatusberichtKW17 from '../../../documents/Privat/Apollon_Statusbericht_KW17.pdf';
import StatusberichtKW18 from '../../../documents/Privat/Apollon_Statusbericht_KW18.pdf';
import StatusberichtKW19 from '../../../documents/Privat/Apollon_Statusbericht_KW19.pdf';



const Privat = () => {
    const { isAuthenticated } = useAuth0();
    if (isAuthenticated) {
        return(
                <div class="card mt-3" style={{textAlign: "left"}}>
                    <div class="card-body text-align-left">
                        <div class="row">
                            <div class="col" style={{paddingRight: 0}}>
                                <h6>Allgemein:</h6>
                                <ul>
                                    <li><a href={Aufwandserfassung} >Aufwandserfassung.xlsx</a> <span class="badge badge-dark" style={{background: "#31574B"}}>Neu</span></li>
                                    <li><a href={UserStories}>UserStories.pdf</a></li>
                                </ul>
                                <h6>Statusberichte:</h6>
                                <ul>
                                    <li><a href={StatusberichtKW10} >Statusbericht_KW10.pdf</a></li>
                                    <li><a href={StatusberichtKW11} >Statusbericht_KW11.pdf</a></li>
                                    <li><a href={StatusberichtKW12} >Statusbericht_KW12.pdf</a></li>
                                    <li><a href={StatusberichtKW13} >Statusbericht_KW13.pdf</a></li>
                                    <li><a href={StatusberichtKW14} >Statusbericht_KW14.pdf</a></li>
                                    <li><a href={StatusberichtKW15} >Statusbericht_KW15.pdf</a></li>
                                    <li><a href={StatusberichtKW16} >Statusbericht_KW16.pdf</a></li>
                                    <li><a href={StatusberichtKW17} >Statusbericht_KW17.pdf</a></li>
                                    <li><a href={StatusberichtKW18} >Statusbericht_KW18.pdf</a></li>
                                    <li><a href={StatusberichtKW19} >Statusbericht_KW19.pdf</a> <span class="badge badge-dark" style={{background: "#31574B"}}>Neu</span></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>      
        );
    }
    return (
        <div class="alert mt-4">
            <div class="alert alert-info" role="alert">
                Sie m??ssen angemeldet sein, um diesen Bereich zu sehen!
            </div>
        </div>
    );
}

export default Privat;