import React from 'react';
import Etienne from '../../../images/Etienne.jpg';
import Leon from '../../../images/Leon.jpg';
import Paul from '../../../images/Paul.png';
import Dani from '../../../images/Dani.png';
import Flo from '../../../images/Flo.png';
import Alfred from '../../../images/Alfred.png';
import Bild1 from '../../../images/Apollon.jpg';
import Bild2 from '../../../images/Wuerfel.jpg';
import Bild3 from '../../../images/Projektplan.png';

import './Homepage.css'


const Homepage = () =>{
    return (
        <React.Fragment>         
        <div class="container-fluid intro" id="intro">
            <div class="mask ">
                <div class="container-fluid d-flex align-items-center justify-content-center h-100">
                    <div class="row d-flex justify-content-center text-center">
                        <div class="col-md-12">
                            <h1 class="display-4 font-weight-bold white-text pt-5 mb-2" style={{marginTop:"20%"}}>Projekt Apollon</h1>
                            <hr class="hr-light" />
                            <a href="https://github.com/FlorianAlbert/Projekt-Apollon"><button type="button" class="btn btn-lg btn-outline-light" style={{fontSize:18}}>Github</button></a>
                            <a href="http://api.apollon-dungeons.de/api/index.html"><button type="button" class="btn btn-lg btn-outline-light" style={{fontSize:18, marginLeft:"2em"}}>Dokumentation</button></a>
                            <a href="http://mud.apollon-dungeons.de/"><button type="button" class="btn btn-lg btn-outline-light" style={{fontSize:18, marginLeft:"2em"}}>Produkt</button></a>
                         </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="container marketing" id="marketing">
        <h1>Das Team</h1>
        <div class="row mt-5">
            <div class="col-lg-4">
            <img class="rounded-circle" src={Paul} alt="Paul" width="140" height="140"/>
            <h2>Paul Burkard</h2>
            <p>Projektleiter</p>
            </div>
            <div class="col-lg-4">
            <img class="rounded-circle" src={Flo} alt="Florian" width="140" height="140"/>
            <h2>Florian Albert</h2>
            <p>Verantwortlicher f??r Implementierung</p>
            </div>
            <div class="col-lg-4">
            <img class="rounded-circle" src={Etienne} alt="Etienne" width="140" height="140"/>
            <h2>Etienne Zink</h2>
            <p>Technischer Assistent & Verantwortlicher f??r Modellierung</p>
            </div>
        </div>
        <div class="row mt-5">
            <div class="col-lg-4">
            <img class="rounded-circle" src={Dani} alt="Daniel" width="140" height="140"/>
            <h2>Daniel Kr??ker</h2>
            <p>Verantwortlicher f??r Qualit??tssicherung und Dokumentation</p>
            </div>
            <div class="col-lg-4">
            <img class="rounded-circle" src={Alfred} alt="Alfred" width="140" height="140"/>
            <h2>Alfred Rustemi</h2>
            <p>Verantwortlicher f??r Recherche</p>
            </div>
            <div class="col-lg-4">
            <img class="rounded-circle" src={Leon} alt="Leon" width="140" height="140"/>
            <h2>Leon Jerke</h2>
            <p>Verantwortlicher f??r Tests</p>
            </div>
        </div>

        <hr class="featurette-divider"/>

        <div class="row featurette">
            <div class="col-md-7">
            <h2 class="featurette-heading">Projekt Apollon. <span class="text-muted">Woher kommt der Name?</span></h2>
            <p class="lead">Apollon ist ein griechisch-r??mischer Gott, geboren auf der Insel Delos: einer heiligen St??tte. Im selben Stile wurde dieses Projekt gegr??ndet, aufbauend auf unserer heiligen St??tte des Zusammenhalts, der Teamarbeit und dem h??chsten Grade an Motivation, das bestm??gliche Ergebnis f??r unsere Kunden zu liefern. Als Besch??tzer der K??nste ist Apollon ein Mahnmal unseres Teams, selbst Produkte zu entwickeln, die als "Kunst" gesehen werden.</p>
            </div>
            <div class="col-md-5">
            <img class="featurette-image img-fluid mx-auto" src={Bild1} alt="Bild1"/>
            </div>
        </div>

        <hr class="featurette-divider"/>

        <div class="row featurette">
            <div class="col-md-7 order-md-2">
            <h2 class="featurette-heading">Die Aufgabe. <span class="text-muted">Worum geht es?</span></h2>
            <p class="lead"> Ziel des Projekts ist die Entwicklung einer Multi User Dungeon Umgebung. Mit dieser soll es m??glich sein ein Spiel als Dungeon Master zu konfigurieren und zu hosten oder als normaler Spieler, mit einem eigens erstellten Avatar, an einem dieser Spiele teilzunehmen.</p>
            </div>
            <div class="col-md-5 order-md-1">
            <img class="featurette-image img-fluid mx-auto" src={Bild2} alt="Bild2"/>
            </div>
        </div>

        <hr class="featurette-divider"/>

        <div class="row featurette">
            <div class="col-md-7">
                <h2 class="featurette-heading">Der Projektplan. <span class="text-muted">Wie ist der Ablauf?</span></h2>
                <p class="lead">Die Grundlage eines jeden erfolgreichen Projekts ist die Planung. Ein ausgekl??gelter Projektplan erm??glicht es dem Team, jederzeit ??berblick ??ber die Deadlines und die bis dahin zu erledigenden Aufgaben zu behalten.</p>
                <a class="btn" href="/documents#" role="button" style={{background: "#3fb3da", color: "white"}}>Zum Projektplan</a>
            </div>
            <div class="col-md-5">
            <a href="/documents#"><img class="featurette-image img-fluid mx-auto" src={Bild3} alt="Bild3"/></a>
            </div>
        </div>

        <hr class="featurette-divider"/>
        </div>
    </React.Fragment>
    );
}

export default Homepage;