using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Punto.Forms
{
    public partial class frmProductos : Form
    {
        private Conexion conexion;
        public frmProductos()
        {
            InitializeComponent();
        }

        //metodo que carga los datos en el datagridview
        private void cargarDatos()
        {
            //instanciamos y hacemos la conexion a la base de datos
            conexion = new Conexion();
            MySqlConnection con = conexion.getConection();
            if (con != null)
            {
                //se crea la consulta
                string consultaDatos = "select * from productos";
                //se crea un adapatador para los datos extraidos de la base de datos
                MySqlDataAdapter adaptador = new MySqlDataAdapter(consultaDatos, con);
                //se crea un data table
                DataTable dt = new DataTable();
                //se vacia el adaptador en el data table
                adaptador.Fill(dt);
                //se asigna el data table al datagridview
                dgvProductos.DataSource = dt;
                //ocultamos el idproducto o cualquiero otro campo que no queramos mostrar
                dgvProductos.Columns["producto_id"].Visible = false;

            }
            else
            {
                MessageBox.Show("Error inesperado");
            }
        }

        private void frmProductos_Load(object sender, System.EventArgs e)
        {
            cargarDatos();
        }

        private void dgvProductos_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //se verifica que esté seleccionado una fila
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];
                lblId.Text = fila.Cells[0].Value.ToString();
                txtCodigo.Text = fila.Cells[1].Value.ToString();
                txtNombre.Text = fila.Cells[2].Value.ToString();
                txtPrecio.Text = fila.Cells[3].Value.ToString();
                txtStock.Text = fila.Cells[4].Value.ToString();
            }
        }

        private void btnEditar_Click(object sender, System.EventArgs e)
        {
            //se rescatan los datos del formulario
            int idproducto = int.Parse(lblId.Text);
            string codigo = txtCodigo.Text;
            string nombre = txtNombre.Text;
            string precioString = txtPrecio.Text;
            string stockString = txtStock.Text;

            //verificamos que los campos no vayan vacios
            if (string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(precioString) || string.IsNullOrWhiteSpace(stockString))
            {
                MessageBox.Show("Los campos no pueden\nir vacios");
                return;
            }
            float precio;
            int stock;
            //verificamos que el precio y el stock sean numeros
            if (!float.TryParse(precioString, out precio) || !int.TryParse(stockString, out stock))
            {
                MessageBox.Show("El valor campo precio y/o stock\nno son adecuados");
                return;
            }

            //verificamos que el precio y el stock sean adecuados
            if (precio <= 0 || stock < 0)
            {
                MessageBox.Show("El precio/stock es invalido");
                return;
            }

            //MessageBox.Show($"precio: {precio} stock: {stock}");

            //se crea la conexion a la base de datos
            conexion = new Conexion();
            MySqlConnection con = conexion.getConection();

            try
            {
                //creamos la actualizacion para modificar los datos            
                string actualizacion = "update productos set codigo=@codigo, descripcion=@descripcion, precio=@precio, stock=@stock where producto_id=@producto_id";
                MySqlCommand comando = new MySqlCommand(actualizacion, con);
                comando.Parameters.AddWithValue("@codigo", codigo);
                comando.Parameters.AddWithValue("@descripcion", nombre);
                comando.Parameters.AddWithValue("@precio", precio);
                comando.Parameters.AddWithValue("@stock", stock);
                comando.Parameters.AddWithValue("@producto_id", idproducto);

                int filasAfectadas = comando.ExecuteNonQuery();
                con.Close();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Operación exitosa");
                    cargarDatos();
                }
                else {
                    MessageBox.Show("Operación fallida");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado");
            }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            //se rescatan los datos del formulario
            int idproducto = int.Parse(lblId.Text);
            //string codigo = txtCodigo.Text;
            string nombre = txtNombre.Text;
            //string precioString = txtPrecio.Text;
            //string stockString = txtStock.Text;

            //se pregunta antes de eliminar
            DialogResult respuesta = MessageBox.Show("En verdad quieres eliminar a : " + nombre + "?", "Eliminar", MessageBoxButtons.YesNo);

            if (respuesta == DialogResult.No)
                return;

            try
            {
                //se crea la conexion a la base de datos
                conexion = new Conexion();
                MySqlConnection con = conexion.getConection();

                string eliminacion = "delete from productos where producto_id=@producto_id";
                MySqlCommand comando = new MySqlCommand(eliminacion, con);
                comando.Parameters.AddWithValue("@producto_id", idproducto);

                int filasAfectadas = comando.ExecuteNonQuery();
                con.Close();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Registro eliminado exitosamente");
                    cargarDatos();
                }
                else
                {
                    MessageBox.Show("Fallo al eliminar el registro");
                }

            }
            catch(Exception ex) 
            {
                MessageBox.Show("Error inesperado");
            }

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            //se rescatan los datos del formulario
            string codigo = txtCodigoNuevo.Text;
            string nombre = txtNombreNuevo.Text;
            string precioString = txtPrecioNuevo.Text;
            string stockString = txtStockNuevo.Text;

            //verificamos que los campos no vayan vacios
            if (string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(precioString) || string.IsNullOrWhiteSpace(stockString))
            {
                MessageBox.Show("Los campos no pueden\nir vacios");
                return;
            }
            float precio;
            int stock;
            //verificamos que el precio y el stock sean numeros
            if (!float.TryParse(precioString, out precio) || !int.TryParse(stockString, out stock))
            {
                MessageBox.Show("El valor campo precio y/o stock\nno son adecuados");
                return;
            }

            //verificamos que el precio y el stock sean adecuados
            if (precio <= 0 || stock < 0)
            {
                MessageBox.Show("El precio/stock es invalido");
                return;
            }

            //se crea la conexion a la base de datos
            conexion = new Conexion();
            MySqlConnection con = conexion.getConection();

            if (con != null) {
                MessageBox.Show("Conexion exitosa");
            }

            try
            {
                string agregar = "insert into productos (codigo, descripcion, precio,stock) values (@codigo, @descripcion, @precio, @stock)";
                MySqlCommand comando = new MySqlCommand(agregar, con);
                comando.Parameters.AddWithValue("@codigo", codigo);
                comando.Parameters.AddWithValue("@descripcion", nombre);
                comando.Parameters.AddWithValue("@precio", precio);
                comando.Parameters.AddWithValue("@stock", stock);

                int filasAfectadas = comando.ExecuteNonQuery();
                con.Close();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Registro exitoso");
                    cargarDatos();
                    limpiarCampos();
                }
                else
                {
                    MessageBox.Show("Registro fallido");
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Error inesperado");
            }

        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            //se crea la conexion a la base de datos
            conexion = new Conexion();
            MySqlConnection con = conexion.getConection();

            if (con != null) {
                TextBox txt = (TextBox)sender;
                string busqueda = "select * from productos where descripcion like @busqueda";
                MySqlDataAdapter adaptador = new MySqlDataAdapter(busqueda, con);
                adaptador.SelectCommand.Parameters.AddWithValue("@busqueda", "%" + txt.Text + "%");

                DataTable dt = new DataTable();
                adaptador.Fill(dt);

                dgvProductos.DataSource = dt;
                dgvProductos.Columns["producto_id"].Visible = false;

            }
        }

        private void limpiarCampos() {
            txtCodigoNuevo.Clear();
            txtNombreNuevo.Clear();
            txtPrecioNuevo.Clear();
            txtStockNuevo.Clear();
        }
    }
}
