using PruebaTecnicaOscarCastro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaOscarCastro.Repositorios.Interfaces {
    public interface IReservaRepository {
        Task<IEnumerable<Reserva>> ConsultarReservas(DateTime? fechaInicio, DateTime? fechaFin, int? salaId);
        Task<bool> ValidarDisponibilidad(int salaId, DateTime fechaReserva);
        Task<int> Insertar(Reserva reserva);
        Task<bool> Actualizar(Reserva reserva);
        Task<bool> Eliminar(int id);
    }
}
