using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FrmTableName
{
    public partial class Form1 : Form
    {
        // Cadena de conexión a la base de datos
        private SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=instpubs;Integrated Security=True;");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        // Método para cargar los datos en el DataGridView
        private void CargarDatos()
        {
            try
            {
                connection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM TableName ORDER BY Id", connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        // Método para guardar nuevos datos
        private void BotonGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarEntrada())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO TableName (Column1, Column2) VALUES (@val1, @val2)", connection))
                    {
                        cmd.Parameters.AddWithValue("@val1", textBox1.Text);
                        cmd.Parameters.AddWithValue("@val2", textBox2.Text);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Datos guardados correctamente.");
                    CargarDatos(); // Recargar datos
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar los datos: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // Método para eliminar datos
        private void BotonEliminar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxId.Text))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM TableName WHERE Id = @id", connection))
                    {
                        cmd.Parameters.AddWithValue("@id", textBoxId.Text);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Datos eliminados correctamente.");
                    CargarDatos(); // Recargar datos
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar los datos: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido para eliminar.");
            }
        }

        // Método para verificar la entrada de datos
        private bool ValidarEntrada()
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return false;
            }
            return true;
        }

        // Método para limpiar los TextBox
        private void LimpiarCampos()
        {
            textBoxId.Clear();
            textBox1.Clear();
            textBox2.Clear();
        }

        // Método para manejar el evento de clic en una celda
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBoxId.Text = row.Cells["Id"].Value.ToString();
                textBox1.Text = row.Cells["Column1"].Value.ToString();
                textBox2.Text = row.Cells["Column2"].Value.ToString();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
