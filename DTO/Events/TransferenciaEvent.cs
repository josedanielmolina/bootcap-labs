
namespace DTO.Events
{
    public class TransferenciaEvent
    {
        public int CuentaRemitente { get; set; }
        public int CuentaDestino { get; set; }
        public decimal Monto { get; set; }
    }
}
