using System;
using System.Windows.Forms;
using Capa_N.EntityProve;

namespace Capa_P
{
    public partial class Login : Form
    {
        private CrearCuentas crearCuentas;

        public Login()
        {
            InitializeComponent();
            crearCuentas = new CrearCuentas(); // Crear una instancia de la clase CrearCuentas
        }
        private void bunifuTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            string nombreUsuario = bunifuTextBox1.Text.Trim();
            string contraseña = bunifuTextBox2.Text;

            if (!string.IsNullOrEmpty(nombreUsuario) && !string.IsNullOrEmpty(contraseña))
            {
                try
                {
                    string mensaje = crearCuentas.IniciarSesion(nombreUsuario, contraseña);

                    if (mensaje == "Inicio de sesión exitoso.")
                    {
                        // Mostrar un mensaje de éxito
                        MessageBox.Show("¡Inicio de sesión exitoso!");

                        // Ocultar el formulario actual
                        this.Hide();

                        Layout frm = new Layout();
                        frm.Show();
                        frm.FormClosed += (s, args) => this.Close();
                        this.Hide();

                    }
                    else
                    {
                        MessageBox.Show("Credenciales incorrectas. Por favor, intente de nuevo.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un nombre de usuario y una contraseña.");
            }
        }
    }
}



