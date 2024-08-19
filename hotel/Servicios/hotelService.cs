using hotel.Interfaces;
using hotel.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace hotel.Servicios
{
    public class hotelService : IhotelService
    {
        private readonly string CadenaSQL;

        public hotelService(IConfiguration configuration)
        {
            CadenaSQL = configuration.GetConnectionString("conexion");
        }

        public async Task<IEnumerable<habitaciones>> Obtener()
        {
            using var conection = new SqlConnection(CadenaSQL);
            return await conection.QueryAsync<habitaciones>(@"SELECT id,numero FROM habitaciones where estado = 0");
        }

        public async Task<IEnumerable<huespedes>> ObtenerReserva()
        {
            using var conection = new SqlConnection(CadenaSQL);
            return await conection.QueryAsync<huespedes>(@"select a.id, nombre, identificacion, b.numero, ingreso, salida,a.estado from huespedes  a
left join habitaciones b on a.habitacion_id = b.id where a.estado in (0,2)");
        }

        public async Task<IEnumerable<auditoria>> Obteneraud()
        {
            using var conection = new SqlConnection(CadenaSQL);
            return await conection.QueryAsync<auditoria>(@"
select TableName as tabla,Operation as operacion ,RecordID as id,OldValues as valorant,NewValues as valornew,UserName as usuario,Timestamp as fecha from AuditLog
");
        }

        public async Task<huespedes> InsertarDocumento(huespedes pro_doc)
        {
            using var conection = new SqlConnection(CadenaSQL);
            var existe = await conection.QueryFirstOrDefaultAsync<int>(@$"select 1 from HUESPEDES
                where identificacion = @identificacion AND estado = 0;"
                 , pro_doc);
            if (existe != 0)
            {
                throw new TaskCanceledException("Usuario ya tiene una habitacion");
            }

            var docCreado = await conection.QuerySingleAsync<int>
           (@"INSERT INTO HUESPEDES (nombre, identificacion, habitacion_id, ingreso, salida, estado) 
            VALUES (@nombre, @identificacion, @numero, @ingreso, @salida, 0);SELECT SCOPE_IDENTITY();", pro_doc);
            try
            {
                if (docCreado == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el registro");
                }
                var update = await conection.ExecuteAsync
                      (@"update habitaciones set estado = 1 where id = @numero", pro_doc);
            }
            catch
            {
                throw;
            }
            
            pro_doc.Id = docCreado;
            return pro_doc;
        }
        public async Task<bool> Eliminar(int idreserva, int idnumero)
        {
            using var conection = new SqlConnection(CadenaSQL);
            var update1 = await conection.ExecuteAsync
                (@"update huespedes set estado =1 where id =@idreserva ;", new { idreserva});
            var update2 = await conection.ExecuteAsync
                (@"update habitaciones set estado = 0 where numero = @idnumero;", new { idnumero });
            try
            {
                if (update1 == 0)
                {
                    throw new TaskCanceledException("No se pudo eliminar el documento");
                }
            }
            catch
            {
                throw;
            }
            return true;
        }
        public async Task<huespedes> Actualizar(huespedes pro_doc)
        {
            using var conection = new SqlConnection(CadenaSQL);
            var docCreado = await conection.ExecuteAsync(@"update huespedes set salida = @salida, 
                     estado = 2 where id = @id; SELECT SCOPE_IDENTITY();", pro_doc);
            var update2 = await conection.ExecuteAsync
                (@"update habitaciones set estado = 0 where numero = @numero;", pro_doc);
            try
            {
                if (docCreado == 0)
                {
                    throw new TaskCanceledException("No se pudo modificar el documento");
                }
            }
            catch
            {
                throw;
            }
            return pro_doc;
        }
    }
}
