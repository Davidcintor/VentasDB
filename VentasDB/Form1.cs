using System.Data;
using Microsoft.Data.SqlClient;

namespace VentasDB
{
    public partial class Form1 : Form
    {
        private string _connectionString;
        public Form1()
        {
            InitializeComponent();
            _connectionString = "Server=localhost;Database=VentasDB;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        private Venta ObtenerVentaDemo()
        {
            Venta venta = new Venta();
            venta.Cliente = "Estrella";
            venta.Total = 2000;
            venta.Conceptos = new List<VentaConcepto>
            {
                new VentaConcepto
                {
                    Cantidad = 2,
                    Descripcion = "Producto 1",
                    PrecioUnitario = 500,
                    Importe = 1000
                },
                new VentaConcepto
                {
                    Cantidad = 1,
                    Descripcion = "Producto 2",
                    PrecioUnitario = 1000,
                    Importe = 1000
                }
            };
            return venta;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Venta venta = ObtenerVentaDemo();

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO Ventas" +
                                  " (Cliente, Total)" +
                                  " VALUES" +
                                  " (@Cliente, @Total);" +
                                  " SELECT SCOPE_IDENTITY()";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("@Cliente", venta.Cliente);
                        cmd.Parameters.AddWithValue("@Total", venta.Total);

                        con.Open();

                        if (!int.TryParse(cmd.ExecuteScalar().ToString(), out int ventaId))
                        {
                            throw new Exception("No se pudo agregar");
                        }
                        venta.Id = ventaId;
                    }

                    //Aqui empezamos a guardar los conceptos
                    query = "INSERT INTO VentasConceptos" +
                            " (VentaId, Cantidad, Descripcion, PrecioUnitario, Importe)" +
                            " VALUES" +
                            " (@VentaId, @Cantidad, @Descripcion, @PrecioUnitario, @Importe)";

                    foreach (VentaConcepto concepto in venta.Conceptos)
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@VentaId", venta.Id);
                            cmd.Parameters.AddWithValue("@Cantidad", concepto.Cantidad);
                            cmd.Parameters.AddWithValue("@Descripcion", concepto.Descripcion);
                            cmd.Parameters.AddWithValue("@PrecioUnitario", concepto.PrecioUnitario);
                            cmd.Parameters.AddWithValue("@Importe", concepto.Importe);

                            int registrosAfectados = cmd.ExecuteNonQuery();

                            if (registrosAfectados == 0)
                            {
                                throw new Exception("No se pudo agregar concepto ");
                            }

                        }

                        throw new Exception();
                    }

                    MessageBox.Show("Venta agregada correctamente" + venta.Id);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
