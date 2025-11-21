// src/pages/Home.jsx
import { useEffect, useState } from "react";
import { getTodaySchedule } from "../services/api";
import FeaturedShow from "../components/FeaturedShow";
import ScheduleList from "../components/ScheduleList";
import LatestNews from "../components/LatestNews";

export default function Home() {
  const [todayEvents, setTodayEvents] = useState([]);

  useEffect(() => {
    getTodaySchedule().then(setTodayEvents).catch(console.error);
  }, []);

  const now = new Date();
  const currentShow = todayEvents.find(e => new Date(e.startTime) <= now && new Date(e.endTime) >= now);

  return (
    <div style={{ maxWidth: "800px", margin: "0 auto", padding: "20px" }}>
      <h1>Welcome to RadioStation</h1>

      <FeaturedShow show={currentShow || todayEvents[0]} />

      <LatestNews />

      <ScheduleList />
    </div>
  );
}
