namespace VentasDB
{
    internal class Venta
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public decimal Total { get; set; }

        public List<VentaConcepto> Conceptos { get; set; }
    }
}
