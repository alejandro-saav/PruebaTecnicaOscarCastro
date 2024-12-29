using PruebaTecnicaOscarCastro.Models;
using PruebaTecnicaOscarCastro.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;

namespace PruebaTecnicaOscarCastro.Repositorios {
    public class SalaRepository : ISalaRepository {
        private readonly IDbConnection _db;

        public SalaRepository() {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        public async Task<IEnumerable<Sala>> ObtenerTodas() {
            return await _db.QueryAsync<Sala>("sp_ObtenerSalas", commandType: CommandType.StoredProcedure);
        }

        public async Task<Sala> ObtenerPorId(int id) {
            var parameters = new DynamicParameters();
            parameters.Add("@ID", id);
            return await _db.QueryFirstOrDefaultAsync<Sala>(
                "SELECT ID, Nombre, Capacidad, Disponibilidad FROM Salas WHERE ID = @ID",
                parameters);
        }

        public async Task<int> Insertar(Sala sala) {
            var parameters = new DynamicParameters();
            parameters.Add("@Nombre", sala.Nombre);
            parameters.Add("@Capacidad", sala.Capacidad);
            parameters.Add("@Disponibilidad", sala.Disponibilidad);

            return await _db.ExecuteScalarAsync<int>(
                "sp_InsertarSala",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> Actualizar(Sala sala) {
            var parameters = new DynamicParameters();
            parameters.Add("@ID", sala.ID);
            parameters.Add("@Nombre", sala.Nombre);
            parameters.Add("@Capacidad", sala.Capacidad);
            parameters.Add("@Disponibilidad", sala.Disponibilidad);

            await _db.ExecuteAsync("sp_ActualizarSala", parameters, commandType: CommandType.StoredProcedure);
            return true;
        }

        public async Task<bool> Eliminar(int id) {
            var parameters = new DynamicParameters();
            parameters.Add("@ID", id);
            var result = await _db.ExecuteAsync("sp_EliminarSala", parameters, commandType: CommandType.StoredProcedure);
            return result == 0;
        }
    }
}