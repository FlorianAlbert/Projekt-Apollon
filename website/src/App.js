import './App.css';
import React, {Component} from 'react';
import { BrowserRouter, Switch, Route } from 'react-router-dom'
import Navbar from './components/Navbar/Navbar';
import Cookies from './components/Cookies/Cookies';
import Footer from './components/Footer/Footer';
import Homepage from './components/pages/Homepage/Homepage';
import Documentpage from './components/pages/Documentpage/Documentpage';
import Impressum from './components/pages/Impressum/Impressum';

var comp_Homepage = () =>(
  <div>
    <Homepage/>
  </div>
)

var comp_Documentpage = () =>(
  <div>
    <Documentpage/>
  </div>
)

var comp_Impressum = () =>(
  <div>
    <Impressum/>
  </div>
)


class App extends Component {
  render() {
    return (
      <BrowserRouter>
        <div className="App">
          <Navbar />
          <Cookies/>
          <Switch>
            <Route exact path='/' component={comp_Homepage}/>
            <Route exact path='/documents' component={comp_Documentpage}/>
            <Route exact path='/impressum' component={comp_Impressum}/>
          </Switch>
          <br/>
          <Footer/>
        </div>
      </BrowserRouter>
    );
  }
}

export default App;
