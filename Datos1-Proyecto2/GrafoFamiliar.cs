using System.Collections.Generic;

namespace Datos1_Proyecto2
{
    public class Nodo
    {
        public string Nombre { get; set; }
        public Nodo[] Conexiones { get; private set; }
        private int cantidadConexiones;

        public Nodo(string nombre)
        {
            Nombre = nombre;
            Conexiones = new Nodo[4]; // Capacidad inicial
            cantidadConexiones = 0;
        }

        public void AgregarConexion(Nodo otro)
        {
            if (!ExisteConexion(otro))
            {
                if (cantidadConexiones == Conexiones.Length)
                {
                    // Si el array está lleno, se duplica el tamaño
                    var nuevoArray = new Nodo[Conexiones.Length * 2];
                    for (int i = 0; i < Conexiones.Length; i++)
                        nuevoArray[i] = Conexiones[i];
                    Conexiones = nuevoArray;
                }
                Conexiones[cantidadConexiones++] = otro;
            }
        }

        public bool ExisteConexion(Nodo otro)
        {
            for (int i = 0; i < cantidadConexiones; i++)
            {
                if (Conexiones[i] == otro)
                    return true;
            }
            return false;
        }
    }

    public class GrafoFamiliar
    {
        private Nodo[] nodos;
        private int cantidadNodos;

        public GrafoFamiliar()
        {
            nodos = new Nodo[8]; // Capacidad inicial, puedes cambiarla
            cantidadNodos = 0;
        }

        public void AgregarPersona(string nombre)
        {
            if (BuscarPersona(nombre) == null)
            {
                if (cantidadNodos == nodos.Length)
                {
                    // Redimensionar el array si es necesario
                    var nuevoArray = new Nodo[nodos.Length * 2];
                    for (int i = 0; i < nodos.Length; i++)
                        nuevoArray[i] = nodos[i];
                    nodos = nuevoArray;
                }
                nodos[cantidadNodos++] = new Nodo(nombre);
            }
        }

        public void AgregarConexion(string nombre1, string nombre2)
        {
            var nodo1 = BuscarPersona(nombre1);
            var nodo2 = BuscarPersona(nombre2);

            if (nodo1 == null)
            {
                AgregarPersona(nombre1);
                nodo1 = BuscarPersona(nombre1);
            }
            if (nodo2 == null)
            {
                AgregarPersona(nombre2);
                nodo2 = BuscarPersona(nombre2);
            }

            nodo1.AgregarConexion(nodo2);
            // Si quieres que la conexión sea bidireccional, descomenta la siguiente línea:
            // nodo2.AgregarConexion(nodo1);
        }

        public Nodo? BuscarPersona(string nombre)
        {
            for (int i = 0; i < cantidadNodos; i++)
            {
                if (nodos[i].Nombre == nombre)
                    return nodos[i];
            }
            return null;
        }

        public Nodo[] ObtenerPersonas()
        {
            var resultado = new Nodo[cantidadNodos];
            for (int i = 0; i < cantidadNodos; i++)
                resultado[i] = nodos[i];
            return resultado;
        }
    }
}