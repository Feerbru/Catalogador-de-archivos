using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace ExploradorArchivo.Controller.Botones
{
    class Menu
    {
        public string CrearCarpeta(string nom)
        {
            if (nom != "")
            {
                string folderPath = @"D:\" + nom;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    MessageBox.Show("Creado con exito");
                    return folderPath;

                }
                else
                {
                    MessageBox.Show("No se puede crear ya exite un nombre igual");
                    return "No se puede crear ya exite un nombre igual";
                }

            }
            else
            {
                MessageBox.Show("No se puede crear, debe colocar un nombre a la Carpeta");
                return "No se puede crear ya exite un nombre igual";
            }
            
        }
       
    }
}
