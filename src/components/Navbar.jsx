import { NavLink } from "react-router-dom";

export default function Navbar(){
  return (
    <header className="navbar">
      <div className="brand">RadioStation</div>
      <nav className="nav-links">
        <NavLink to="/" end>Home</NavLink>
        <NavLink to="/schedule">Schedule</NavLink>
        <a href="#" onClick={(e)=>{e.preventDefault(); alert("Listen live not implemented in demo");}}>Listen Live</a>
      </nav>
    </header>
  );
}
