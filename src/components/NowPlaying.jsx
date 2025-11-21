import { useEffect, useState } from "react";

// NowPlaying shows a mocked "now playing" entry; optionally you can fetch from backend if available
export default function NowPlaying(){
  const [now, setNow] = useState({
    title: "Loading...",
    artist: ""
  });

  useEffect(()=>{
    // If your backend exposes now-playing, call it. For now we mock.
    setTimeout(()=> setNow({ title: "Morning Jazz Hour", artist: "DJ Alice" }), 300);
  },[]);

  return (
    <div className="card now-playing">
      <div className="title">Now Playing</div>
      <div style={{fontSize:"1.1rem", marginTop:8}}>{now.title}</div>
      <div className="meta">{now.artist}</div>
      <div style={{marginTop:12}}>
        <button onClick={()=>alert("Playing demo stream")} style={{padding:"0.5rem 0.8rem"}}>▶ Listen</button>
      </div>
    </div>
  );
}
