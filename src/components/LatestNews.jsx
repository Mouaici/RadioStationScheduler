export default function LatestNews() {
  const news = [
    "New show launched this week!",
    "Tune in for special guest interviews.",
    "Check out our podcast recordings online."
  ];

  return (
    <section>
      <h3>Latest News</h3>
      <ul>
        {news.map((item, index) => <li key={index}>{item}</li>)}
      </ul>
    </section>
  );
}
