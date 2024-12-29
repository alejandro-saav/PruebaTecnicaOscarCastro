using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaTecnicaOscarCastro.Models {
    public class Reserva {
        public int ID { get; set; }
        public int SalaID { get; set; }
        public DateTime FechaReserva { get; set; }
        public string Usuario { get; set; }
        public Sala Sala { get; set; }
    }
}