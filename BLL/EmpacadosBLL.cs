using Microsoft.EntityFrameworkCore;
using PruebaExamen2.DAL;
using PruebaExamen2.Models;
using System.Linq.Expressions;


namespace PruebaExamen2.BLL
{
    public class EmpacadosBLL
    {
        private Contexto _contexto;

        public EmpacadosBLL(Contexto _contexto)
        {
            this._contexto = _contexto;
        }
        public bool Existe(int EmpacadoId)
        {
            return _contexto.Empacados.Any(emp => emp.EmpacadoId == EmpacadoId);
        }
        public bool Insertar(Empacados empacado)
        {
            var paso = false;
            try
            {
                Productos? producto;
                foreach (var detalle in empacado.EmpacadosDetalle)
                {
                    producto = _contexto.Productos.SingleOrDefault(p => p.ProductoId == detalle.ProductoId);
                    if (producto != null)
                    {
                        producto.Existencia -= detalle.Cantidad;
                        _contexto.Entry(producto).State = EntityState.Modified;
                        _contexto.Entry(detalle).State = EntityState.Added;
                    }
                }
                var producido = _contexto.Productos.SingleOrDefault(p => p.ProductoId == empacado.ProductoId);
                if (producido != null)
                {
                    producido.Existencia += empacado.Cantidad;
                    _contexto.Entry(producido).State = EntityState.Modified;
                }
                _contexto.Entry(empacado).State = EntityState.Added;
                paso = _contexto.SaveChanges() > 0;
                _contexto.Entry(empacado).State = EntityState.Detached;
            }
            catch (Exception)
            {
                return false;
            }
            return paso;
        }

        public bool Modificar(Empacados empacado)
        {
            var paso = false;
            try
            {
                var EmpacadoAntiguo = Buscar(empacado.EmpacadoId);
                Productos? producto;
                if (EmpacadoAntiguo != null)
                {
                    foreach (var detalle in EmpacadoAntiguo.EmpacadosDetalle)
                    {
                        producto = _contexto.Productos.Find(detalle.DetalleId);

                        if (producto != null)
                            producto.Existencia += detalle.Cantidad;
                    }
                    var producidoAntiguo = _contexto.Productos.Find(EmpacadoAntiguo.ProductoId);
                    if (producidoAntiguo != null)
                    {
                        producidoAntiguo.Existencia -= EmpacadoAntiguo.Cantidad;
                        _contexto.Entry(producidoAntiguo).State = EntityState.Modified;
                    }
                }
                _contexto.Database.ExecuteSqlRaw($"DELETE FROM EmpacadosDetalle WHERE EmpacadoId = {empacado.EmpacadoId}");
                foreach (var New in empacado.EmpacadosDetalle)
                {
                    producto = _contexto.Productos.Find(New.DetalleId);
                    if (producto != null)
                        producto.Existencia -= New.Cantidad;
                    _contexto.Entry(New).State = EntityState.Added;
                }
                var ProducidoNuevo = _contexto.Productos.Find(empacado.ProductoId);
                if (ProducidoNuevo != null)
                {
                    ProducidoNuevo.Existencia += empacado.Cantidad;
                    _contexto.Entry(ProducidoNuevo).State = EntityState.Modified;
                }
                _contexto.Entry(empacado).State = EntityState.Modified;
                paso = _contexto.SaveChanges() > 0;
                _contexto.Entry(empacado).State = EntityState.Detached;

            }
            catch (Exception)
            {
                return false;
            }
            return paso;
        }
        public bool Guardar(Empacados empacado)
        {
            try
            {
                if (!Existe(empacado.EmpacadoId))
                    return Insertar(empacado);
                else
                    return Modificar(empacado);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Eliminar(Empacados empacado)
        {
            var paso = false;
            try
            {
                Productos? producto;
                foreach (var detalle in empacado.EmpacadosDetalle)
                {
                    producto = _contexto.Productos.SingleOrDefault(p => p.ProductoId == detalle.ProductoId);
                    if (producto != null)
                    {
                        producto.Existencia += detalle.Cantidad;
                        _contexto.Entry(producto).State = EntityState.Modified;
                    }
                }
                var producido = _contexto.Productos.SingleOrDefault(p => p.ProductoId == empacado.ProductoId);
                if (producido != null)
                {
                    producido.Existencia -= empacado.Cantidad;
                    _contexto.Entry(producido).State = EntityState.Modified;
                }
                _contexto.Entry(empacado).State = EntityState.Deleted;
                paso = _contexto.SaveChanges() > 0;
                _contexto.Entry(empacado).State = EntityState.Detached;

            }
            catch (Exception)
            {
                return false;
            }
            return paso;
        }
        public Empacados? Buscar(int EmpacadoId)
        {
            return _contexto.Empacados.Include(e => e.EmpacadosDetalle).Where(emp => emp.EmpacadoId == EmpacadoId).AsNoTracking().SingleOrDefault();
        }
        public List<Empacados> GetList(Expression<Func<Empacados, bool>> criterio)
        {
            return _contexto.Empacados.Include(e => e.EmpacadosDetalle).AsNoTracking().Where(criterio).ToList();
        }
    }
}
