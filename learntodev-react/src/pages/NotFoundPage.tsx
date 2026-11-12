import {Link} from "react-router";

export default function NotFoundPage() {
  return (
    <div className="card">
      <h1>Side ikke funnet</h1>
      <p>Siden du leter etter finnes ikke.</p>
      <Link className="btn" to="/">
        Gå tilbake til forsiden
      </Link>
    </div>
  );
}
