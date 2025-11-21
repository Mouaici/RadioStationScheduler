export default function FeaturedShow({ show }) {
  if (!show) return null;

  return (
    <section style={{ background: "#f3f3f3", padding: "20px", borderRadius: "10px", marginBottom: "20px" }}>
      <h2>Now on Air: {show.title}</h2>
      <p>
        {new Date(show.startTime).toLocaleTimeString()} - {new Date(show.endTime).toLocaleTimeString()}
      </p>
      <p>Hosts: {show.hosts.join(", ")}</p>
    </section>
  );
}
