
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PFG.Comun;

namespace PFG.Gestor
{
	public partial class Principal : Form
	{
		private const string COMENZAR_JORNADA_TEXT = "Comenzar Jornada";
		private static readonly Color COMENZAR_JORNADA_FORECOLOR = Color.FromArgb(  0,   0,   0);
		private static readonly Color COMENZAR_JORNADA_BACKCOLOR = Color.FromArgb(  0, 200,   0);

		private const string TERMINAR_JORNADA_TEXT = "Terminar Jornada";
		private static readonly Color TERMINAR_JORNADA_FORECOLOR = Color.FromArgb(255, 255, 255);
		private static readonly Color TERMINAR_JORNADA_BACKCOLOR = Color.FromArgb(255,   0,   0);

		private ControladorRed Servidor;
		private ProcesadorGestor ProcesadorMensajesRecibidos;

		public Principal()
		{
			InitializeComponent();
		}

		private void Principal_Load(object sender, EventArgs e)
		{
			GestionUsuarios.Cargar();
			GestionMesas.Cargar();
			GestionArticulos.Cargar();

			string servidorIP = Comun.Global.Get_MiIP_Windows();

			ProcesadorMensajesRecibidos = new();
			Servidor = new(servidorIP, ProcesadorMensajesRecibidos.Procesar, true);

			IPGestor.Text = servidorIP;
		}

		private void ComenzarTerminarJornada_Click(object sender, EventArgs e)
		{
			if(Global.JornadaEnCurso) // Terminar Jornada
			{
				Global.JornadaEnCurso = false;

				ComenzarTerminarJornada.Text = COMENZAR_JORNADA_TEXT;
				ComenzarTerminarJornada.ForeColor = COMENZAR_JORNADA_FORECOLOR;
				ComenzarTerminarJornada.BackColor = COMENZAR_JORNADA_BACKCOLOR;

				GestionMesas.Mesas.ForEach(m => m.EstadoMesa = EstadosMesa.Vacia);
				GestionTareas.Tareas.Clear();
			}
			else // Comenzar Jornada
			{
				Global.JornadaEnCurso = true;

				ComenzarTerminarJornada.Text = TERMINAR_JORNADA_TEXT;
				ComenzarTerminarJornada.ForeColor = TERMINAR_JORNADA_FORECOLOR;
				ComenzarTerminarJornada.BackColor = TERMINAR_JORNADA_BACKCOLOR;
			}
		}

		private void Salir_Click(object sender, EventArgs e)
		{
			if(Global.JornadaEnCurso)
			{
				MessageBox.Show
				(
					"No se puede cerrar el Gestor mientras la Jornada está en curso",
					"Alerta",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
			}
			else
			{
				Servidor.Cerrar();
				CerrarTodasLasSesiones();
				GuardarDatos();
				/*Form*/ Close();
			}
		}

		private void CerrarTodasLasSesiones()
		{
			GestionUsuarios.Usuarios
				.ForEach(u => 
					u.Conectado = false);
		}

		private void GuardarDatos()
		{
			GestionUsuarios.Guardar();
			GestionMesas.Guardar();
			GestionArticulos.Guardar();
		}
	}
}
