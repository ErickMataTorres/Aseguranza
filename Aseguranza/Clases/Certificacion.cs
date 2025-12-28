using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aseguranza.Clases
{
    public class Certificacion
    {
        public int Id { get; set; }
        public int IdTrabajador { get; set; }
        public int IdProceso { get; set; }
        public DateTime FechaCertificacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int IdCertificador { get; set; }
        public string? Comentario { get; set; }
    }
}
