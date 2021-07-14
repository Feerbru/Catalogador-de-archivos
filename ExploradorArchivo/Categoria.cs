using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExploradorArchivo
{
    public partial class Categoria : Form
    {
        public Categoria()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (tbNombreArchivo.Text.ToString() == "" || tbCategoria.Text.ToString() == "")
            {
                MessageBox.Show("Por favor complete los campos Nombre Archivo y Categoria");
            }
            else {
                this.Visible = false;
            }  
        }

        internal void Load(ListViewItem item)
        {
            MessageBox.Show(item.Text);
        }
    }
}
