using PruebaTecnicaOscarCastro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaOscarCastro.Repositorios.Interfaces {
    public interface ISalaRepository {
        Task<IEnumerable<Sala>> ObtenerTodas();
        Task<Sala> ObtenerPorId(int id);
        Task<int> Insertar(Sala sala);
        Task<bool> Actualizar(Sala sala);
        Task<bool> Eliminar(int id);
    }
}
