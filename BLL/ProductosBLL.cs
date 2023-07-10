using Microsoft.EntityFrameworkCore;
using PruebaExamen2.DAL;
using PruebaExamen2.Models;
using System.Linq.Expressions;

namespace PruebaExamen2.BLL
{
    public class ProductosBLL
    {
        private Contexto _contexto;
        public ProductosBLL(Contexto _contexto)
        {
            this._contexto = _contexto;
        }

        public bool Existe(int productoID)
        {
            return _contexto.Productos.Any(p => p.ProductoId == productoID);
        }

        public bool Insertar(Productos producto)
        {
            _contexto.Productos.Add(producto);
            return _contexto.SaveChanges() > 0;
        }

        public bool Modificar(Productos producto)
        {
            _contexto.Entry(producto).State = EntityState.Modified;
            return _contexto.SaveChanges() > 0;
        }

        public bool Guardar(Productos producto)
        {
            if (!Existe(producto.ProductoId))
                return this.Insertar(producto);
            else
                return this.Modificar(producto);
        }

        public bool Eliminar(Productos producto)
        {
            _contexto.Entry(producto).State = EntityState.Deleted;
            return _contexto.SaveChanges() > 0;
        }

        public Productos? Buscar(int ProductoId)
        {
            return _contexto.Productos.Where(p => p.ProductoId == ProductoId).AsNoTracking().SingleOrDefault();
        }

        public List<Productos> GetList(Expression<Func<Productos, bool>> criterios)
        {
            return _contexto.Productos.AsNoTracking().Where(criterios).ToList();
        }
    }
}
