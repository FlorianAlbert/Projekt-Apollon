import React from 'react';
import Privat from "./Privat";


/* Imports Allgemein */
import Projektplan from '../../../documents/allgemein/Apollon_Projektplan.xlsx';

/* Imports KW10 */
import Risikoanalyse from '../../../documents/KW10/Apollon_Risikoanalyse.xlsx';
import Rollenverteilung from '../../../documents/KW10/Apollon_Rollenverteilung.xlsx';
import Aufwandserfassung from '../../../documents/KW10/Apollon_Aufwandserfassung.xlsx';
import Aufgabenblatt1 from '../../../documents/KW10/Aufgabenblatt1.pdf';

/* Imports KW11 */
import Aufgabenblatt2 from '../../../documents/KW11/Aufgabenblatt2.pdf';


import './Documentpage.css'


const Documentpage = () =>{
    return (
    <React.Fragment> 
        <div class="container mt-4 vh-100 dokumente">
            <h1>Dokumente</h1>

            <ul class="nav nav-tabs" role="tablist">
                <li class="nav-item active"><a class="nav-link" href="#allgemein" data-toggle="tab"><span style={{color: "black"}}><h5>Allgemeine Dokumente</h5></span></a></li>
                <li class="nav-item"><a class="nav-link" href="#aufgaben" data-toggle="tab"><span style={{color: "black"}}><h5>Wöchentliche Abgaben</h5></span></a></li>
                <li class="nav-item"><a class="nav-link" href="#privat" data-toggle="tab"><span style={{color: "black"}}><h5>Privater Bereich</h5></span></a></li>
            </ul>

            <div class="tab-content">
                

                <div class="tab-pane fade hide" id="privat" role="tabpanel">
                    <Privat/>
                </div>

                <div class="tab-pane show fade active" id="allgemein" role="tabpanel">
                    <div class="card mt-3" style={{textAlign: "left"}}>
                        <div class="card-body text-align-left">
                            <ul>
                                <li><a href={Projektplan} >Projektplan.xlsx</a> <span class="badge badge-dark" style={{background: "#31574B"}}>Neu</span></li>
                            </ul>
                        </div>
                    </div>      
                </div>

                <div class="tab-pane fade hide" id="aufgaben" role="tabpanel">
                    <div class="accordion mt-3" id="accordion">

                        <div class="accordion-item">
                            <h2 class="accordion-header" id="headingOne">
                            <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                <span class="text">KW 10: Initialisierungsphase</span>
                            </button>
                            </h2>
                            <div id="collapseOne" class="accordion-collapse collapse" aria-labelledby="headingOne" data-bs-parent="#accordion">
                                <div class="accordion-body">
                                    <ul>
                                        <li><a href={Aufgabenblatt1} >Aufgabenblatt1.pdf</a></li>
                                        <li><a href={Risikoanalyse} >Risikoanalyse.xlsx</a></li>
                                        <li><a href={Rollenverteilung}>Rollenverteilung.xlsx</a></li> 
                                        <li><a href={Aufwandserfassung}>Aufwandserfassung.xlsx</a></li> 

                                    </ul>
                                </div>
                            </div>
                        </div>

                        <div class="accordion-item">
                            <h2 class="accordion-header" id="headingTwo">
                            <button class="accordion-button show" type="button" data-bs-toggle="collapse" data-bs-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                <span class="text">KW 11: Recherchephase</span>
                            </button>
                            </h2>
                            <div id="collapseTwo" class="accordion-collapse collapse show" aria-labelledby="headingTwo" data-bs-parent="#accordion">
                            <div class="accordion-body">
                                <ul>
                                    <li><a href={Aufgabenblatt2} >Aufgabenblatt2.pdf</a></li>

                                </ul>                               
                            </div>
                            </div>
                        </div>

                        <div class="accordion-item">
                            <h2 class="accordion-header" id="headingThree">
                            <button class="accordion-button collapsed" type="button" data-bs-toggle="hide" data-bs-target="#collapseThree" aria-expanded="false" aria-controls="collapseThree" disabled>
                                <span class="text-muted">KW 12: Einarbeitungsphase </span>
                            </button>
                            </h2>
                            <div id="collapseThree" class="accordion-collapse collapse" aria-labelledby="headingThree" data-bs-parent="#accordion">
                            <div class="accordion-body">
 
                            </div>
                            </div>
                        </div>

                        <div class="accordion-item">
                            <h2 class="accordion-header" id="headingFour">
                            <button class="accordion-button collapsed" type="button" data-bs-toggle="hide" data-bs-target="#collapseFour" aria-expanded="false" aria-controls="collapseFour" disabled>
                                <span class="text-muted">KW 13: Anforderungsanalyse</span>
                            </button>
                            </h2>
                            <div id="collapseFour" class="accordion-collapse collapse" aria-labelledby="headingFour" data-bs-parent="#accordion">
                            <div class="accordion-body">

                            </div>
                            </div>
                        </div>

                        <div class="accordion-item">
                            <h2 class="accordion-header" id="headingFive">
                            <button class="accordion-button collapsed" type="button" data-bs-toggle="hide" data-bs-target="#collapseFive" aria-expanded="false" aria-controls="collapseFive" disabled>
                                <span class="text-muted">KW 14&15: Implementierungsphase</span>
                            </button>
                            </h2>
                            <div id="collapseFive" class="accordion-collapse collapse" aria-labelledby="headingFive" data-bs-parent="#accordion">
                            <div class="accordion-body">
                              
                            </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </React.Fragment>
    );
}

export default Documentpage;