using Microsoft.AspNetCore.Mvc;
using ProyectoASll.Models;
using ProyectoASll.Repositorio.IRepositorio;
using System.Diagnostics;
using System.Security.Claims;
using ProyectoASll.ViewModels;
namespace ProyectoASll.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //generar una instancia a la unidad de trabajo
        private readonly IUnidadTrabajo _unidadTrabajo;

        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            /*
            // Obtener el ID del empleado logueado
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Asegurarse de que el usuario est� logueado
            if (userId == null)
            {
                return Unauthorized(); // Manejar el caso en que el usuario no est� logueado
            }

            
            // Obtener la URL de la imagen del empleado
            var imagenUrl = await _unidadTrabajo.EmpleadoRepositorio.ObtenerImagenUrlEmpleado(userId);
            var empleadoVM = new EmpleadoVM
            {
                ImagenUrl = imagenUrl
            };
            // Pasar la URL de la imagen a la vista
            ViewBag.ImagenUrl = imagenUrl;*/
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Usuario no autenticado" });
            }

            var empleado = await _unidadTrabajo.EmpleadoRepositorio.ObtenerEmpleado(userId);
            if (empleado == null)
            {
                return NotFound(new { message = "Empleado no encontrado" });
            }
            var empleadoViewModel = new EmpleadoVM
            {
                Nombre = empleado.Nombre,
                Rol = empleado.Rol,
                Telefono = empleado.Numero,
                Email = empleado.Email,
                ImagenUrl = empleado.ImagenURL ?? "/img/arle.jpg"
            };
            return View(empleadoViewModel); 
        }

        public IActionResult Catalogo()
        {           
            return View(); 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region API


        [HttpGet]
        public async Task<IActionResult> obtenerimg()
        {
            // Obtener el ID del empleado logueado
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Asegurarse de que el usuario est� logueado
            if (userId == null)
            {
                return Unauthorized(); // Manejar el caso en que el usuario no est� logueado
            }
            //con esto nos devolvera los campos de categoria y marca para incorporarlos a la vista producto
            var imagenUrl = await _unidadTrabajo.EmpleadoRepositorio.ObtenerImagenUrlEmpleado(userId);
            return Json(new { data = imagenUrl });
        }

        #endregion
    }
}
