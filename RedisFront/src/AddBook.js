import axios from "axios";
import { getRole, getUsername } from "./utils";
const AddBook = () => {
  const token = localStorage.getItem("token");
  //var userId = getUserId();
  var userRole = getRole();
  var userUsername = getUsername();
  const addBook = (e) => {
    e.preventDefault();
    var isbn = e.target.isbn.value;
    var naslov = e.target.naslov.value;
    var autor = e.target.autor.value;
    var zanr = e.target.zanr.value;
    var datIz = e.target.datIz.value;
    datIz = datIz + "T00:00:00.000Z";
    var uok = e.target.uok.value;

    const book = {
      isbn: isbn,
      naslov: naslov,
      autor: autor,
      zanr: zanr,
      datumIzdavanja: datIz,
      ukratkoOKnjizi: uok,
    };
    console.log(book);
    axios
      .post("https://localhost:7119/Book/AddBook/", book, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then(window.location.reload())
      .catch((err) => console.log(err.message));
  };

  return (
    <div>
      <form onSubmit={(e) => addBook(e)}>
        <h3>ADD BOOK</h3>
        <div className="mb-3">
          <label>ISBN</label>
          <input
            id="isbn"
            type="text"
            className="form-control"
            placeholder="ISBN"
          />
        </div>
        <div className="mb-3">
          <label>Naslov</label>
          <input
            id="naslov"
            type="text"
            className="form-control"
            placeholder="naslov"
          />
        </div>
        <div className="mb-3">
          <label>Autor</label>
          <input
            id="autor"
            type="text"
            className="form-control"
            placeholder="Autor"
          />
        </div>
        <div className="mb-3">
          <label>Zanr</label>
          <input
            id="zanr"
            type="text"
            className="form-control"
            placeholder="Zanr"
          />
        </div>
        <div className="mb-3">
          <label>Datum izavanja</label>
          <input
            id="datIz"
            type="date"
            className="form-control"
            placeholder="Datum izdavanja"
          />
        </div>
        <div className="mb-3">
          <label>Ukratko o knjizi</label>
          <input
            id="uok"
            type="text"
            className="form-control"
            placeholder="Ukratko o knjizi"
          />
        </div>
        <div className="d-grid">
          <button type="submit" className="btn btn-success">
            Create
          </button>
        </div>
      </form>
    </div>
  );
};
export default AddBook;
