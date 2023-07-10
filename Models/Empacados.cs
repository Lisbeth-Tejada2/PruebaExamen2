using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PruebaExamen2.Models
{
    public class Empacados
    {
        [Key]
        public int EmpacadoId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Today;
        public string Concepto { get; set; } = string.Empty;
        [ForeignKey("EmpacadoId")]
        public virtual List<EmpacadosDetalle> EmpacadosDetalle { get; set; } = new List<EmpacadosDetalle>();
        public int Cantidad { get; set; }
        public int ProductoId { get; set; }
    }
}
