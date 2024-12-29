using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaTecnicaOscarCastro.Models {
    public class Sala {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int Capacidad { get; set; }
        public bool Disponibilidad { get; set; }
    }
}