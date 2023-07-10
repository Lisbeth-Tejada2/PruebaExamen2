using Microsoft.EntityFrameworkCore;
using PruebaExamen2.Models;

namespace PruebaExamen2.DAL
{
    public class Contexto : DbContext
    {
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Empacados> Empacados { get; set; }

        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Productos>().HasData(
                new Productos
                {
                    ProductoId = 1,
                    Costo = 30,
                    Precio = 50,
                    Existencia = 30,
                    Descripcion = "Maní"
                },

                new Productos
                {
                    ProductoId = 2,
                    Costo = 20,
                    Precio = 40,
                    Existencia = 25,
                    Descripcion = "Pasas"
                },

                new Productos
                {
                    ProductoId = 3,
                    Costo = 50,
                    Precio = 70,
                    Existencia = 30,
                    Descripcion = "Ciruela"
                },

                new Productos
                {
                    ProductoId = 4,
                    Costo = 65,
                    Precio = 90,
                    Existencia = 20,
                    Descripcion = "Arandanos"
                }

            );
        }
    }
}
