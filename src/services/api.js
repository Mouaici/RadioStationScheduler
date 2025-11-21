// src/services/api.js
const API_BASE = import.meta.env.VITE_API_BASE;

export async function getTodaySchedule() {
  const res = await fetch(`${API_BASE}/schedule/today`);
  if (!res.ok) throw new Error("Failed to fetch schedule");
  return res.json();
}

export async function getNext7Days() {
  const res = await fetch(`${API_BASE}/schedule/next7days`);
  if (!res.ok) throw new Error("Failed to fetch schedule");
  return res.json();
}
