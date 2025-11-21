import { useEffect, useState } from "react";
import { getTodaySchedule } from "../services/api";

export default function ScheduleList() {
  const [events, setEvents] = useState([]);

  useEffect(() => {
    getTodaySchedule().then(setEvents).catch(console.error);
  }, []);

  return (
    <section>
      <h3>Today's Schedule</h3>
      {events.length === 0 && <p>No shows today</p>}
      <ul>
        {events.map((e) => (
          <li key={e.id}>
            <strong>{e.title}</strong> ({new Date(e.startTime).toLocaleTimeString()} - {new Date(e.endTime).toLocaleTimeString()})
          </li>
        ))}
      </ul>
    </section>
  );
}
