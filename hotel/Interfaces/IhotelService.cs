using hotel.Models;
using System.Diagnostics;

namespace hotel.Interfaces
{
    public interface IhotelService
    {
        Task<IEnumerable<habitaciones>> Obtener();
        Task<IEnumerable<huespedes>> ObtenerReserva();
        Task<IEnumerable<auditoria>> Obteneraud();
        Task<huespedes> InsertarDocumento(huespedes pro_doc);
        Task<bool> Eliminar(int idreserva, int idnumero);
        Task<huespedes> Actualizar(huespedes pro_doc);

    }
}
