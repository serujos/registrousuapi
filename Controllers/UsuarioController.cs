using Microsoft.AspNetCore.Mvc;
using registrousuapi.Data;
using registrousuapi.Models;
using registrousuapi.Services;
using registrousuapi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace registrousuapi.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly PaisService _paisService;

        public UsuarioController(ApplicationDbContext db, PaisService paisService)
        {
            _db = db;
            _paisService = paisService;
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            var info = await _paisService.GetInfoPaisAsync(modelo.PaisCode);
            if (info == null)
            {
                ModelState.AddModelError("PaisCode", "Código de país inválido.");
                return View(modelo);
            }

            _db.Usuarios.Add(modelo);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _db.Usuarios.AsNoTracking().ToListAsync();
            var vm = new List<UsuarioVista>();

            foreach (var u in usuarios)
            {
                var info = await _paisService.GetInfoPaisAsync(u.PaisCode);
                vm.Add(new UsuarioVista
                {
                    Nombre = u.Nombre,
                    Email = u.Email,
                    PaisOficial = info?.NombreOficial ?? "Desconocido",
                    Region = info?.Region ?? "-",
                    BanderaUrl = info?.BanderaUrl
                });
            }

            return View(vm);
        }
    }
}
