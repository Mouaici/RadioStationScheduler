import { useState } from "react";
import { addEvent } from "../services/api";

export default function ScheduleForm({ onEventAdded }) {
  const [form, setForm] = useState({
    title: "",
    startTime: "",
    endTime: "",
  });

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const newEvent = await addEvent(form);
    onEventAdded(newEvent);
    setForm({ title: "", startTime: "", endTime: "" });
  };

  return (
    <form onSubmit={handleSubmit} style={{ marginBottom: "2rem" }}>
      <h3>Add New Event</h3>
      <input
        type="text"
        name="title"
        placeholder="Title"
        value={form.title}
        onChange={handleChange}
        required
      />
      <input
        type="datetime-local"
        name="startTime"
        value={form.startTime}
        onChange={handleChange}
        required
      />
      <input
        type="datetime-local"
        name="endTime"
        value={form.endTime}
        onChange={handleChange}
        required
      />
      <button type="submit">Add Event</button>
    </form>
  );
}
