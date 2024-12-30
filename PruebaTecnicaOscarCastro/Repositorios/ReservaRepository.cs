using Dapper;
using PruebaTecnicaOscarCastro.Models;
using PruebaTecnicaOscarCastro.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PruebaTecnicaOscarCastro.Repositorios {
    public class ReservaRepository : IReservaRepository {
        private readonly IDbConnection _db;

        public ReservaRepository() {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        public async Task<IEnumerable<Reserva>> ConsultarReservas(DateTime? fechaInicio, DateTime? fechaFin, int? salaId) {
            var parameters = new DynamicParameters();
            parameters.Add("@FechaInicio", fechaInicio);
            parameters.Add("@FechaFin", fechaFin);
            parameters.Add("@SalaID", salaId);

            return await _db.QueryAsync<Reserva, Sala, Reserva>(
                "sp_ConsultarReservas",
                (reserva, sala) => {
                    reserva.Sala = sala;
                    return reserva;
                },
                parameters,
                splitOn: "SalaID",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ValidarDisponibilidad(int salaId, DateTime fechaReserva) {
            var parameters = new DynamicParameters();
            parameters.Add("@SalaID", salaId);
            parameters.Add("@FechaReserva", fechaReserva);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await _db.ExecuteAsync(
                "sp_ValidarDisponibilidadSala",
                parameters,
                commandType: CommandType.StoredProcedure);

            var result = parameters.Get<int>("@Result");
            return result == 0;
        }

        public async Task<int> Insertar(Reserva reserva) {
            var parameters = new DynamicParameters();
            parameters.Add("@SalaID", reserva.SalaID);
            parameters.Add("@FechaReserva", reserva.FechaReserva);
            parameters.Add("@Usuario", reserva.Usuario);

            return await _db.ExecuteScalarAsync<int>(
                "sp_InsertarReserva",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> Actualizar(Reserva reserva) {
            var parameters = new DynamicParameters();
            parameters.Add("@ID", reserva.ID);
            parameters.Add("@SalaID", reserva.SalaID);
            parameters.Add("@FechaReserva", reserva.FechaReserva);
            parameters.Add("@Usuario", reserva.Usuario);

            var result = await _db.ExecuteAsync(
                "sp_ActualizarReserva",
                parameters,
                commandType: CommandType.StoredProcedure);

            return result == 0;
        }

        public async Task<bool> Eliminar(int id) {
            var parameters = new DynamicParameters();
            parameters.Add("@ID", id);
            await _db.ExecuteAsync("sp_EliminarReserva", parameters, commandType: CommandType.StoredProcedure);
            return true;
        }
    }
}