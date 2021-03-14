import React, {Component} from 'react';

import './Impressum.css'


const Impressum = () =>{
    return (
    <React.Fragment> 
        <div class="container mt-4 vh-100">
            <p><strong>Impressum</strong></p>
            <p>Anbieter:<br />Max Mustermann<br />Musterstraße 1<br />80999 München</p>
            <p>Kontakt:<br />Telefon: 089/1234567-8<br />Telefax: 089/1234567-9<br />E-Mail: mail@mustermann.de<br />Website: www.mustermann.de</p>
        </div>
    </React.Fragment>
    );
}

export default Impressum;