namespace hotel.Models
{
    public class huespedes
    {
        public int Id { get; set; }
        public string? Nombre { get; set; } = null!;
        public string? Identificacion { get; set; } = null!;
        public int numero { get; set; }
        public DateTime? Ingreso { get; set; }
        public DateTime? Salida { get; set; }
        public int? estado { get; set; }
    }
}
