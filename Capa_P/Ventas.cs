using Capa_N.Entity;
using Capa_N.EntityProv;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;


namespace Capa_P
{

    public partial class Ventas : Form
    {

        Productos productos = new Productos();
        Materiales materiales = new Materiales();
        Tipo_Madera tipo_Madera = new Tipo_Madera();
        FacturaHeader facturaH = new FacturaHeader();
        FacturaDetalle facturaD = new FacturaDetalle();
        Cliente cl = new Cliente();
        ncf ncf = new ncf();

        int id_Madera = 0;
        int id_Material = 0;
        int id_producto = 0;
        int cantidad = 0;
        float preciou = 0;
        int pageNumber = 1;


        public Ventas()
        {
            InitializeComponent();

            CargarProductos();
            CargarMateriales();
            CargarMadera();
            SecuenciaFiscal();
            lblNCF.Visible = false;
        }

        private void Ventas_Load(object sender, EventArgs e)
        {
            cbmImpuesto.Text = "Si";
            cbmNCF.Text = "No";
            this.Focus();
        }

        public void CargarProductos()
        {
            System.Data.DataTable dt = productos.ListadoProductos();
            cbmProducto.DataSource = dt;
            cbmProducto.DisplayMember = "Nombre";
        }

        public void CargarMateriales()
        {
            System.Data.DataTable dt = materiales.ListadoMateriales();
            cbmMaterial.DataSource = dt;
            cbmMaterial.DisplayMember = "Nombre";
        }

        public void CargarMadera()
        {
            System.Data.DataTable dt = tipo_Madera.ListadoMadera();
            cbmMadera.DataSource = dt;
            cbmMadera.DisplayMember = "Nombre";
        }

        private string textoAnterior = string.Empty;

        // Variables para guardar las posiciones originales (declarar en la clase)
        private bool posicionesIntercambiadascbm = false;

        // Bandera para indicar si estamos en el caso "Closet"
        bool esCloset = false;

        private void cbmProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Guardar el texto actual de txtServicio antes de cualquier cambio
            if (cbmProducto.Text != "Instalacion" && txtServicio.Text != "Instalacion")
            {
                textoAnterior = txtServicio.Text;
            }

            // Establecer el ValueMember de cbmProducto
            cbmProducto.ValueMember = "id";

            // Obtener el valor seleccionado
            if (cbmProducto.SelectedValue != null)
            {
                id_producto = Convert.ToInt32(cbmProducto.SelectedValue);
            }

            // Cambiar el comportamiento según el texto de cbmProducto
            switch (cbmProducto.Text)
            {
                case "Cocinas":
                    // Solo intercambiar posiciones si aún no se ha hecho
                    if (!posicionesIntercambiadascbm)
                    {
                        txtServicio.Text = textoAnterior;
                        // Mover los controles a la izquierda
                        txtAncho.Left -= 604;
                        label17.Left -= 599;
                        txtLargo.Left -= 594;
                        label7.Left -= 600;

                        // Intercambiar las posiciones de lblTipo y label4, cbmTipo y cbmMaterial
                        var lblTipoLocation = lblTipo.Location;
                        var label4Location = label4.Location;
                        var cbmTipoLocation = cbmTipo.Location;
                        var cbmMaterialLocation = cbmMaterial.Location;

                        // Intercambiar las posiciones
                        lblTipo.Location = label4Location;
                        label4.Location = lblTipoLocation;
                        cbmTipo.Location = cbmMaterialLocation;
                        cbmMaterial.Location = cbmTipoLocation;

                        posicionesIntercambiadascbm = true;
                    }

                    // Cambiar visibilidad: Material invisible, Tipo visible
                    label4.Visible = false;
                    cbmMaterial.Visible = false;
                    lblTipo.Visible = true;
                    cbmTipo.Visible = true;

                    // Volver invisible otros controles
                    label16.Visible = false;
                    cbmMadera.Visible = false;
                    label19.Visible = false;
                    cbmTipoPuerta.Visible = false;
                    lblApanelado.Visible = false;
                    cbmApanelado.Visible = false;
                    lblJambas.Visible = false;
                    cbmJambas.Visible = false;
                    label12.Visible = false;
                    cbmEspesor.Visible = false;

                    //Hacer invisible la casilla de precio unitario por instalacion
                    label32.Visible = false;
                    txtInstalacion.Visible = false;

                    //Hacer visible las casillas de torres
                    lblTorre.Visible = true;
                    txtTorre.Visible = true;

                    // Hacer invisible las casillas de torres
                    lblTorre.Visible = false;
                    txtTorre.Visible = false;

                    break;

                case "Closet":

                    txtServicio.Text = textoAnterior;
                    // Mostrar controles específicos de Closet
                    lblTorre.Visible = true;
                    txtTorre.Visible = true;

                    // Marcar la bandera como true porque estamos en Closet
                    esCloset = true;

                    // Luego, hacer todo lo que hace el caso de "Puertas"
                    goto case "Puertas";

                case "Puertas":

                    txtServicio.Text = textoAnterior;
                    // Solo restaurar posiciones si estaban intercambiadas
                    if (posicionesIntercambiadascbm)
                    {
                        // Mover los controles de vuelta a la derecha
                        txtAncho.Left += 604;
                        label17.Left += 599;
                        txtLargo.Left += 594;
                        label7.Left += 600;

                        // Restaurar las posiciones originales de lblTipo y label4, cbmTipo y cbmMaterial
                        var lblTipoLocation = lblTipo.Location;
                        var label4Location = label4.Location;
                        var cbmTipoLocation = cbmTipo.Location;
                        var cbmMaterialLocation = cbmMaterial.Location;

                        // Intercambiar de vuelta las posiciones
                        label4.Location = lblTipoLocation;
                        lblTipo.Location = label4Location;
                        cbmMaterial.Location = cbmTipoLocation;
                        cbmTipo.Location = cbmMaterialLocation;

                        posicionesIntercambiadascbm = false;
                    }

                    // Cambiar visibilidad: Material visible, Tipo invisible
                    label4.Visible = true;
                    cbmMaterial.Visible = true;
                    lblTipo.Visible = false;
                    cbmTipo.Visible = false;

                    // Volver visibles otros controles
                    label16.Visible = true;
                    cbmMadera.Visible = true;
                    label19.Visible = true;
                    cbmTipoPuerta.Visible = true;
                    lblApanelado.Visible = true;
                    cbmApanelado.Visible = true;
                    lblJambas.Visible = true;
                    cbmJambas.Visible = true;
                    label12.Visible = true;
                    cbmEspesor.Visible = true;

                    // Hacer invisible la casilla de precio unitario por instalación
                    label32.Visible = false;
                    txtInstalacion.Visible = false;

                    // Si **NO** es "Closet", hacer invisibles las casillas de torres
                    if (!esCloset)
                    {
                        lblTorre.Visible = false;
                        txtTorre.Visible = false;
                    }

                    // Restablecer la bandera
                    esCloset = false;

                    break;

                case "Instalacion":

                    //Cambiar text
                    txtServicio.Text = "Instalacion";

                    //Hacer visible la casilla de precio unitario por instalacion
                    label32.Visible = true;
                    txtInstalacion.Visible = true;

                    // Hacer invisible las casillas de torres
                    lblTorre.Visible = false;
                    txtTorre.Visible = false;

                    break;

                default:
                    // Restaurar visibilidad y cualquier otro comportamiento si se selecciona otra opción
                    txtServicio.Text = textoAnterior;
                    txtInstalacion.Visible = false;
                    label32.Visible = false;
                    lblTorre.Visible = false;
                    txtTorre.Visible = false;

                    if (!cbmJambas.Items.Contains("Dobles"))
                    {
                        cbmJambas.Items.Add("Dobles");
                    }
                    break;
            }
        }
        

        private void cbmMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {

            cbmMaterial.ValueMember = "id";
            // Obtener el valor seleccionado
            if (cbmMaterial.SelectedValue != null)
            {
                id_Material = Convert.ToInt32(cbmMaterial.SelectedValue);
            }

            if (cbmMaterial.Text != "Madera")
            {
                cbmMadera.Visible = false;
                label16.Visible = false;
            }
            else
            {
                cbmMadera.Visible = true;
                label16.Visible = true;
            }
        }

        private void cbmMadera_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbmMadera.ValueMember = "id";
            // Obtener el valor seleccionado
            if (cbmMadera.SelectedValue != null)
            {
                id_Madera = Convert.ToInt32(cbmMadera.SelectedValue);
            }
            
        }

        public void SeleccionApanelado()
        {
            // Verifica si el tipo de puerta es "Semisólida" o "Apanelada"
            if (cbmTipoPuerta.Text == "Semisolida" || cbmTipoPuerta.Text == "Apanelada")
            {
                double incrementoApanelado = 0;

                // Aplica un incremento específico dependiendo del material seleccionado
                switch (cbmApanelado.Text)
                {
                    case "Plywood":
                        incrementoApanelado = 0.1; // Ejemplo: un incremento del 15%
                        break;
                    case "Madera":
                        incrementoApanelado = 0.20; // Ejemplo: un incremento del 20%
                        break;
                    case "Cristal":
                        incrementoApanelado = 0.40; // Ejemplo: un incremento del 25%
                        break;
                    default:
                        break;
                }

                // Calcula el precio total con el incremento del apanelado
                double totalConApanelado = precioAjustado * (1 + incrementoApanelado);

                // Actualiza el precio ajustado con el apanelado
                precioAjustado = totalConApanelado;

                // Actualiza el precio total en el formulario
                lblTotalln.Text = totalConApanelado.ToString("N2");
            }
        }


        private double porcentajeIncrementoTipoPuerta = 0; // Variable para almacenar el porcentaje de incremento del tipo de puerta

        public void SeleccionTipoPuerta()
        {
            if (double.TryParse(lblTotalln.Text, out double total))
            {
                switch (cbmTipoPuerta.Text)
                {
                    case "Apanelada":
                        porcentajeIncrementoTipoPuerta = 0.08; // Ejemplo: un incremento del 10%
                        break;

                    case "Semisolida":
                        porcentajeIncrementoTipoPuerta = 0.30; // Ejemplo: un incremento del 20%
                        break;

                    case "Maciza":
                        porcentajeIncrementoTipoPuerta = 0.45; // Ejemplo: un incremento del 30%
                        break;

                    case "Chapada":
                        porcentajeIncrementoTipoPuerta = 0.25; // Ejemplo: un incremento del 15%
                        break;

                    default:
                        porcentajeIncrementoTipoPuerta = 0; // Sin incremento
                        break;
                }

                total = precioBase * (1 + porcentajeIncrementoTipoPuerta); // Calcula el precio total con el incremento
                lblTotalln.Text = total.ToString("N2");
                precioAjustado = total; // Actualiza el precio ajustado
            }
        }

        private bool calculoDoblesRealizado = false; // Variable para controlar si el cálculo para "Dobles" ya se realizó
        private double precioBase = 0; // Variable para almacenar el precio base
        private double precioAjustado = 0; // Variable para almacenar el precio ajustado con las jambas

        public void SeleccionJambas()
        {
            if (double.TryParse(lblTotalln.Text, out double total))
            {
                switch (cbmJambas.Text)
                {
                    case "Frontales":
                        // No hay incremento, solo restablece el precio ajustado
                        lblTotalln.Text = precioAjustado.ToString("N2");
                        break;

                    case "Dobles":
                        double incrementoJambas = precioBase * 0.20; // Incremento del 20% para jambas dobles
                        precioAjustado += incrementoJambas; // Suma el incremento al precio ajustado
                        lblTotalln.Text = precioAjustado.ToString("N2");
                        break;

                    default:
                        // Manejo para otras opciones si es necesario
                        break;
                }
            }
        }


        public void SeleccionTerminacion()
        {
            if (double.TryParse(lblTotalln.Text, out double total))
            {
                switch (cbmTerminacion.Text)
                {
                    case "Lacado natural":
                        double incrementoLacadoNatural = precioBase * 0.15; // Incremento del 15% para lacado natural
                        precioAjustado += incrementoLacadoNatural; // Suma el incremento al precio ajustado
                        lblTotalln.Text = precioAjustado.ToString("N2");
                        break;

                    case "Color":
                        double incrementoColor = precioBase * 0.20; // Incremento del 20% para color
                        precioAjustado += incrementoColor; // Suma el incremento al precio ajustado
                        lblTotalln.Text = precioAjustado.ToString("N2");
                        break;

                    case "Amaderada":
                        double incrementoAmaderado = precioBase * 0.25; // Incremento del 20% para color
                        precioAjustado += incrementoAmaderado; // Suma el incremento al precio ajustado
                        lblTotalln.Text = precioAjustado.ToString("N2");
                        break;

                    default:
                        // Manejo para otras opciones si es necesario
                        break;
                }
            }
        }

        public void Calcular()
        {
            if (cbmProducto.Text != "Instalacion")
            {
                // Crear una instancia de tu clase Precios
                Precios precios = new Precios();

                // Asignar valores a las propiedades de la clase
                precios.Tipo_Madera_id = id_Madera;
                precios.Tipo_Material_id = id_Material;
                precios.Producto_id = id_producto;

                if (float.TryParse(txtAncho.Text, out float ancho) && float.TryParse(txtLargo.Text, out float largo))
                {
                    // Si se pudo convertir correctamente, asigna el valor a precios.Ancho
                    precios.Ancho = ancho;
                    precios.Largo = largo;
                }

                // Ejecutar el procedimiento almacenado y obtener el resultado en un DataTable
                System.Data.DataTable resultado = precios.ObtenerPrecioPuerta();

                try
                {
                    // Verificar si hay filas en el resultado
                    if (resultado.Rows.Count > 0)
                    {
                        // Obtener el precio de la primera fila (asumiendo que solo esperas un resultado)
                        float precio = Convert.ToSingle(resultado.Rows[0]["PrecioAjustado"]);
                        cantidad = int.Parse(txtCantidad.Text);
                        precio = precio * cantidad;

                        // Verificar si el producto es un closet y si hay torres
                        if (cbmProducto.Text == "Closet")
                        {
                            if (!string.IsNullOrEmpty(txtTorre.Text) && int.TryParse(txtTorre.Text, out int torres))
                            {
                                // Sumar un porcentaje al precio por cada torre
                                double porcentajeAumento = 0.80; // Por ejemplo, un 5% de aumento por torre
                                double aumento = precio * porcentajeAumento * torres;
                                precio += (float)aumento;
                            }
                        }

                        double impuesto = precio * 0.18;

                        lblitbis.Text = impuesto.ToString("N2", CultureInfo.InvariantCulture);

                        // Asignar el precio al TextBox
                        precioBase = precio; // Asignar a la variable de precio base
                        lblTotalln.Text = precioBase.ToString("N2", CultureInfo.InvariantCulture);
                        lblactualtotal.Text = precioBase.ToString("N2", CultureInfo.InvariantCulture);

                        // Resetea el precio ajustado y la bandera de cálculo
                        precioAjustado = precioBase;
                        calculoDoblesRealizado = false;

                        // Aplicar selección de tipo de puerta después de calcular el precio base
                        SeleccionTipoPuerta();

                        // Aplicar selección de apanelado después de ajustar por el tipo de puerta
                        SeleccionApanelado();

                        // Aplicar selección de terminación después de ajustar por el tipo de puerta y apanelado
                        SeleccionTerminacion();

                        // Aplicar selección de jambas después de ajustar por la terminación


                        // Aplicar selección de espesor
                        SeleccionEspesor();

                        SeleccionJambas();
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron resultados.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al obtener el precio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    cantidad = int.Parse(txtCantidad.Text);
                    preciou = float.Parse(txtInstalacion.Text);

                    double total = cantidad * preciou;
                    lblTotalln.Text = total.ToString("N2", CultureInfo.InvariantCulture);
                    double impuesto = total * 0.18;

                    lblitbis.Text = impuesto.ToString("N2", CultureInfo.InvariantCulture);
                }
                catch
                {
                    MessageBox.Show("Debe asignar un precio unitario a la instalación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        public void SeleccionEspesor()
        {
            if (double.TryParse(lblTotalln.Text, out double total))
            {
                switch (cbmEspesor.Text)
                {
                    case "1.5Pul":
                        double incrementoEspesor1p5 = precioBase * 0.10; // Incremento del 10% para espesor de 1.5 pulgadas
                        precioAjustado += incrementoEspesor1p5; // Suma el incremento al precio ajustado
                        lblTotalln.Text = precioAjustado.ToString("N2");
                        break;

                    case "2.0Pul":
                        double incrementoEspesor2p0 = precioBase * 0.15; // Incremento del 15% para espesor de 2.0 pulgadas
                        precioAjustado += incrementoEspesor2p0; // Suma el incremento al precio ajustado
                        lblTotalln.Text = precioAjustado.ToString("N2");
                        break;

                    default:
                        // Manejo para otras opciones si es necesario
                        break;
                }
            }
        }




        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            if (cbmProducto.Text != "Instalacion" && cbmMaterial.Text == "Madera")
            {
                Calcular();
            }
            else if (cbmProducto.Text == "Closet" && cbmMaterial.Text == "Melamina")
            {
                CalcularClosetTorre();
            }
        }



        private void Insertar()
        {
            try
            {
                int xRows = dtaVentas.Rows.Add();

                double Totalln;
                if (!double.TryParse(lblTotalln.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out Totalln))
                {
                    MessageBox.Show("El campo Totalln no tiene un formato válido.");
                    return;
                }

                double Itbis = 0;
                if (cbmImpuesto.Text != "No" && !double.TryParse(lblitbis.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out Itbis))
                {
                    MessageBox.Show("El campo Itbis no tiene un formato válido.");
                    return;
                }

                int cantidad;
                if (!int.TryParse(txtCantidad.Text, out cantidad))
                {
                    MessageBox.Show("El campo Cantidad no tiene un formato válido.");
                    return;
                }

                double Punidad = Totalln / cantidad;

                double pintalacion;
                if (!double.TryParse(txtInstalacion.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out pintalacion))
                {
                    MessageBox.Show("El campo Instalación no tiene un formato válido.");
                    return;
                }

                // Si cbmImpuesto.Text es "No", Itbis se establece en 0
                Itbis = cbmImpuesto.Text == "No" ? 0 : Itbis;

                if (cbmProducto.Text == "Puertas" || cbmProducto.Text == "Closet")
                {
                    dtaVentas.Rows[xRows].Cells[0].Value = txtServicio.Text;
                    dtaVentas.Rows[xRows].Cells[1].Value = cbmProducto.Text;
                    dtaVentas.Rows[xRows].Cells[2].Value = cbmMaterial.Text;
                    dtaVentas.Rows[xRows].Cells[3].Value = cbmMadera.Text;
                    dtaVentas.Rows[xRows].Cells[4].Value = cbmApanelado.Text;
                    dtaVentas.Rows[xRows].Cells[5].Value = cbmJambas.Text;
                    dtaVentas.Rows[xRows].Cells[6].Value = $"{txtAncho.Text} X {txtLargo.Text}";
                    dtaVentas.Rows[xRows].Cells[7].Value = txtCantidad.Text;
                    dtaVentas.Rows[xRows].Cells[8].Value = Punidad.ToString("N2");
                    dtaVentas.Rows[xRows].Cells[9].Value = Itbis.ToString("N2"); // Siempre se inserta 0 si el impuesto es "No"
                    dtaVentas.Rows[xRows].Cells[10].Value = Totalln.ToString("N2");
                }
                else if (cbmProducto.Text == "Cocinas")
                {
                    dtaVentas.Rows[xRows].Cells[0].Value = txtServicio.Text;
                    dtaVentas.Rows[xRows].Cells[1].Value = cbmTipo.Text;
                    dtaVentas.Rows[xRows].Cells[2].Value = "";
                    dtaVentas.Rows[xRows].Cells[3].Value = "";
                    dtaVentas.Rows[xRows].Cells[4].Value = "";
                    dtaVentas.Rows[xRows].Cells[5].Value = "";
                    dtaVentas.Rows[xRows].Cells[6].Value = $"{txtAncho.Text} X {txtLargo.Text}";
                    dtaVentas.Rows[xRows].Cells[7].Value = txtCantidad.Text;
                    dtaVentas.Rows[xRows].Cells[8].Value = Punidad.ToString("N2");
                    dtaVentas.Rows[xRows].Cells[9].Value = Itbis.ToString("N2");
                    dtaVentas.Rows[xRows].Cells[10].Value = Totalln.ToString("N2");
                }
                else
                {
                    dtaVentas.Rows[xRows].Cells[0].Value = txtServicio.Text;
                    dtaVentas.Rows[xRows].Cells[1].Value = "";
                    dtaVentas.Rows[xRows].Cells[2].Value = "";
                    dtaVentas.Rows[xRows].Cells[3].Value = "";
                    dtaVentas.Rows[xRows].Cells[4].Value = "";
                    dtaVentas.Rows[xRows].Cells[5].Value = "";
                    dtaVentas.Rows[xRows].Cells[6].Value = "";
                    dtaVentas.Rows[xRows].Cells[7].Value = txtCantidad.Text;
                    dtaVentas.Rows[xRows].Cells[8].Value = pintalacion.ToString("N2");
                    dtaVentas.Rows[xRows].Cells[9].Value = Itbis.ToString("N2");
                    dtaVentas.Rows[xRows].Cells[10].Value = Totalln.ToString("N2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}");
            }
        }



        private void CalcularClosetTorre()
        {
            double total = 0;
            string input = txtTorre.Text;
            int cantidad;

            // Verificar si el valor de torres es un número y obtener la cantidad
            if (int.TryParse(input, out int torres) && int.TryParse(txtCantidad.Text, out cantidad))
            {
                // Si hay torres, calcular el total basado en el número de torres
                total = torres * 21000 * cantidad;
                lblTotalln.Text = total.ToString("N2");
            }
            else
            {
                // Si no se ingresaron torres, calcular el precio por medidas
                if (float.TryParse(txtAncho.Text, out float ancho) && float.TryParse(txtLargo.Text, out float largo) &&
                    int.TryParse(txtCantidad.Text, out cantidad))
                {
                    // Calcular el total basado en las medidas
                    total = ancho * largo * 10000; // Ejemplo de precio por área (ajusta según sea necesario)
                    total *= cantidad; // Multiplicar por la cantidad de closets

                    precioAjustado = total;
                    precioBase = 10000;
                    lblTotalln.Text = precioAjustado.ToString("N2");

                    SeleccionTerminacion();
                    SeleccionJambas();
                    SeleccionApanelado();
                    SeleccionEspesor();
                    SeleccionTipoPuerta();


                }
                else
                {
                    MessageBox.Show("Por favor, ingresa medidas y cantidad válidas.");
                    return; // Detener ejecución si no se ingresan valores válidos
                }
            }

            // Mostrar el total ajustado en la etiqueta


            // Calcular impuesto
            double impuesto = total * 0.18;
            lblitbis.Text = impuesto.ToString("N2", CultureInfo.InvariantCulture);
        }


        private void TotalTot()
        {
            double xTotal = 0;
            double xSubtotal = 0;
            double xImpuesto = 0;


            lblTotal.Text = "";
            lblSubTotal.Text = "";
            lblImpuesto.Text = "";

            foreach (DataGridViewRow row in dtaVentas.Rows)
            {
                if (row != null && row.Cells[9] != null && row.Cells[10].Value != null)
                {
                    // Añade un bloque try-catch para capturar la excepción específica
                    try
                    {
                        if (double.TryParse(Convert.ToString(row.Cells[10].Value), NumberStyles.Any, CultureInfo.InvariantCulture, out double total) &&
                            double.TryParse(Convert.ToString(row.Cells[9].Value), NumberStyles.Any, CultureInfo.InvariantCulture, out double impuesto))
                        {


                            if (cbmImpuesto.Text == "Si")
                            {
                                xImpuesto += impuesto;
                            }

                            xSubtotal += total;
                            xTotal += total + impuesto;


                        }
                        else
                        {
                            MessageBox.Show($"Error al convertir el valor '{row.Cells[9].Value}' de la celda a número.", "Error de conversión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;  // Salir del método si la conversión falla
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al convertir el valor de la celda a número: {ex.Message}", "Error de conversión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;  // Salir del método en caso de excepción
                    }
                }
            }

            lblTotal.Text = xTotal.ToString("N2", CultureInfo.InvariantCulture);
            lblSubTotal.Text = xSubtotal.ToString("N2", CultureInfo.InvariantCulture);
            lblImpuesto.Text = xImpuesto.ToString("N2", CultureInfo.InvariantCulture);

        }

        private void btnIns_Click(object sender, EventArgs e)
        {
            double total;
            if (double.TryParse(lblTotalln.Text, out total))
            {
                // La conversión fue exitosa, puedes usar la variable 'total'
                lblitbis.Text = (total * 0.18).ToString("N2"); // Ejemplo: calcular el 18% de ITBIS
            }
            if (cbmProducto.Text == "Cocinas")
            {
                if (double.TryParse(lblTotalln.Text, out total))
                {
                    // La conversión fue exitosa, puedes usar la variable 'total'
                    lblitbis.Text = (total * 0.18).ToString("N2"); // Ejemplo: calcular el 18% de ITBIS
                }
            }

            Insertar();
            TotalTot();
            if (txtTransporte.Text.Length > 0)
            {
                SumarTransporte();
            }
        }


        private void imprimir()
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                if (swtipe.Checked == true)
                {
                    save.FileName = lblFac.Text + "-" + txtNombre.Text + ".pdf";
                }
                else
                {
                    save.FileName = ".pdf";
                }
                save.DefaultExt = "pdf";
                save.Filter = "Archivos PDF (*.pdf)|*.pdf";

                string plantilla_html = Properties.Resources.plantilla.ToString();

                double transporte;

                // Intenta convertir el valor de txtTransporte.Text a double
                if (!double.TryParse(txtTransporte.Text, out transporte))
                {
                    transporte = 0; // Establece un valor predeterminado si la conversión falla
                }

                // Reemplazos de valores en la plantilla HTML
                plantilla_html = plantilla_html.Replace("@Cliente", txtNombre.Text);
                plantilla_html = plantilla_html.Replace("@Rnc", txtRnc.Text);
                plantilla_html = plantilla_html.Replace("@Email", txtCorreo.Text);
                plantilla_html = plantilla_html.Replace("@Direccion", txtDireccion.Text);
                plantilla_html = plantilla_html.Replace("@Contacto", txtTelefono.Text);
                plantilla_html = plantilla_html.Replace("@Proyecto", txtProyecto.Text);
                plantilla_html = plantilla_html.Replace("@ENTRADA", txtEntrada.Text);
                plantilla_html = plantilla_html.Replace("@Salida", txtSalida.Text);
                plantilla_html = plantilla_html.Replace("@Entregada", txtEntrega.Text);

                // Verifica si hay un valor válido en txtTransporte.Text para reemplazar en la plantilla HTML
                if (!string.IsNullOrEmpty(txtTransporte.Text))
                {
                    plantilla_html = plantilla_html.Replace("@TRANSPORTE", "$" + transporte.ToString("N2"));
                }
                else
                {
                    plantilla_html = plantilla_html.Replace("@TRANSPORTE", "");
                }


                plantilla_html = plantilla_html.Replace("@TIEMPOENTREGA", txtTiempoEntrega.Text);
                plantilla_html = plantilla_html.Replace("@vigencia", txtVigencia.Text);
                plantilla_html = plantilla_html.Replace("@Asesor", txtAsesor.Text);

                plantilla_html = plantilla_html.Replace("@Titulo", txtTitulo.Text);
                plantilla_html = plantilla_html.Replace("@Fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                plantilla_html = plantilla_html.Replace("@SubTotal", "$" + lblSubTotal.Text);
                plantilla_html = plantilla_html.Replace("@Impuestos", "$" + lblImpuesto.Text);
                plantilla_html = plantilla_html.Replace("@Total", "$" + lblTotal.Text);

                // Reemplazos adicionales dependiendo de las opciones seleccionadas
                plantilla_html = plantilla_html.Replace("@NumFac", lblFac.Text);
                if (cbmNCF.Text == "Si")
                {
                    plantilla_html = plantilla_html.Replace("@CONT", lblNCF.Text);
                }
                else
                {
                    plantilla_html = plantilla_html.Replace("@CONT", "");
                    plantilla_html = plantilla_html.Replace("NCF:", "");
                }

                // Agregar filas de la tabla de ventas
                string filas = string.Empty;
                foreach (DataGridViewRow Row in dtaVentas.Rows)
                {
                    filas += "<tr>";
                    filas += "<td style='font-size: 12px; width: 53%;'>" + Row.Cells["Descripcion"].Value.ToString() + "</td>";
                    filas += "<td align='center' style='font-size: 13px; font-weight: bold; width: 12%;'>" + Row.Cells["Sizes"].Value.ToString() + "</td>";
                    filas += "<td align='right' style='font-size: 13px; font-weight: bold; width: 12%;'>" + "$" + Row.Cells["PrecioUnidad"].Value.ToString() + "</td>";
                    filas += "<td align='center' style='width: 8%; font-weight: bold; font-size: 13px;'>" + Row.Cells["Canti"].Value.ToString() + "</td>";
                    filas += "<td align='right' style='width: 15%; font-weight: bold; font-size: 13px;'>" + "$" + Row.Cells["Total_Linea"].Value.ToString() + "</td>";
                    filas += "</tr>";
                }
                plantilla_html = plantilla_html.Replace("@Lista", filas);

                // Agregar precios incluidos y no incluidos
                string precioIn = string.Empty;
                foreach (string linea in txtPrecioIncluye.Lines)
                {
                    precioIn += "<li>" + linea + "</li>";
                }
                plantilla_html = plantilla_html.Replace("@PrecioIn", precioIn);

                string precioNoIN = string.Empty;
                foreach (string linea in txtPrecioNoIncluye.Lines)
                {
                    precioNoIN += "<li>" + linea + "</li>";
                }
                plantilla_html = plantilla_html.Replace("@PrecioNOIn", precioNoIN);

                string condicion = string.Empty;
                foreach (string linea in txtCondicion.Lines)
                {
                    condicion += "<li>" + linea + "</li>";
                }
                plantilla_html = plantilla_html.Replace("@Condicion", condicion);

                string Imagenes = string.Empty;
                if (listBox1.Items.Count > 0)
                {


                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        string imagen = listBox1.Items[i].ToString();
                        Imagenes += $"<tr><td><table style='margin-top: 50pt;'><tr><td><div style='page-break-after: always; text-align: center; margin-top: 20pt;'><img src='{imagen}' style='vertical-align: middle; width: 400px height: 500px;' /></div></td></tr></table></td></tr>";
                    }
                }
                else
                {
                    Imagenes = $"<tr><td></td></tr>";
                }
                plantilla_html = plantilla_html.Replace("@ImagenesExtra", Imagenes);


                // Guardar la imagen de la firma desde los recursos en una ruta temporal
                string tempImagePath = Path.Combine(Path.GetTempPath(), "firma.png");
                Properties.Resources.firma.Save(tempImagePath);

                // Crear el HTML para la imagen
                string imagenHtml = $"<img src='file:///{tempImagePath.Replace('\\', '/')}' alt='Imagen de la factura' style='max-width:100%; height:auto; display:block; margin:auto;' />";
                plantilla_html = plantilla_html.Replace("@img", imagenHtml);



                if (save.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(save.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 25, 25, 75, 195);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);


                        // Ajusta el margen inferior


                        CustomHeader header = new CustomHeader(); // Crear instancia del header personalizado
                        writer.PageEvent = header; // Asignar el evento del header personalizado

                        Footer footer = new Footer(); // Crear instancia del footer
                        writer.PageEvent = footer; // Asignar el evento del footer

                        pdfDoc.Open();
                        pdfDoc.Add(new Phrase(""));

                        PdfContentByte cb = writer.DirectContent;
                        ColumnText ct = new ColumnText(cb);
                        ct.SetSimpleColumn(new Rectangle(36, 36, 559, 806));

                        using (StringReader str = new StringReader(plantilla_html))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, str);

                            ct.Go();
                            float yPosition = ct.YLine;
                            float footerHeight = 200; // Ajusta esto según el tamaño real del footer
                            float minYPosition = pdfDoc.BottomMargin + footerHeight;

                            if (yPosition < minYPosition)
                            {
                                pdfDoc.NewPage();
                                ct.SetSimpleColumn(new Rectangle(36, 36, 559, 806));
                            }
                        }

                        pdfDoc.Close();
                        stream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir la factura: " + ex.Message);
            }
        }

        public class Footer : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfPTable footer = new PdfPTable(3); // Cambia a 3 columnas
                footer.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                footer.DefaultCell.Border = 0;
                footer.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                footer.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                footer.WidthPercentage = 100; // Asegura que la tabla ocupe el ancho completo

                // Primera imagen
                var logo1 = Properties.Resources.sello_solo_impdoor; // Accede al recurso de la primera imagen
                using (MemoryStream ms1 = new MemoryStream())
                {
                    logo1.Save(ms1, ImageFormat.Png);
                    ms1.Seek(0, SeekOrigin.Begin);
                    iTextSharp.text.Image pdfImage1 = iTextSharp.text.Image.GetInstance(ms1.ToArray());
                    pdfImage1.ScaleAbsolute(350f, 120f); // Ajusta el tamaño de la imagen si es necesario (ancho x alto)
                    PdfPCell imageCell1 = new PdfPCell(pdfImage1);
                    imageCell1.Border = 0;
                    imageCell1.HorizontalAlignment = Element.ALIGN_LEFT;
                    imageCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    imageCell1.PaddingLeft = -100f; // Ajuste horizontal de la primera imagen
                    footer.AddCell(imageCell1);
                }

                // Celda vacía en el medio para separar las dos imágenes
                PdfPCell emptyCell = new PdfPCell();
                emptyCell.Border = 0;
                footer.AddCell(emptyCell);

                // Segunda imagen
                var logo2 = Properties.Resources.datosfooter; // Accede al recurso de la segunda imagen
                using (MemoryStream ms2 = new MemoryStream())
                {
                    logo2.Save(ms2, ImageFormat.Png);
                    ms2.Seek(0, SeekOrigin.Begin);
                    iTextSharp.text.Image pdfImage2 = iTextSharp.text.Image.GetInstance(ms2.ToArray());
                    pdfImage2.ScaleAbsolute(220f, 100f); // Ajusta el tamaño de la segunda imagen si es necesario (ancho x alto)
                    PdfPCell imageCell2 = new PdfPCell(pdfImage2);
                    imageCell2.Border = 0;
                    imageCell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    imageCell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                    imageCell2.PaddingRight = -10f; // Ajuste horizontal de la segunda imagen
                    footer.AddCell(imageCell2);
                }

                float xPosition = document.LeftMargin;
                float yPosition = document.BottomMargin + 40; // Ajuste vertical para ambas imágenes

                // Escribir el footer en la posición especificada
                footer.WriteSelectedRows(0, -1, xPosition, yPosition, writer.DirectContent);
            }
        }


        public class CustomHeader : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfPTable header = new PdfPTable(1);
                header.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                header.DefaultCell.Border = 0;
                header.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                header.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                var logoHeader = Properties.Resources.impdoor_logo; // Accede al recurso de imagen
                if (logoHeader == null)
                {
                    throw new FileNotFoundException("No se encontró la imagen en los recursos del proyecto.");
                }


                // Convertir la imagen a un MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    logoHeader.Save(ms, ImageFormat.Png);
                    ms.Seek(0, SeekOrigin.Begin);

                    // Convertir el MemoryStream a una instancia de iTextSharp.text.Image
                    iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(ms.ToArray());

                    // Ajustar la posición del header según el número de página
                    if (writer.PageNumber == 1)
                    {
                        pdfImage.ScaleToFit(200f, 50f);
                        PdfPCell imageCellHeader = new PdfPCell(pdfImage);
                        imageCellHeader.Border = 0;
                        imageCellHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                        imageCellHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                        header.AddCell(imageCellHeader);
                        float yPosition = document.PageSize.Height - document.TopMargin - 5; // Ajuste según sea necesario
                        header.WriteSelectedRows(0, -1, document.LeftMargin, yPosition, writer.DirectContent);
                    }
                    else
                    {
                        pdfImage.ScaleToFit(125f, 35f);
                        PdfPCell imageCellHeader = new PdfPCell(pdfImage);
                        imageCellHeader.Border = 0;
                        imageCellHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                        imageCellHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                        header.AddCell(imageCellHeader);
                        float yPosition = document.PageSize.Height - document.TopMargin + 45; // Ajuste según sea necesario
                        header.WriteSelectedRows(0, -1, document.LeftMargin, yPosition, writer.DirectContent);
                    }
                }
            }
        }







        private void btnImprimir_Click(object sender, EventArgs e)
        {
            imprimir();
            // comentario para probar push
        }

        private void txtCantidad_Leave(object sender, EventArgs e)
        {
            if (cbmProducto.Text == "Puertas" && cbmMaterial.Text == "Madera" || cbmProducto.Text == "Instalacion")
            {
                Calcular();
            }
            else if (cbmProducto.Text == "Closet" && cbmMaterial.Text == "Melamina")
            {
                CalcularClosetTorre();
            }

        }

        /*  private void Limpiar()
          {
              cbmMadera.SelectedIndex = -1;
              cbmJambas.SelectedIndex = -1;
              cbmApanelado.SelectedIndex = -1;
              cbmSize.SelectedIndex = -1;
              txtServicio.Clear();
              lblTotalln.Text = "";
              txtCantidad.Clear();
              txtInstalacion.Clear();
          }*/

        private void btnLimpiarD_Click(object sender, EventArgs e)
        {
            //Limpiar();
        }

        private void Ventas_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private bool posicionesIntercambiadas = false; // Bandera para controlar si las posiciones ya han sido intercambiadas

        private void cbmTipoPuerta_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Drawing.Point ubicacionCbmJamba = cbmJambas.Location;
            System.Drawing.Point ubicacionCbmApanelado = cbmApanelado.Location;
            System.Drawing.Point ubicacionTituloJamba = lblJambas.Location;
            System.Drawing.Point ubicacionTituloApanelado = lblApanelado.Location;

            if (cbmTipoPuerta.Text == "Semisolida" || cbmTipoPuerta.Text == "Apanelada")
            {
                lblApanelado.Visible = true;
                cbmApanelado.Visible = true;


                // Si las posiciones fueron intercambiadas previamente, restablece las ubicaciones
                if (posicionesIntercambiadas)
                {
                    cbmJambas.Location = ubicacionCbmApanelado;
                    cbmApanelado.Location = ubicacionCbmJamba;
                    lblJambas.Location = ubicacionTituloApanelado;
                    lblApanelado.Location = ubicacionTituloJamba;

                    posicionesIntercambiadas = false; // Restablece la bandera
                }
            }
            else
            {
                lblApanelado.Visible = false;
                cbmApanelado.Visible = false;

                // Si las posiciones no han sido intercambiadas, hazlo
                if (!posicionesIntercambiadas)
                {
                    // Intercambia las ubicaciones
                    cbmJambas.Location = ubicacionCbmApanelado;
                    cbmApanelado.Location = ubicacionCbmJamba;
                    lblJambas.Location = ubicacionTituloApanelado;
                    lblApanelado.Location = ubicacionTituloJamba;

                    posicionesIntercambiadas = true; // Marca que las posiciones han sido intercambiadas
                }
            }
        }

        private void AbrirDialogo()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";


                // Añadir cada nombre de archivo al ListBox
                foreach (string fileName in openFileDialog1.FileNames)
                {
                    listBox1.Items.Add(fileName);
                }
            }
        }
        private void BuscarCliente()
        {
            try
            {
                if (txtNombre.Text != string.Empty)
                {
                    string nombreBusqueda = txtNombre.Text.Trim();



                    cl.BuscarCliente(nombreBusqueda);

                    txtNombre.Text = cl.Nombre;
                    txtRnc.Text = cl.Rnc;
                    txtCorreo.Text = cl.Correo;
                    txtTelefono.Text = cl.Telefono;
                    txtDireccion.Text = cl.Direccion;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImagen_Click(object sender, EventArgs e)
        {
            AbrirDialogo();
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            BuscarCliente();
        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtInstalacion_Leave(object sender, EventArgs e)
        {
            if(txtCantidad.Text != string.Empty)
            {
                Calcular();
            }
        }

        private void SumarTransporte()
        {
            try
            {
                TotalTot();
                double total = double.Parse(lblTotal.Text);
                double transporte = double.Parse(txtTransporte.Text);
                double nuevoTotal = total + transporte;
                lblTotal.Text = nuevoTotal.ToString("N2");

            }
            catch (Exception ex)
            {
                if (txtTransporte.Text.Length > 0)
                {
                    MessageBox.Show($"Erorr al sumar el precio del transporte: {ex.Message} ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {
            SumarTransporte();
        }

        private void GuardarHeaderFactura()
        {
            String msj = "";

            if (lblFac.Text == string.Empty)
            {
                MessageBox.Show("La cotizacion debe tener un codigo identificador");
                return;
            }

            try
            {

                facturaH.Factura = lblFac.Text;
                facturaH.SubTotal = float.Parse(lblSubTotal.Text);
                facturaH.ITBIS = float.Parse(lblImpuesto.Text);
                facturaH.Total = float.Parse(lblTotal.Text);
                if (cbmNCF.Text == "Si")
                {
                    facturaH.Credito_Fiscal = lblNCF.Text;
                }
                facturaH.Estado_Pago = "Pendiente";
                facturaH.Cliente = txtNombre.Text;
                facturaH.Rnc = txtRnc.Text;


                msj = facturaH.GuardarFacturaHeader();
                MessageBox.Show(msj);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GuardarDetalleFactura()
        {
            String msj = "";

            try
            {

                foreach (DataGridViewRow Row in dtaVentas.Rows)
                {
                    facturaD.Factura = lblFac.Text;
                    facturaD.Producto = Row.Cells["Producto"].Value?.ToString() ?? string.Empty;
                    facturaD.Descripcion = Row.Cells["Descripcion"].Value?.ToString() ?? string.Empty;
                    facturaD.Material = Row.Cells["Material"].Value?.ToString() ?? string.Empty;
                    facturaD.Madera = Row.Cells["Madera"].Value?.ToString() ?? string.Empty;
                    facturaD.Apanelado = Row.Cells["Apanelado"].Value?.ToString() ?? string.Empty;
                    facturaD.Jambas = Row.Cells["Jamba"].Value?.ToString() ?? string.Empty;
                    facturaD.Size = Row.Cells["Sizes"].Value?.ToString() ?? string.Empty;
                    facturaD.Cantidad = int.Parse(Row.Cells["Canti"].Value?.ToString());
                    facturaD.PrecioUnit = float.Parse(Row.Cells["PrecioUnidad"].Value?.ToString());
                    facturaD.ITBIS = float.Parse(Row.Cells["ITBIS"].Value?.ToString());
                    facturaD.Total = float.Parse(Row.Cells["Total_Linea"].Value?.ToString());

                    msj = facturaD.GuardarFacturaDetalle();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            GuardarHeaderFactura();
            GuardarDetalleFactura();
        }

        private void SecuenciaFiscal()
        {
            try
            {
                // Llamar al método que ejecuta el procedimiento almacenado y devuelve un DataTable
                System.Data.DataTable dt = ncf.SecuenciaFiscal();

                // Verificar si se obtuvo algún resultado del procedimiento almacenado
                if (dt.Rows.Count > 0)
                {
                    // Obtener el valor del primer campo (suponiendo que sea un único valor)
                    string ncfDisponible = dt.Rows[0]["Codigo"].ToString(); // Reemplaza "NCF" con el nombre real de la columna

                    // Asignar el valor al Text del Label (suponiendo que tienes un Label llamado lblNCF)
                    lblNCF.Text = ncfDisponible;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el NCF disponible: " + ex.Message);
            }
        }

        private void cbmNCF_SelectedIndexChanged(object sender, EventArgs e)
        {
            OcultarNCF();
        }

        private void OcultarNCF()
        {
            if (cbmNCF.SelectedIndex == 0)
            {
                lblNCF.Visible = false;
                lblAviso.Visible = false;

            }
            else
            {
                lblNCF.Visible = true;
                lblAviso.Visible = true;

            }
        }

        private void cbmImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtServicio_TextChange(object sender, EventArgs e)
        {
            if (cbmProducto.Text != "Instalacion")
            {
                textoAnterior = txtServicio.Text;
            }
        }

        private void lblNCF_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTotalln_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbmTerminacion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private int ContarFilasRelevantes()
        {
            int contador = 0;

            // Recorremos las filas del DataGridView
            foreach (DataGridViewRow row in dtaVentas.Rows)
            {
                // Asumiendo que la columna donde se almacena el tipo de producto es la columna 1
                string producto = row.Cells[0].Value?.ToString() ?? "";

                // Excluir filas que sean solo "Instalación"
                if (producto != "Instalacion")
                {
                    contador++;
                }
            }

            return contador;
        }


        private void ActualizarDescripcion()
        {
            // Contar filas relevantes
            int numeroFila = ContarFilasRelevantes() + 1;  // Sumamos 1 porque será el siguiente número

            // Obtener las selecciones actuales de cada ComboBox
            string tipoProducto = cbmProducto.Text;
            string Material = cbmMaterial.Text;
            string tipoMadera = cbmMadera.Text;
            string espesor = cbmEspesor.SelectedItem?.ToString() ?? "";
            string jamba = cbmJambas.SelectedItem?.ToString() ?? "";
            string TipoPuerta = cbmTipoPuerta.SelectedItem?.ToString() ?? "";
            string apanelado = cbmApanelado.SelectedItem?.ToString() ?? "";
            string terminacion = cbmTerminacion.SelectedItem?.ToString() ?? "";
            string tipococina = cbmTipo.SelectedItem?.ToString() ?? "";
            string descripcion = "";

            // Construir la descripción
            if (Material == "Madera")
            {
                if(tipoProducto == "Puertas")
                {
                    descripcion = $"{numeroFila}-Puerta {tipoMadera}";
                }
                else if(tipoProducto == "Cocina")
                {
                    descripcion = $"{numeroFila}-{tipoProducto} {tipococina}";
                }
                else
                {
                    descripcion = $"{numeroFila}-{tipoProducto} {tipoMadera}";
                }
                
            }
            else
            {
                if (tipoProducto == "Puertas")
                {
                    descripcion = $"{numeroFila}-Puerta {Material}";
                }
                else if (tipoProducto == "Cocina")
                {
                    descripcion = $"{numeroFila}-{tipoProducto} {tipococina}";
                }
                else
                {
                    descripcion = $"{numeroFila}-{tipoProducto} {Material}";
                }
            }
            

            if (!string.IsNullOrEmpty(TipoPuerta))
            {
                descripcion += $" {TipoPuerta}";
            }

            if (!string.IsNullOrEmpty(apanelado))
            {
                descripcion += $" con  {apanelado}";
            }

            if (!string.IsNullOrEmpty(jamba))
            {
                descripcion += $", jambas {jamba}";
            }

            if (!string.IsNullOrEmpty(espesor))
            {
                descripcion += $", con espesor {espesor}";
            }

            if (!string.IsNullOrEmpty(terminacion))
            {
                descripcion += $", terminacion tipo {terminacion}";
            }

            txtServicio.Text = descripcion;
        }


        private void BorrarLinea()
        {
            if (dtaVentas.SelectedRows.Count > 0)
            {
                // Obtener la fila seleccionada
                DataGridViewRow filaSeleccionada = dtaVentas.SelectedRows[0];

                // Eliminar la fila seleccionada
                dtaVentas.Rows.Remove(filaSeleccionada);
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBorrarLinea_Click(object sender, EventArgs e)
        {
            BorrarLinea();
            TotalTot();
        }

        private void btnAutoDescripcion_Click(object sender, EventArgs e)
        {
            ActualizarDescripcion();
        }

        private void txtCondicion_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtaVentas_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            TotalTot();
        }

        private void Ventas_KeyDown(object sender, KeyEventArgs e)
        {
            // Variables para almacenar las posiciones actuales de scroll
            int currentX = Math.Abs(this.AutoScrollPosition.X);
            int currentY = Math.Abs(this.AutoScrollPosition.Y);

            // Desplazamiento hacia abajo
            if (e.KeyCode == Keys.Down)
            {
                int newY = currentY + 20; // Ajusta el valor según sea necesario
                this.AutoScrollPosition = new System.Drawing.Point(-currentX, newY); // Mantiene el X actual
                e.Handled = true; // Marca el evento como manejado
            }
            // Desplazamiento hacia arriba
            else if (e.KeyCode == Keys.Up)
            {
                int newY = currentY - 20; // Ajusta el valor según sea necesario
                this.AutoScrollPosition = new System.Drawing.Point(-currentX, newY); // Mantiene el X actual
                e.Handled = true; // Marca el evento como manejado
            }

        }

        private void txtNombre_KeyDown(object sender, KeyEventArgs e)
        {


        }
    }

}
