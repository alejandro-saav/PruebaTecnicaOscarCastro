using PruebaTecnicaOscarCastro.Models;
using PruebaTecnicaOscarCastro.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PruebaTecnicaOscarCastro.Controllers {
    public class SalasController : Controller {
        private readonly ISalaRepository _salaRepository;

        public SalasController(ISalaRepository salaRepository) {
            _salaRepository = salaRepository;
        }

        public async Task<ActionResult> Index() {
            var salas = await _salaRepository.ObtenerTodas();
            return View(salas);
        }

        [HttpGet]
        public ActionResult Crear() {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Crear(Sala sala) {
            if (ModelState.IsValid) {
                await _salaRepository.Insertar(sala);
                return RedirectToAction(nameof(Index));
            }
            return View(sala);
        }

        [HttpGet]
        public async Task<ActionResult> Editar(int id) {
            var sala = await _salaRepository.ObtenerPorId(id);
            if (sala == null) {
                return HttpNotFound();
            }
            return View(sala);
        }

        [HttpPost]
        public async Task<ActionResult> Editar(Sala sala) {
            if (ModelState.IsValid) {
                await _salaRepository.Actualizar(sala);
                return RedirectToAction(nameof(Index));
            }
            return View(sala);
        }

        [HttpPost]
        public async Task<ActionResult> Eliminar(int id) {
            await _salaRepository.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }

}