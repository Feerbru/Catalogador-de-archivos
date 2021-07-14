using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;  /* para acceder a la libreria xml*/

namespace ExploradorArchivo
{
    class XML
    {

        public XmlDocument doc = new XmlDocument();
        public string rutaXml;

        public void _crearXml()
        {
            string ruta = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"archivo.xml");
            this.rutaXml = ruta;
            

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);

            XmlNode root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);


            XmlNode element1 = doc.CreateElement("Categorias");
            doc.AppendChild(element1);
            doc.Save(ruta);

        }

        public void _Añadir(string nombreCategoria, string nombreArchivo, string descripcion, string rutaArchivo, string tipo)
        {
            string ruta = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"archivo.xml");
            this.rutaXml = ruta;
            doc.Load(rutaXml);

            XmlNode empleado = _Crear_Archivos(nombreCategoria, nombreArchivo, descripcion, rutaArchivo, tipo);

            XmlNode nodoRaiz = doc.DocumentElement;

            nodoRaiz.InsertAfter(empleado, nodoRaiz.LastChild);

            doc.Save(rutaXml);


        }

        private XmlNode _Crear_Archivos(string nombreCategoria, string nombreArchivo, string descripcion, string rutaArchivo, string tipo)
        {

            XmlNode elemento = doc.CreateElement("Elemento");


            XmlElement cat = doc.CreateElement("NombreCategoria");
            cat.InnerText = nombreCategoria;
            elemento.AppendChild(cat);


            XmlElement xnombre = doc.CreateElement("NombreArchivo");
            xnombre.InnerText = nombreArchivo;
            elemento.AppendChild(xnombre);

            XmlElement xdescripcion = doc.CreateElement("Descripcion");
            xdescripcion.InnerText = descripcion;
            elemento.AppendChild(xdescripcion);

            XmlElement xRutaArchivo = doc.CreateElement("RutaArchivo");
            xRutaArchivo.InnerText = rutaArchivo;
            elemento.AppendChild(xRutaArchivo);

            XmlElement xTipo = doc.CreateElement("Tipo");
            xTipo.InnerText = tipo;
            elemento.AppendChild(xTipo);

            return elemento;
        }

        public List<Elemento> _ReadXml(string ruta)
        {
            this.rutaXml = ruta;
            doc.Load(ruta);

            XmlNodeList listaArchivos = doc.SelectNodes("Categorias/Elemento");
            List<Elemento> archivosElementos = new List<Elemento>();
            XmlNode unElemento;

            for (int i = 0; i < listaArchivos.Count; i++)
            {

                unElemento = listaArchivos.Item(i);

                string nombreCategoria = unElemento.SelectSingleNode("NombreCategoria").InnerText;
                string nombreArchivo = unElemento.SelectSingleNode("NombreArchivo").InnerText;
                string descripcion = unElemento.SelectSingleNode("Descripcion").InnerText;
                string rutaArchivo = unElemento.SelectSingleNode("RutaArchivo").InnerText;
                string tipo = unElemento.SelectSingleNode("Tipo").InnerText;

                archivosElementos.Add(new Elemento(nombreArchivo, nombreCategoria, descripcion, rutaArchivo, tipo));
            }

            return archivosElementos;
        }

        public void _DeleteCategoria(string id_borrar)
        {
            string ruta = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"archivo.xml");
            this.rutaXml = ruta;
            doc.Load(rutaXml);

            XmlNode Eleentos = doc.DocumentElement;

            XmlNodeList listaEmpleados = doc.SelectNodes("Categorias/Elemento");

            foreach (XmlNode item in listaEmpleados)
            {

                if (item.SelectSingleNode("NombreCategoria").InnerText == id_borrar)
                {

                    XmlNode nodoOld = item;

                    Eleentos.RemoveChild(nodoOld);
                }
            }

            doc.Save(rutaXml);
        }

        public void _DeleteArcivo(string id_borrar)
        {
            string ruta = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"archivo.xml");
            this.rutaXml = ruta;
            doc.Load(rutaXml);

            XmlNode Eleentos = doc.DocumentElement;

            XmlNodeList listaEmpleados = doc.SelectNodes("Categorias/Elemento");

            foreach (XmlNode item in listaEmpleados)
            {

                if (item.SelectSingleNode("NombreArchivo").InnerText == id_borrar)
                {

                    XmlNode nodoOld = item;

                    Eleentos.RemoveChild(nodoOld);
                }
            }

            doc.Save(rutaXml);
        }
        public void _UpdateXml(string nombreCategoria, string nombreArchivo, string descripcion, string rutaArchivo, string tipo)
        {
            string ruta = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"archivo.xml");
            this.rutaXml = ruta;
            XmlElement elemento = doc.DocumentElement;

            XmlNodeList listaElementos = doc.SelectNodes("Categorias/Elemento");

            XmlNode nuevo_elemeto = _Crear_Archivos(nombreCategoria, nombreArchivo, descripcion, rutaArchivo, tipo);

            foreach (XmlNode item in listaElementos)
            {

                if (item.FirstChild.InnerText == nombreCategoria)
                {
                    XmlNode nodoOld = item;
                    elemento.ReplaceChild(nuevo_elemeto, nodoOld);
                }
            }

            doc.Save(rutaXml);
        }

        public void guardarComo(string ruta) {
            string ruta2 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"archivo2.xml");
   
            FileInfo fi = new FileInfo(rutaXml);
            fi.CopyTo(ruta, true); // existing file will be overwritten

        }

        public void _Añadir_ruta(string patch)
        {
            string ruta = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"archivo.xml");
            this.rutaXml = ruta;
            doc.Load(rutaXml);

            XmlNode elemento = doc.CreateElement("RutaCarpeta");


            XmlElement cat = doc.CreateElement("Ruta");
            cat.InnerText = patch;
            elemento.AppendChild(cat);

            XmlNode nodoRaiz = doc.DocumentElement;

            nodoRaiz.InsertAfter(elemento, nodoRaiz.LastChild);

            doc.Save(rutaXml);

        }

        public HashSet<string> leerRuta(string ruta)
        {
            this.rutaXml = ruta;
            doc.Load(ruta);
           
            XmlNodeList listaArchivos = doc.SelectNodes("Categorias/RutaCarpeta");
            
            XmlNode unElemento;
            HashSet<string> patch = new HashSet<string>();
            for (int i = 0; i < listaArchivos.Count; i++)
            {

                unElemento = listaArchivos.Item(i);

                patch.Add(unElemento.SelectSingleNode("Ruta").InnerText);
              
            }
            return patch;
        }


    }
}
