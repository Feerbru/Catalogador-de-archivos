using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExploradorArchivo
{
    public partial class Form1 : Form
    {
        private const string V = ":";
        string fullPach,rutaXML = "";
        public int conDisck = 0;
        HashSet<string> todasLasRutas = new HashSet<string>();
        List<string> listFiles = new List<string>();
        List<Elemento> elementos = new List<Elemento>();
        List<DataGridViewRow> tablaSinFliltro = new List<DataGridViewRow>();
        // Create a new DataTable.
        System.Data.DataTable table = new DataTable("ParentTable");
        // Declare variables for DataColumn and DataRow objects.
        DataColumn column;
        DataRow row;


        XML xml;
        //*************
        int nroDiscos;
        DriveInfo[] discos;
        Button[] itemDiscos;
        // ******************

        public Form1()
        {
            InitializeComponent();

            iniciaDiscos();
            //conDisck = 0;
            //List<string> MyList = new List<string>();
            //char[] arr = new char[] { 'c', 'd', 'e', 'f', 'x' };
            //try
            //{

            //    for (int i = 0; i < arr.Length; i++)
            //    {

            //        //C Drive Path, this is useful when you are about to find a Drive root from a Location Path.
            //        string path = arr[i] + ":";

            //        //Find its root directory i.e "C:\\"
            //        string rootDir = Directory.GetDirectoryRoot(path);

            //        //Get all information of Drive i.e C
            //        DriveInfo driveInfo = new DriveInfo(rootDir); //you can pass Drive path here e.g   DriveInfo("C:\\")



            //        long availableFreeSpace = driveInfo.AvailableFreeSpace;
            //        string driveFormat = driveInfo.DriveFormat;
            //        string name = driveInfo.Name;
            //        long totalSize = driveInfo.TotalSize;

            //        // MyList.Add("Dato1: " + availableFreeSpace + "Dato2: " + driveFormat + "Dato3: " + name + "Dato3: " + totalSize);
            //        //listBox1.Items.Add("Dato: " + availableFreeSpace + "Dato: " + driveFormat + "Dato: " + name + "Dato: " + totalSize);
            //        conDisck++;
            //        label1.Text = "Cantidad de Discos: " + Convert.ToString(conDisck);
            //    }



            //}
            //catch (Exception)
            //{

            //    //throw;
            //}
        }

        //******************************************************************************
        private void iniciaDiscos()
        {
            flowLayoutPanel1.Controls.Clear(); // limpia el tablero de los controles
            nroDiscos = 0;
            label1.Text = "";

            discos = DriveInfo.GetDrives(); // obtiene informacion de los discos activos
            itemDiscos = new Button[discos.Length]; // crea un arreglo para cargar los discos activos

            foreach (DriveInfo x in discos) // recorre los discos para obtener info y cargar el tablero
            {
                if (x.IsReady)
                { // verifica si la unidad esta activa

                    Button a = new Button();
                    a.Click += new EventHandler(btnDiscos_Click);
                    a.Width = 160;
                    a.Height = 40;
                    a.TextImageRelation = TextImageRelation.ImageBeforeText;
                    a.Image = imageList.Images[0];
                    a.ImageAlign = ContentAlignment.MiddleLeft;
                    a.Text = x.Name + " : " + x.VolumeLabel;

                    flowLayoutPanel1.Controls.Add(a);
                    itemDiscos[nroDiscos] = a; // carga el arreglo con todas las unidades activas
                    nroDiscos++;
                }
            }
        }

        private void btnDiscos_Click(object sender, System.EventArgs e)
        {
            label1.Text = ((Button)sender).Text;
        }
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            iniciaDiscos();
            listFiles.Clear();
            listView.Items.Clear();
            advancedDataGridView1.DataSource = null;
            advancedDataGridView1.Rows.Clear();
            preView.ImageLocation = "";
            txtPath.Text = "";
            fullPach = "";
            webBrowser.Navigate((Uri)null); ;
            borrarXml();
            tsbtnConfig.Enabled = false;
            tsbtnElim.Enabled = false;
            tsbtnBuscar.Enabled = false;
            //lbRutas.Text = "";
            listViewRutas.Clear();
            listViewRutas.Items.Add("Todos");
            todasLasRutas.Clear();
            table.Clear();
            xml = new XML();
            xml._crearXml();
            bindingSource1.DataSource = table;
            advancedDataGridView1.DataSource = bindingSource1;
            advancedDataGridView1.AutoGenerateColumns = true;
        }
        //***********************************************************

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            //Declarcion de variables
            string nombre;
            //Datos de entrada
            nombre = Microsoft.VisualBasic.Interaction.InputBox("Ingrese un Nombre para la carpeta:", "Crear Carpeta", "Ejemplo", 100, 100);
            //Crear Carpeta
            ExploradorArchivo.Controller.Botones.Menu menu = new Controller.Botones.Menu();
             menu.CrearCarpeta(nombre);
       
        }

        //***************************************************************************************



        private void Form1_Load(object sender, EventArgs e)
        {
            xml = new XML();
            xml._crearXml(); /*solo hay que crearlo una vez*/
     
            //creo la estructura de la tabla de datos
            // Creo una columna
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Ruta";
            column.ReadOnly = true;
            column.Unique = true;
            // Add the Column to the DataColumnCollection.
            table.Columns.Add(column);

            // Creo una columna
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "NombreArchivo";
            column.AutoIncrement = false;
            column.Caption = "Nombre Archivo";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Creo una columna
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Categoria";
            column.AutoIncrement = false;
            column.Caption = "Categoria";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Creo una columna
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Descripcion";
            column.AutoIncrement = false;
            column.Caption = "Descripcion";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Creo una columna
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Tipo";
            column.AutoIncrement = false;
            column.Caption = "Tipo";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            // Make the ID column the primary key column.
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = table.Columns["Ruta"];
            table.PrimaryKey = PrimaryKeyColumns;
            //binculo el dataTable a el datagridView

            bindingSource1.DataSource = table;
            advancedDataGridView1.DataSource = bindingSource1;
            advancedDataGridView1.AutoGenerateColumns = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string rutaBase = Ruta();
            fullPach = rutaBase;

            DirectoryInfo directoryInfo = new DirectoryInfo(rutaBase);
        }

        private string Ruta()
        {
            var filePath = string.Empty;

            using (FolderBrowserDialog ruta = new FolderBrowserDialog())
            {


                if (ruta.ShowDialog() == DialogResult.OK)
                {

                    filePath = ruta.SelectedPath;

                }
            }
            return filePath.ToString();
        }

        private TreeNode crearArbol(DirectoryInfo directoryInfo)
        {
            TreeNode treeNode = new TreeNode(directoryInfo.Name);

            foreach (var item in directoryInfo.GetDirectories())
            {
                treeNode.Nodes.Add(crearArbol(item));
            }

            foreach (var item in directoryInfo.GetFiles())
            {
                treeNode.Nodes.Add(new TreeNode(item.Name));
            }

            return treeNode;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cadena;
            cadena = label2.Text;
            string[] datoscortados = cadena.Split(':');
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select your path." })
            {
                fbd.SelectedPath = "C:\\";
                label2.Text = "" + datoscortados[3] + ":\\";
                fbd.SelectedPath = label2.Text;
                webBrowser.Url = new Uri(fbd.SelectedPath);
            }


        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            listFiles.Clear();
            listView.Items.Clear();
            //dataGridView1.DataSource = null;
            //dataGridView1.Rows.Clear();
            preView.ImageLocation = "";
            txtPath.Text = "";
            fullPach = "";
            webBrowser.Navigate((Uri)null); ;
            borrarXml();
            xml = new XML();
            xml._crearXml();

            //Open folder browser dialog
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select your path." })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    //Set path to textbox
                    txtPath.Text = fbd.SelectedPath;
                    fullPach = fbd.SelectedPath;
                    tsbtnConfig.Enabled = true;
                    tsbtnElim.Enabled = true;
                    tsbtnBuscar.Enabled = true;
                    label1.Text = "Disco seleccionado: " + fullPach.Substring(0,1);
                    
                    foreach (string item in Directory.GetFiles(fbd.SelectedPath))
                    {
                        //Add image to imagelist
                        imageList.Images.Add(System.Drawing.Icon.ExtractAssociatedIcon(item));
                        FileInfo fi = new FileInfo(item);
                        listFiles.Add(fi.FullName);//Add file name to list
                        listView.Items.Add(fi.Name, imageList.Images.Count - 1);
                        webBrowser.Url = new Uri(fullPach);
                    }
                   
                }
            }
        }

        private void webBrowser_Click(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select your path." })
            {
                
                 txtPath.Text = fbd.SelectedPath;

            }
        }

        //guardo los datos en el xml
        private void button1_Click(object sender, EventArgs e)
        {
            if (fullPach != null && fullPach != "")
            {
            todasLasRutas.Add(txtPath.Text);
                cargarCarpetasArchivos(txtPath.Text);
            //cargo las rutas al tablePag
            cargarRutas("");
            }
            else
            {
                MessageBox.Show("Cargue una ruta por favor");
            }
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (fullPach != "")
            {

           
            //Clear all items
            listFiles.Clear();
            listView.Items.Clear();
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select your path." })
            {

                txtPath.Text = fbd.SelectedPath;
                //fbd.SelectedPath = label2.Text;
                // webBrowser.Url = new Uri(fbd.SelectedPath);
                //txtPath.Text= webBrowser.Url.AbsoluteUri;
                txtPath.Text = webBrowser.Url.LocalPath;
                label2.Text = webBrowser.Url.LocalPath;
                fbd.SelectedPath = txtPath.Text;


                    try
                    {
                        foreach (string item in Directory.GetFiles(fbd.SelectedPath))
                        {
                            //Add image to imagelist
                            imageList.Images.Add(System.Drawing.Icon.ExtractAssociatedIcon(item));
                            FileInfo fi = new FileInfo(item);
                            listFiles.Add(fi.FullName);//Add file name to list
                                                       //Add file name and image to listview
                            listView.Items.Add(fi.Name, imageList.Images.Count - 1);
                        }
                    }
                    catch (Exception)
                    {

                       // throw;
                    }
                

            }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (webBrowser.CanGoForward)
                webBrowser.GoForward();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (webBrowser.CanGoBack)
                webBrowser.GoBack();
        }


        //se ejecuta cuando hacen click en un objeto del listado de los archivos
        private void listView_ItemActivate(object sender, EventArgs e)
        {
            
            ListView a = (ListView)sender; //casteo el objeto a listView
            string disco = fullPach.Substring(0, 1);
            foreach (ListViewItem item in a.SelectedItems)//obtengo todos los items seleccionados
            {
                Categoria categoria = new Categoria(); //cargo un formulario para ingresar los datos
                categoria.tbNombreArchivo.Text = item.Text; //cargo en nombre del archivo seleccionado en la ventana emergente
                categoria.ShowDialog(); //muestro la ventana

                //Verifica se se cambio el nombre del archivo
                if (categoria.tbNombreArchivo.Text != item.Text)
                {
                    if (categoria.tbNombreArchivo.Text != "")
                    {
                        FileSystem.Rename((fullPach + "\\" + item.Text), (fullPach + "\\" + categoria.tbNombreArchivo.Text));
                        item.Text = categoria.tbNombreArchivo.Text;
                    }
                   
                }

                //cago la tabla con los datos, verifica si ya existe si existe lo remplaza
                try
                {
                    //cargo la tabla
                    row = table.NewRow();
                    row["Ruta"] = txtPath.Text + "\\" + item.Text;
                    row["NombreArchivo"] = categoria.tbNombreArchivo.Text;
                    row["Categoria"] = categoria.tbCategoria.Text;
                    row["Descripcion"] = categoria.tbDescripcion.Text;
                    table.Rows.Add(row);
                    cargarCategoriaslb();
                }
                catch (Exception)
                {
                    MessageBox.Show("El Archivo Seleccionado ya se encuentra en la lista. Utilize los filtros para encontrarlo");
                }
                  
                
              
                //si el archivo es una imagen muestra una preview si no muestra un icono por defecto.
                string extencionArchivo = item.Text.Substring((item.Text.Length - 3), 3);
                if (extencionArchivo == "jpg" || extencionArchivo == "png")
                {
                    preView.ImageLocation = fullPach + "\\" + item.Text;
                }
                else {
                    //obtengo la ruta donde se ejecuta el programa y acceso a la carpeta resources
                    string ruta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    ruta = ruta.Substring(0, (ruta.Length - 9));
                    ruta = ruta + "Resources\\icono.png";
                    preView.ImageLocation = ruta;

                }
                //cargo las rutas
                todasLasRutas.Add(fullPach);
                //muestro las rutas
                cargarRutas("");
            }
           
        }

        //actualiza 1 objeto de la tabla
        private void updateTabla(string unidad, string categoria, string descripcion, string nombreArchivo)
        {
            //cargo los datos de la tabla a al XML
            foreach (DataGridViewRow dr in advancedDataGridView1.Rows)
            {
                if (dr.Cells[3].Value == nombreArchivo)
                {
                    dr.Cells[0].Value = unidad;
                    dr.Cells[1].Value = categoria;
                    dr.Cells[2].Value = descripcion;
                    dr.Cells[3].Value = nombreArchivo;
                }
            }
        }

        //copia el XML en la direccion que elijas
        private void guardarCambios() {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    xml.guardarComo(saveFileDialog1.FileName);
                }
            }
        }
        //borro el archivo XML *Se llama cuando cerrar el programa*
        private void borrarXml() {
            string rutaXml = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"archivo.xml");

            FileInfo fi = new FileInfo(rutaXml);
            fi.Delete();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            borrarXml();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            todasLasRutas.Clear();
            Stream myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "txt files (*.xml)|*.xml";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    //tabla
                    advancedDataGridView1.DataSource = null;
                    advancedDataGridView1.Rows.Clear();
                    table.Clear();
                    //rutas
                    txtPath.Text = xml.leerRuta(openFileDialog1.FileName).FirstOrDefault();
                    fullPach = xml.leerRuta(openFileDialog1.FileName).FirstOrDefault();
                    rutaXML = openFileDialog1.FileName;
                  
                    //botones
                    tsbtnConfig.Enabled = true;
                    tsbtnElim.Enabled = true;
                    tsbtnBuscar.Enabled = true;
                    //Lista de rutas
                    cargarRutas(openFileDialog1.FileName);
                    foreach (var item in xml._ReadXml(openFileDialog1.FileName))
                    {  
                        //cargo la tabla
                        row = table.NewRow();
                        row["Ruta"] = item.RutaArchivo;
                        row["NombreArchivo"] = item.NombreArchivo;
                        row["Categoria"] = item.Categoria;
                        row["Descripcion"] = item.Descrpcion;
                        row["Tipo"] = item.Tipo;
                        table.Rows.Add(row);
                    }
                    //cargo el xml temporal
                    foreach (DataGridViewRow dr in advancedDataGridView1.Rows)
                    {
                        xml._Añadir(
                            (dr.Cells[0].Value != null ? dr.Cells[0].Value.ToString() : string.Empty),//RutaArchivo
                            (dr.Cells[1].Value != null ? dr.Cells[1].Value.ToString() : string.Empty),//Categoria
                            (dr.Cells[3].Value != null ? dr.Cells[3].Value.ToString() : string.Empty),//Nombre Archivo
                            (dr.Cells[2].Value != null ? dr.Cells[2].Value.ToString() : string.Empty),//Descripcion
                            (dr.Cells[4].Value != null ? dr.Cells[4].Value.ToString() : string.Empty)//tipo
                            );
                    }

                  
                }
            }

            //cargo los archivos para mostrarlos
            //Clear all items
            listFiles.Clear();
            listView.Items.Clear();
            try
            {
                webBrowser.Url = new Uri(fullPach);
                foreach (string item in Directory.GetFiles(fullPach))
                {
                    //Add image to imagelist
                    imageList.Images.Add(System.Drawing.Icon.ExtractAssociatedIcon(item));
                    FileInfo fi = new FileInfo(item);
                    listFiles.Add(fi.FullName);//Add file name to list
                                               //Add file name and image to listview
                    listView.Items.Add(fi.Name, imageList.Images.Count - 1);
                }

            }
            catch (Exception)
            {
             //   MessageBox.Show("!ERROR, No se pede acceder a la ruta guardada");
            }
            cargarCategoriaslb();

            bindingSource1.DataSource = table;
            advancedDataGridView1.DataSource = bindingSource1;
            advancedDataGridView1.AutoGenerateColumns = true;

        }


        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listFiles.Clear();
            listView.Items.Clear();
            advancedDataGridView1.DataSource = null;
            advancedDataGridView1.Rows.Clear();
            preView.ImageLocation = "";
            txtPath.Text = "";
            fullPach = "";
            webBrowser.Navigate((Uri)null); ;
            borrarXml();
            tsbtnConfig.Enabled = true;
            tsbtnElim.Enabled = false;
            tsbtnBuscar.Enabled = false;
            //lbRutas.Text = "";
            listViewRutas.Clear();
            listViewRutas.Items.Add("Todos");
            todasLasRutas.Clear();
            table.Clear();
            xml = new XML();
            xml._crearXml();
            bindingSource1.DataSource = table;
            advancedDataGridView1.DataSource = bindingSource1;
            advancedDataGridView1.AutoGenerateColumns = true;
        }


        private void cargarCategoriaslb() {
            HashSet<string> categorias = new HashSet<string>();
            string cat = "";
            //cargo las categorias al lb de categorias
            foreach (DataGridViewRow dr in advancedDataGridView1.Rows)
            {
                categorias.Add(dr.Cells[2].Value.ToString());
            }
            foreach (var item in categorias)
            {
               cat = cat + "\n" + "*" +item + "\n";
            }
            lbCategorias.Text = cat;
            
        }

        private void acercadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GrupoIntegrantes a = new GrupoIntegrantes();
            a.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //limpio los filtros de la tabla
            this.bindingSource1.RemoveFilter();
            //cargo los datos de la tabla a al XML
            foreach (DataGridViewRow dr in advancedDataGridView1.Rows)
            {
                xml._Añadir(
                    (dr.Cells[2].Value != null ? dr.Cells[2].Value.ToString() : string.Empty),//Categoria
                    (dr.Cells[1].Value != null ? dr.Cells[1].Value.ToString() : string.Empty),//Nombre Archivo
                    (dr.Cells[3].Value != null ? dr.Cells[3].Value.ToString() : string.Empty),//Descripcion
                    (dr.Cells[0].Value != null ? dr.Cells[0].Value.ToString() : string.Empty),//RutaArchivo
                    (dr.Cells[4].Value != null ? dr.Cells[4].Value.ToString() : string.Empty)//Tipo
                    );
            }
            foreach (var item in todasLasRutas)
            {
                xml._Añadir_ruta(item);
            }
            guardarCambios();
            MessageBox.Show("!Guardado con Exito");
            todasLasRutas.Clear();

            listFiles.Clear();
            listView.Items.Clear();
            advancedDataGridView1.DataSource = null;
            advancedDataGridView1.Rows.Clear();
            preView.ImageLocation = "";
            txtPath.Text = "";
            fullPach = "";
            webBrowser.Navigate((Uri)null); ;
            borrarXml();
            tsbtnConfig.Enabled = true;
            tsbtnElim.Enabled = false;
            tsbtnBuscar.Enabled = false;
            //lbRutas.Text = "";
            listViewRutas.Clear();
            listViewRutas.Items.Add("Todos");
            todasLasRutas.Clear();
            table.Clear();
            xml = new XML();
            xml._crearXml();
            bindingSource1.DataSource = table;
            advancedDataGridView1.DataSource = bindingSource1;
            advancedDataGridView1.AutoGenerateColumns = true;
        }

      

        private void advancedDataGridView1_SortStringChanged(object sender, EventArgs e)
        {
            this.bindingSource1.Sort = this.advancedDataGridView1.SortString;
        }

        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)
        {
            this.bindingSource1.Filter = this.advancedDataGridView1.FilterString;
            
           // bindingSource1.ResetBindings(false);
        }

        private void advancedDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in advancedDataGridView1.SelectedRows)
            {
                // si el archivo es una imagen muestra una preview si no muestra un icono por defecto.
                string extencionArchivo = dr.Cells[1].Value.ToString().Substring((dr.Cells[1].Value.ToString().Length - 3), 3);
                if (extencionArchivo == "jpg" || extencionArchivo == "png")
                {
                    preView.ImageLocation = dr.Cells[0].Value.ToString();
                }
                else
                {
                    //obtengo la ruta donde se ejecuta el programa y acceso a la carpeta resources
                    string ruta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    ruta = ruta.Substring(0, (ruta.Length - 9));
                    ruta = ruta + "Resources\\icono.png";
                    preView.ImageLocation = ruta;
                }
            }
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if (listViewRutas.SelectedItems[0].Text == "Todos")
            {
                this.bindingSource1.RemoveFilter();
            }
            else { 
            //creo la consulta para el filtro
            string fil = "([Ruta] LIKE '%" + listViewRutas.SelectedItems[0].Text + "%')";
            fil = fil.Replace(@"\", @"[\]");
            //le paso la consulta al filtro
            this.bindingSource1.Filter = fil;

            //Cargo la vista
            CargarVistas(listViewRutas.SelectedItems[0].Text);
            }
        }

        private void cargarRutas(string ruta) {

            listViewRutas.Clear();
            listViewRutas.Items.Add("Todos");
            //lo hago cuando se carga un archivo XML
            if (ruta != "")
            {
                foreach (var item in xml.leerRuta(ruta))
                {
                    todasLasRutas.Add(item);
                    listViewRutas.Items.Add(item);   
                }
            }
            //Lo hago cuando NO se cargo un archivo XML
            else {
                foreach (var item in todasLasRutas)
                {
                    listViewRutas.Items.Add(item);
                }
            }
          
        }

        private void tsbtnBuscar_Click(object sender, EventArgs e)
        {
            CargaMultiple a = new CargaMultiple();
            a.ShowDialog();
            foreach (DataGridViewRow dr in advancedDataGridView1.SelectedRows)
            {
                dr.Cells[2].Value = a.tbCategoriaArchivos.Text;
                dr.Cells[3].Value = a.tbDescripciones.Text;
            }
        }

        private void tsbtnImpr_Click(object sender, EventArgs e)
        {

            //PrintDocument doc = new PrintDocument();
            //doc.DefaultPageSettings.Landscape = true;
            //doc.PrinterSettings.PrinterName = "Microsoft Print to PDF";

            //PrintPreviewDialog ppd = new PrintPreviewDialog { Document = doc };
            //((Form)ppd).WindowState = FormWindowState.Maximized;

            //doc.PrintPage += delegate (object ev, PrintPageEventArgs ep)
            //{
            //    const int DGV_ALTO = 35;
            //    int left = ep.MarginBounds.Left, top = ep.MarginBounds.Top;

                //foreach (DataGridViewColumn col in advancedDataGridView1.Columns)
                //{
                //    ep.Graphics.DrawString(col.HeaderText, new Font("Segoe UI", 16, FontStyle.Bold), Brushes.DeepSkyBlue, left, top);
                //    left += col.Width;

                //    if (col.Index < advancedDataGridView1.ColumnCount - 1)
                //        ep.Graphics.DrawLine(Pens.Gray, left - 5, top, left - 5, top + 43 + (advancedDataGridView1.RowCount - 1) * DGV_ALTO);
                //}
                //left = ep.MarginBounds.Left;
                //ep.Graphics.FillRectangle(Brushes.Black, left, top + 40, ep.MarginBounds.Right - left, 3);
                //top += 43;

            //    foreach (DataGridViewRow row in advancedDataGridView1.Rows)
            //    {
            //        if (row.Index == advancedDataGridView1.RowCount - 1) break;
            //        left = ep.MarginBounds.Left;
            //        foreach (DataGridViewCell cell in row.Cells)
            //        {
            //            ep.Graphics.DrawString(Convert.ToString(cell.Value), new Font("Segoe UI", 13), Brushes.Black, left + 100, top + 4);
            //            left += cell.OwningColumn.Width;
            //        }
            //        top += DGV_ALTO;
            //        ep.Graphics.DrawLine(Pens.Gray, ep.MarginBounds.Left, top, ep.MarginBounds.Right, top);
            //    }
            //};
            //ppd.ShowDialog();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            cargarCategoriaslb();
        }

        private void CargarVistas(string ruta) {

            //cargo los archivos para mostrarlos
            listFiles.Clear();
            listView.Items.Clear();
            try
            {
                webBrowser.Url = new Uri(ruta);
                foreach (string item in Directory.GetFiles(ruta))
                {
                    //Add image to imagelist
                    imageList.Images.Add(System.Drawing.Icon.ExtractAssociatedIcon(item));
                    FileInfo fi = new FileInfo(item);
                    listFiles.Add(fi.FullName);//Add file name to list
                                               //Add file name and image to listview
                    listView.Items.Add(fi.Name, imageList.Images.Count - 1);
                }

            }
            catch (Exception)
            {
                //   MessageBox.Show("!ERROR, No se pede acceder a la ruta guardada");
            }
            cargarCategoriaslb();

            bindingSource1.DataSource = table;
            advancedDataGridView1.DataSource = bindingSource1;
            advancedDataGridView1.AutoGenerateColumns = true;
        }

        public void cargarCarpetasArchivos(string patch) {
            try
            {
                //cargo todas las carpetas
                foreach (string item in Directory.GetDirectories(patch))
                {
                    //creo un fileInfo para poder acceder a los atributos de cada Archivo
                    FileInfo fi = new FileInfo(item);
                    //cargo las categorias al tablePag
                    cargarCategoriaslb();

                    //cargo la tabla
                    row = table.NewRow();
                    row["Ruta"] = fi.DirectoryName + "\\" + fi.Name;
                    row["NombreArchivo"] = fi.Name;
                    row["Categoria"] = "";
                    row["Descripcion"] = "";
                    row["Tipo"] = "Carpeta";
                    table.Rows.Add(row);
                    todasLasRutas.Add(item);
                    cargarRutas("");
                    cargarCarpetasArchivos(item);
                }

                //cargo todos los archivos de la ruta seleccionada al datagridView
                foreach (string item2 in Directory.GetFiles(patch))
                {
                    //creo un fileInfo para poder acceder a los atributos de cada Archivo
                    FileInfo fi2 = new FileInfo(item2);
                    //cargo las categorias al tablePag
                    cargarCategoriaslb();
                   // MessageBox.Show(fi2.DirectoryName + "\\" + fi2.Name);
                    //cargo la tabla
                    row = table.NewRow();
                    row["Ruta"] = fi2.DirectoryName + "\\" + fi2.Name;
                    row["NombreArchivo"] = fi2.Name;
                    row["Categoria"] = "";
                    row["Descripcion"] = "";
                    row["Tipo"] = "Archivo";
                    table.Rows.Add(row);
                }

            }
            catch
            {
                MessageBox.Show("Todos los archivos ya se encuentran cargados");
            }
        }

    }

       

    
}
