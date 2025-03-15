namespace VentasDB
{
    internal class VentaConcepto
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Importe { get; set; }
    }
}
