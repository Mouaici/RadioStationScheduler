import { useEffect, useState } from "react";
import { getTodaySchedule } from "../services/api";

export default function SchedulePreview(){
  const [items, setItems] = useState([]);
  const [err, setErr] = useState("");

  useEffect(()=>{
    let mounted = true;
    getTodaySchedule()
      .then(data => { if(mounted) setItems(data || []) })
      .catch(e => { console.error(e); setErr("Could not load schedule"); });
    return ()=> mounted = false;
  },[]);

  return (
    <div className="card schedule-preview">
      <h3>Today's Schedule</h3>
      {err && <div style={{color:"red"}}>{err}</div>}
      <ul>
        {items.length === 0 && <li>No events scheduled for today.</li>}
        {items.slice(0,6).map(ev => (
          <li key={ev.id}>
            <strong>{ev.title}</strong><br/>
            <small>{new Date(ev.startTime).toLocaleTimeString([], {hour:"2-digit", minute:"2-digit"})} — {new Date(ev.endTime).toLocaleTimeString([], {hour:"2-digit", minute:"2-digit"})}</small>
          </li>
        ))}
      </ul>
      <div style={{marginTop:8}}>
        <a href="/schedule">View full schedule</a>
      </div>
    </div>
  );
}
