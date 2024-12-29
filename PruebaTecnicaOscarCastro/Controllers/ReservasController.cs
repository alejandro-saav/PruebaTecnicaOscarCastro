using System.Threading.Tasks;
using System.Web.Mvc;
using System;
using PruebaTecnicaOscarCastro.Repositorios.Interfaces;
using PruebaTecnicaOscarCastro.Models;
using System.Linq;

public class ReservasController : Controller {
    private readonly IReservaRepository _reservaRepository;
    private readonly ISalaRepository _salaRepository;

    public ReservasController(IReservaRepository reservaRepository, ISalaRepository salaRepository) {
        _reservaRepository = reservaRepository;
        _salaRepository = salaRepository;
    }

    public async Task<ActionResult> Index() {
        var reservas = await _reservaRepository.ConsultarReservas(null, null, null);
        return View(reservas);
    }

    [HttpGet]
    public async Task<ActionResult> Crear() {
        var salas = await _salaRepository.ObtenerTodas();
        ViewBag.Salas = new SelectList(salas, "ID", "Nombre");
        return View(new Reserva { FechaReserva = DateTime.Today });
    }

    [HttpPost]
    public async Task<ActionResult> Crear(Reserva reserva) {
        if (ModelState.IsValid) {
            var disponible = await _reservaRepository.ValidarDisponibilidad(reserva.SalaID, reserva.FechaReserva);
            if (!disponible) {
                ModelState.AddModelError("", "La sala no está disponible para la fecha seleccionada");
                var salas = await _salaRepository.ObtenerTodas();
                ViewBag.Salas = new SelectList(salas, "ID", "Nombre");
                return View(reserva);
            }

            await _reservaRepository.Insertar(reserva);
            return RedirectToAction(nameof(Index));
        }
        return View(reserva);
    }

    [HttpGet]
    public async Task<ActionResult> Editar(int id) {
        var reservas = await _reservaRepository.ConsultarReservas(null, null, null);
        var reserva = reservas.FirstOrDefault(r => r.ID == id);
        if (reserva == null) {
            return HttpNotFound();
        }

        var salas = await _salaRepository.ObtenerTodas();
        ViewBag.Salas = new SelectList(salas, "ID", "Nombre");
        return View(reserva);
    }

    [HttpPost]
    public async Task<ActionResult> Editar(Reserva reserva) {
        if (ModelState.IsValid) {
            await _reservaRepository.Actualizar(reserva);
            return RedirectToAction(nameof(Index));
        }
        return View(reserva);
    }

    [HttpPost]
    public async Task<JsonResult> ValidarDisponibilidad(int salaId, DateTime fecha) {
        var disponible = await _reservaRepository.ValidarDisponibilidad(salaId, fecha);
        return Json(new { disponible });
    }

    [HttpPost]
    public async Task<ActionResult> Eliminar(int id) {
        await _reservaRepository.Eliminar(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<ActionResult> Consultar(DateTime? fechaInicio, DateTime? fechaFin, int? salaId) {
        var reservas = await _reservaRepository.ConsultarReservas(fechaInicio, fechaFin, salaId);
        return PartialView("_ListaReservas", reservas);
    }
}