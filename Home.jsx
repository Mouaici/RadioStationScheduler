import NowPlaying from "../components/NowPlaying";
import FeaturedShows from "../components/FeaturedShows";
import SchedulePreview from "../components/SchedulePreview";

export default function Home(){
  return (
    <div>
      <section className="hero card" style={{marginBottom:12}}>
        <div>
          <h1>RadioStation — Today's Hits and Classics</h1>
          <p style={{color:"#666"}}>Live shows, guest interviews and the best music.</p>
          <div className="cta">
            <button onClick={()=>alert("Listen live pressed")} style={{padding:"0.6rem 0.9rem"}}>▶ Listen Live</button>
            <a href="/schedule" style={{marginLeft:8}}>Today's Schedule</a>
          </div>
        </div>
        <div style={{textAlign:"right"}}>
          <img src="" alt="" style={{width:160, opacity:0.9}} />
        </div>
      </section>

      <div className="grid">
        <div>
          <NowPlaying />
          <div style={{height:12}} />
          <FeaturedShows />
        </div>
        <div>
          <SchedulePreview />
        </div>
      </div>
    </div>
  );
}
