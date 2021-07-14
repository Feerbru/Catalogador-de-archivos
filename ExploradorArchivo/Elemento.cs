using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploradorArchivo
{
    class Elemento
    {
        private string nombreArchivo;
        private string categoria;
        private string descrpcion;
        private string rutaArchivo;
        private string tipo;

        public Elemento(string nombreArchivo, string categoria, string descrpcion, string rutaArchivo, string tipo)
        {
            this.nombreArchivo = nombreArchivo;
            this.categoria = categoria;
            this.descrpcion = descrpcion;
            this.rutaArchivo = rutaArchivo;
            this.Tipo = tipo;
        }



        public string NombreArchivo { get => nombreArchivo; set => nombreArchivo = value; }
        public string Categoria { get => categoria; set => categoria = value; }
        public string Descrpcion { get => descrpcion; set => descrpcion = value; }
        public string RutaArchivo { get => rutaArchivo; set => rutaArchivo = value; }
        public string Tipo { get => tipo; set => tipo = value; }
    }
}
