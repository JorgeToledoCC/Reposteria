export interface Producto{
    id :Guid,  
    nombre :string, 
    descripcion :string,
    precio :decimal ,
    stock :int ,
    nombreCategoria :string
}
interface Categoria {
  nombre: string
  descripcion?: string
}