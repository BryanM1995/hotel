

namespace hotel.Models
{
    public class auditoria
    {
        //public int AuditID { get; set; }
        public string? tabla { get; set; } = null!;
        public string? operacion { get; set; } = null!;
        public int id { get; set; }
        public string? valorant { get; set; } = null!;
        public string? valornew { get; set; } = null!;
        public string? usuario { get; set; } = null!;
        public DateTime? fecha { get; set; }

    }
}
