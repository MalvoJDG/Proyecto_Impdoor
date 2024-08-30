using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;
using Capa_N.EntityProve;
using Newtonsoft.Json.Linq;

namespace Capa_P
{
    public partial class Login : Form
    {
        private CrearCuentas crearCuentas;

        public Login()
        {
            InitializeComponent();
            crearCuentas = new CrearCuentas(); // Crear una instancia de la clase CrearCuentas
            CheckAndUpdateApp();
        }


        private void CheckAndUpdateApp()
        {
            WebClient webClient = new WebClient();
 
            try
            {
                string latestVersionUrl = "https://api.github.com/repos/MalvoJDG/Proyecto_Impdoor/releases/latest";
                webClient.Headers.Add("User-Agent", "request");

                string json = webClient.DownloadString(latestVersionUrl);
                dynamic release = JObject.Parse(json);

                string latestVersion = release.tag_name;
                Version currentVersion = new Version("1.0.2"); // Versión actual de tu aplicación

                if (new Version(latestVersion) > currentVersion)
                {
                    if (MessageBox.Show("¡Nueva actualización disponible! ¿Desea actualizar?", "Actualización", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        string downloadUrl = release.assets[0].browser_download_url; // URL de descarga del archivo

                        string tempPath = Path.GetTempPath();
                        string zipPath = Path.Combine(tempPath, "ImpdoorSetup.zip");
                        string extractPath = Path.Combine(tempPath, "ImpdoorSetup");

                        // Eliminar archivos temporales anteriores si existen
                        if (File.Exists(zipPath))
                            File.Delete(zipPath);

                        if (Directory.Exists(extractPath))
                            Directory.Delete(extractPath, true);

                        // Descargar el archivo de actualización
                        webClient.DownloadFile(downloadUrl, zipPath);

                        // Extraer el archivo ZIP
                        ZipFile.ExtractToDirectory(zipPath, extractPath);

                        // Obtener el nombre del archivo EXE
                        string[] exeFiles = Directory.GetFiles(extractPath, "*.exe");
                        if (exeFiles.Length > 0)
                        {
                            string exePath = exeFiles[0];

                            // Ejecutar el instalador
                            Process.Start(exePath);

                            // Cerrar la aplicación actual
                            Application.Exit();
                        }
                        else
                        {
                            MessageBox.Show("El archivo EXE no se encontró en la carpeta de extracción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar/actualizar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                webClient.Dispose();
            }
        }


        private bool IsNewerVersion(string latestVersion, string currentVersion)
        {
            // Convertir las versiones en arrays de números enteros
            int[] latestNumbers = Array.ConvertAll(latestVersion.Split('.'), int.Parse);
            int[] currentNumbers = Array.ConvertAll(currentVersion.Split('.'), int.Parse);

            // Comparar versión por versión
            for (int i = 0; i < Math.Min(latestNumbers.Length, currentNumbers.Length); i++)
            {
                if (latestNumbers[i] > currentNumbers[i])
                {
                    return true; // La última versión es más nueva
                }
                else if (latestNumbers[i] < currentNumbers[i])
                {
                    return false; // La última versión es más antigua
                }
                // Si son iguales, continuar comparando el siguiente nivel de versión
            }

            // Si todas las partes de la versión son iguales hasta el mínimo de las longitudes
            return latestNumbers.Length > currentNumbers.Length; // Si hay más partes en la última versión, es más nueva
        }


        private void bunifuTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnincio_Click(object sender, EventArgs e)
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



