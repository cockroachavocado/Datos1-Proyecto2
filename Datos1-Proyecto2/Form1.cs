using System;
using System.Drawing;
using System.Windows.Forms;

namespace Datos1_Proyecto2
{
    public partial class Form1 : Form
    {
        private NodoArbol[] nodos;
        private int cantidadNodos = 4;

        // Controles para la interfaz
        private TextBox txtNombre;
        private ComboBox cmbPadre1;
        private ComboBox cmbPadre2;
        private Button btnAgregar;
        private Button btnZoomMas;
        private Button btnZoomMenos;
        private Panel panelGrafo;

        private float zoom = 1.0f;

        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(20, 24, 34);
            this.DoubleBuffered = true;

            nodos = new NodoArbol[50]; // Capacidad máxima de nodos
            nodos[0] = new NodoArbol("Minion1", 150, 30, 1);
            nodos[1] = new NodoArbol("Minion2", 150, 130, 2);
            nodos[2] = new NodoArbol("Mike", 50, 250, 0);
            nodos[3] = new NodoArbol("Lisa", 250, 250, 0);

            nodos[0].Hijos[0] = nodos[1];
            nodos[1].Hijos[0] = nodos[2];
            nodos[1].Hijos[1] = nodos[3];

            InicializarControles();

            // Panel para el grafo sin scroll
            panelGrafo = new Panel
            {
                Location = new Point(0, 50),
                Size = new Size(800, 600),
                BackColor = Color.FromArgb(20, 24, 34)
            };
            panelGrafo.Paint += PanelGrafo_Paint;
            Controls.Add(panelGrafo);

            // Botones de zoom
            btnZoomMas = new Button { Location = new Point(500, 10), Width = 40, Height = 40, Text = "+", ForeColor = Color.White };
            btnZoomMenos = new Button { Location = new Point(550, 10), Width = 40, Height = 40, Text = "-", ForeColor = Color.White};
            btnZoomMas.Click += (s, e) => { zoom += 0.1f; panelGrafo.Invalidate(); };
            btnZoomMenos.Click += (s, e) => { if (zoom > 0.2f) zoom -= 0.1f; panelGrafo.Invalidate(); };
            Controls.Add(btnZoomMas);
            Controls.Add(btnZoomMenos);
        }

        private void InicializarControles()
        {
            txtNombre = new TextBox { Location = new Point(10, 10), Width = 120, PlaceholderText = "Nombre" };
            cmbPadre1 = new ComboBox { Location = new Point(140, 10), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPadre2 = new ComboBox { Location = new Point(270, 10), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            btnAgregar = new Button { Location = new Point(400, 10), Width = 80, Height = 40, Text = "Agregar", ForeColor = Color.White};

            ActualizarComboPadres();

            btnAgregar.Click += BtnAgregar_Click;

            Controls.Add(txtNombre);
            Controls.Add(cmbPadre1);
            Controls.Add(cmbPadre2);
            Controls.Add(btnAgregar);
        }

        private void ActualizarComboPadres()
        {
            cmbPadre1.Items.Clear();
            cmbPadre2.Items.Clear();
            cmbPadre1.Items.Add("Padre");
            cmbPadre2.Items.Add("Madre");
            for (int i = 0; i < cantidadNodos; i++)
            {
                cmbPadre1.Items.Add(nodos[i].Nombre);
                cmbPadre2.Items.Add(nodos[i].Nombre);
            }
            cmbPadre1.SelectedIndex = 0;
            cmbPadre2.SelectedIndex = 0;
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("Ingrese el nombre del nodo.");
                return;
            }

            int idxPadre1 = cmbPadre1.SelectedIndex - 1;
            int idxPadre2 = cmbPadre2.SelectedIndex - 1;

            // Validación: al menos uno de los padres debe estar seleccionado
            if (idxPadre1 < 0 && idxPadre2 < 0)
            {
                MessageBox.Show("Debe seleccionar al menos un padre o madre.");
                return;
            }

            // Posición automática (puedes mejorar el algoritmo de posicionamiento)
            int x = 50 + (cantidadNodos % 5) * 100;
            int y = 350 + (cantidadNodos / 5) * 100;

            NodoArbol nuevoNodo = new NodoArbol(nombre, x, y, 0);

            nodos[cantidadNodos] = nuevoNodo;

            // Conectar padres seleccionados
            if (idxPadre1 >= 0)
                AgregarHijo(nodos[idxPadre1], nuevoNodo);

            if (idxPadre2 >= 0 && idxPadre2 != idxPadre1)
                AgregarHijo(nodos[idxPadre2], nuevoNodo);

            cantidadNodos++;
            ActualizarComboPadres();
            txtNombre.Text = "";
            cmbPadre1.SelectedIndex = 0;
            cmbPadre2.SelectedIndex = 0;
            panelGrafo.Invalidate(); // Redibuja el panel
        }

        private void AgregarHijo(NodoArbol padre, NodoArbol hijo)
        {
            int nuevoTam = padre.Hijos.Length + 1;
            NodoArbol[] nuevosHijos = new NodoArbol[nuevoTam];
            for (int i = 0; i < padre.Hijos.Length; i++)
                nuevosHijos[i] = padre.Hijos[i];
            nuevosHijos[nuevoTam - 1] = hijo;
            padre.Hijos = nuevosHijos;
        }

        private void PanelGrafo_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.ScaleTransform(zoom, zoom);

            using (var lapiz = new Pen(Color.Cyan, 2))
            {
                for (int i = 0; i < cantidadNodos; i++)
                {
                    var nodo = nodos[i];
                    for (int j = 0; j < nodo.Hijos.Length; j++)
                    {
                        var hijo = nodo.Hijos[j];
                        if (hijo != null)
                        {
                            var centroPadre = new Point(nodo.X, nodo.Y + 40);
                            var centroHijo = new Point(hijo.X, hijo.Y + 40);
                            e.Graphics.DrawLine(lapiz, centroPadre, centroHijo);
                        }
                    }
                }
            }

            for (int i = 0; i < cantidadNodos; i++)
            {
                var nodo = nodos[i];
                DibujarNodo(e.Graphics, nodo.Nombre, nodo.X, nodo.Y);
            }
        }

        private void DibujarNodo(Graphics g, string texto, int x, int y)
        {
            int radio = 80;
            var rect = new Rectangle(x - radio / 2, y, radio, radio);

            using (var lapiz = new Pen(Color.Cyan, 2))
            using (var brocha = new SolidBrush(Color.FromArgb(20, 24, 34)))
            using (var brochaTexto = new SolidBrush(Color.White))
            using (var formato = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.FillEllipse(brocha, rect);
                g.DrawEllipse(lapiz, rect);
                g.DrawString(texto, new Font("Segoe UI", 10, FontStyle.Regular), brochaTexto, rect, formato);
            }
        }
    }

    public class NodoArbol
    {
        public string Nombre { get; }
        public int X { get; }
        public int Y { get; }
        public NodoArbol[] Hijos { get; set; }

        public NodoArbol(string nombre, int x, int y, int cantidadHijos)
        {
            Nombre = nombre;
            X = x;
            Y = y;
            Hijos = new NodoArbol[cantidadHijos];
        }
    }
}
