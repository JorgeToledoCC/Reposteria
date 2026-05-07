import { useEffect, useState } from 'react'
import axios from 'axios'
import './App.css'
import logoImg from './assets/logo.png'
import type { Producto, Categoria } from './Lib/Types/definiciones'

function App() {
  const [categorias, setCategorias] = useState<Categoria[]>([])
  const [productos, setProductos] = useState<Producto[]>([])
  const [filtro, setFiltro] = useState<string>("Todas")
  const [loading, setLoading] = useState(true)
  const [busqueda, setBusqueda] = useState<string>("")

  useEffect(() => {
    const cargarDatos = async () => {
      try {
        const [resCat, resProd] = await Promise.all([
          axios.get<Categoria[]>('http://localhost:5244/api/Categoria/mostrar'),
          axios.get<Producto[]>('http://localhost:5244/api/Productos')
        ]);
        setCategorias(resCat.data);
        setProductos(resProd.data);
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };
    cargarDatos();
  }, [])

  const reproducirSonido = () => {
    const audio = new Audio('/vine-boom.mp3');
    audio.play().catch(error => console.log("Error:", error));
  };

  const productosFiltrados = productos.filter(p => {
    const coincideCategoria = filtro === "Todas" || p.nombreCategoria === filtro;
    const coincideBusqueda = p.nombre.toLowerCase().includes(busqueda.toLowerCase());
    return coincideCategoria && coincideBusqueda;
  });

  if (loading) return <div className="loading">Preparando el horno...</div>;

  return (
    <div className="container">
      <header className="main-header">
        <div className="header-brand-wrapper">
          <svg viewBox="0 0 1000 500" className="curved-title-svg">
            <path 
              id="archPath" 
              d="M 120,400 A 380,350 0 0,1 880,400" 
              fill="none"
            />
            <text className="svg-text">
              <textPath href="#archPath" startOffset="50%" textAnchor="middle">
                PASTELERÍA <tspan className="text-highlight">UYUYUI</tspan>
              </textPath>
            </text>
          </svg>
          <div className="logo-container">
            <img 
              src={logoImg} 
              alt="Usagi" 
              className="header-logo-main" 
              onClick={reproducirSonido}
            />
          </div>
        </div>

        <div className="description-container">
          <p className="subtitle">EXPERIENCIAS DULCES CREADAS CON PASIÓN Y TRADICIÓN ARTESANAL</p>
          <div className="subtitle-underline"></div>
        </div>
        
        <div className="search-wrapper">
          <div className="search-box">
            <input 
              type="text" 
              placeholder="Buscar mi postre favorito..." 
              className="search-input"
              value={busqueda}
              onChange={(e) => setBusqueda(e.target.value)}
            />
          </div>
        </div>
      </header>

      <section className="filter-section">
        <div className="category-pills">
          <button className={filtro === "Todas" ? "active" : ""} onClick={() => setFiltro("Todas")}>Todas</button>
          {categorias.map((c, i) => (
            <button key={i} className={filtro === c.nombre ? "active" : ""} onClick={() => setFiltro(c.nombre)}>{c.nombre}</button>
          ))}
        </div>
      </section>

      <main className="product-grid">
        {productosFiltrados.length > 0 ? (
          productosFiltrados.map((p, i) => (
            /* AQUÍ ESTÁ EL CAMBIO: Clase dinámica para el fondo y nueva estructura */
            <div key={i} className={`product-card card-${p.nombreCategoria.toLowerCase().replace(/\s+/g, '-')}`}>
              <div className="card-overlay">
                <div className="card-top">
                  <span className="badge">{p.nombreCategoria}</span>
                </div>
                
                <div className="card-info">
                  <h3>{p.nombre}</h3>
                  <p className="description">{p.descripcion || "Un deleite artesanal para compartir."}</p>
                  
                  <div className="card-footer">
                    <span className="price">Bs. {p.precio}</span>
                    <span className={`stock ${p.stock < 5 ? 'low-stock' : ''}`}>
                      {p.stock < 5 ? `¡Solo quedan ${p.stock}!` : `Stock: ${p.stock}`}
                    </span>
                  </div>
                  
                  <button className="btn-add">Añadir al carrito</button>
                </div>
              </div>
            </div>
          ))
        ) : (
          <div className="no-results">No hay resultados para "{busqueda}"</div>
        )}
      </main>
    </div>
  )
}

export default App