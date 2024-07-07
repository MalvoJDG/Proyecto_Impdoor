using Bunifu.Framework.UI;
using Capa_N.EntityProve;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Capa_P
{
    public partial class CrearCuenta : Form
    {

        public CrearCuentas cc = new CrearCuentas();
        public CrearCuenta()
        {
            InitializeComponent();
            bunifuCheckBox1.Checked = false;
            bunifuDropdown1.Items.Add("Administrador");
            bunifuDropdown1.Items.Add("Usuario");
        }

        


        private void CrearCuentas() 
        {
            String msj = "";
            try
            {
                
                cc.Nombre = txtName.Text;
                cc.UserName = txtUserName.Text;
                cc.Contraseña = txtContraseña.Text;
                cc.ConContraseña = txtConContraseña.Text;
               
                






                if (!string.IsNullOrEmpty(cc.Nombre) && !string.IsNullOrEmpty(cc.Contraseña) && !string.IsNullOrEmpty(cc.ConContraseña) && cc.RoleId <= 0 && cc.RoleId >= 3) 
                {
                    if (cc.Contraseña == cc.ConContraseña)
                    {
                      
                            msj = cc.RegistrarCuentas();
                            MessageBox.Show(msj);

                        
                       
                    }
                    else 
                    {
                        MessageBox.Show("El Campo Contraseña y ConfirmarContraseña deben Coincidir");
                    }
                }
                else
                {
                    MessageBox.Show("Termine de completar los campos Requeridos");
                }



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        
        }

        private void txtName_TextChanged(object sender, EventArgs e)
       {

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtContraseña_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtConContraseña_TextChanged(object sender, EventArgs e)
        {

            

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            CrearCuentas();

            
        }

        private void bunifuCheckBox1_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if (bunifuCheckBox1.Checked == false)
            {
                txtContraseña.PasswordChar = '*';
                txtConContraseña.PasswordChar = '*';    
            }
            else if (bunifuCheckBox1.Checked == true)
            {
                txtContraseña.PasswordChar = '\0';
                txtConContraseña.PasswordChar = '\0';
            }
        }

        private void bunifuDropdown1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRole = bunifuDropdown1.SelectedItem.ToString();

            if (selectedRole == "Administrador")
            {
                cc.RoleId = 1;
            }
            else if (selectedRole == "Usuario")
            {
                cc.RoleId = 2;
            }

            int numeroComoInt = int.Parse(cc.RoleId);
        }


        
    }
   
}
