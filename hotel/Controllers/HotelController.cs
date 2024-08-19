using Microsoft.AspNetCore.Mvc;
using hotel.Models;
using hotel.Interfaces;


namespace hotel.Controllers
{
    public class HotelController : Controller
    {
        private readonly IhotelService _hotelService;

        public HotelController(IhotelService hotelService)
        {
            _hotelService = hotelService;
            
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var resultado = await _hotelService.Obtener();
            return StatusCode(StatusCodes.Status200OK, new { data = resultado });
        }

        [HttpGet]
        public async Task<IActionResult> ListaDisponibles()
        {
            var resultado = await _hotelService.ObtenerReserva();
            return StatusCode(StatusCodes.Status200OK, new { data = resultado });
        }

        [HttpGet]
        public async Task<IActionResult> ListaDisponibles2()
        {
            var resultado = await _hotelService.Obteneraud();
            return StatusCode(StatusCodes.Status200OK, new { data = resultado });
        }


        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] huespedes modelo)
        {
            GenericResponse<huespedes> gResponse = new GenericResponse<huespedes>();
            try
            {
                huespedes documento_creado = await _hotelService.InsertarDocumento(modelo);
                if (documento_creado.Id == 0)
                    throw new TaskCanceledException("No se pudo crear el documento");
                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idreserva,int idnumero)
        {
            GenericResponse<huespedes> gResponse = new GenericResponse<huespedes>();
            try
            {
                var resudes = await _hotelService.Eliminar(idreserva, idnumero);
                if (resudes == false)
                {
                    return Json($"El documento no se pudo eliminar");
                }
                gResponse.Estado = true;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] huespedes modelo)
        {
            GenericResponse<huespedes> gResponse = new GenericResponse<huespedes>();
            try
            {

                var resudes = await _hotelService.Actualizar(modelo);
                if (resudes is null)
                {
                    return Json($"El documento no se pudo actualizar");
                }
                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}
