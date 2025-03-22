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
            venta.Cliente = "Cliente 1";
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
                    string query = "insert into Clientes " +
                        "(nombre,telefono,email)" +
                        " values " +
                        "(@nombre,@telefono,@email)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);

                        con.Open();
                        int registrosAfectados = cmd.ExecuteNonQuery();

                        if (registrosAfectados == 0)
                        {
                            throw new Exception("No se pudo agregar");
                        }

                    }
                }

                MessageBox.Show("Venta agregada correctamente");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
