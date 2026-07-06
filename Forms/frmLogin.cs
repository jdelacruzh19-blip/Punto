using System.Windows.Forms;
using MySql.Data.MySqlClient;
using BCrypt.Net;

namespace Punto.Forms
{
    public partial class frmLogin : Form
    {
        frmPrincipal principal;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            string usuario = txtUser.Text;
            string contrasenia = txtPassword.Text;

            //validacion de campos vacios
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasenia))
            {
                MessageBox.Show("El usuario o la contraseña no pueden \n ir vacios");
                return;
            }

            //se hace la conexion con la base de datos
            Conexion conexion = new Conexion();
            MySqlConnection conn = conexion.getConection();

            //probar la conexion
            /*if (conn != null)
            {
                MessageBox.Show("conexion exitosa");
            }
            else
            {
                MessageBox.Show("Error de coneccion");
            }*/


            try
            {
                string consultaUsuario = "select * from usuarios where username=@nombre";
                //se crea un comando para ejecutar la consulta usuario
                MySqlCommand comando = new MySqlCommand(consultaUsuario, conn);
                //se asigna el usuario al parametro
                comando.Parameters.AddWithValue("@nombre", usuario);
                //se ejecuta la consulta
                MySqlDataReader reader = comando.ExecuteReader();

                
                if (reader.Read())
                {
                    //entra cuando el usuario coindice
                    string passwordDBHash = BCrypt.Net.BCrypt.HashPassword(reader["PASSWORD"].ToString());
                    bool verifica = BCrypt.Net.BCrypt.Verify(contrasenia, passwordDBHash);
                    if (verifica)
                    {
                        //si usuario es correcto y la contraseña es correcta
                        MessageBox.Show("Acceso correcto");
                        this.Hide();
                        principal = new frmPrincipal();
                        principal.Show();
                    }
                    else
                    {
                        //cuando el usuario es correcto pero la contraseña es incorrecta
                        conn.Close();
                        MessageBox.Show("Credenciales incorrectas\nIntente de nuevo");
                        txtPassword.Clear();
                        txtUser.Focus();
                    }
                }
                else
                {
                    //ejecuta cuando el usuario no coincide
                    MessageBox.Show("Credenciales incorrectas\nIntente de nuevo");
                    txtUser.Clear();
                    txtPassword.Clear();
                    txtUser.Focus();
                }
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
                return;
            }

        }

        private void frmLogin_Load(object sender, System.EventArgs e)
        {

        }
    }
}
