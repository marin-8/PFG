
using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

using PFG.Comun;

namespace PFG.Gestor
{
	public partial class Principal : Form
	{
	// ============================================================================================== //

		// Constantes

		private const string COMENZAR_JORNADA_TEXT = "Comenzar Jornada";
		private static readonly Color COMENZAR_JORNADA_FORECOLOR = Color.FromArgb(  0,   0,   0);
		private static readonly Color COMENZAR_JORNADA_BACKCOLOR = Color.FromArgb(  0, 200,   0);

		private const string TERMINAR_JORNADA_TEXT = "Terminar Jornada";
		private static readonly Color TERMINAR_JORNADA_FORECOLOR = Color.FromArgb(255, 255, 255);
		private static readonly Color TERMINAR_JORNADA_BACKCOLOR = Color.FromArgb(255,   0,   0);

	// ============================================================================================== //

		// Variables

		private ControladorRed Servidor;

	// ============================================================================================== //

		// Inicialización

		public Principal()
		{
			InitializeComponent();
		}

		private void Principal_Load(object sender, EventArgs e)
		{
			bool ultimoCierreCorrecto;

			GestionUsuarios.Cargar();
			GestionMesas.Cargar();
			GestionArticulos.Cargar();
			ultimoCierreCorrecto = GestionTareas.Cargar();
			GestionTareas.ComenzarGuardadoAutomatico();

			string servidorIP = Comun.Global.Get_MiIP_Windows();

			Servidor = new(servidorIP, ProcesadorGestor.ProcesarComandosRecibidos, true);

			ProcesadorGestor.EmpezarAComprobarConectados();

			IPGestor.Text = servidorIP;

			if(!ultimoCierreCorrecto)
			{
				ComenzarTerminarJornada.PerformClick();

				MessageBox.Show
				(
					"La última vez que se cerró el Gestor no se hizo correctamente.\n\n" +
					"Se han cargado y desasignado las tareas guardadas y se ha reanudado la jornada. " +
					"Cuando se conecte el primer usuario, se le asignarán todas las tareas. " +
					"Éste tendrá que reasignarlas manualmente (recomendable hacerlo una vez " +
					"se hayan conectado el resto de usuarios).",
					"Alerta",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
			}
		}

	// ============================================================================================== //

		// Eventos UI

		private void ComenzarTerminarJornada_Click(object sender, EventArgs e)
		{
			if(Global.JornadaEnCurso) // Terminar Jornada
			{
				CerrarTodasLasSesiones();
				VaciarMesas();
				GestionTareas.ResetearTareas();
			}
			else // Comenzar Jornada
			{
				// (?)
			}

			Global.JornadaEnCurso = !Global.JornadaEnCurso;

			ActualizarEstiloBotonComenzarTerminarJornada();
		}

		private void Salir_Click(object sender, EventArgs e)
		{
			if(Global.JornadaEnCurso)
			{
				MessageBox.Show
				(
					"No se puede cerrar el Gestor\nmientras la Jornada está en curso",
					"Alerta",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
			}
			else
			{
				ProcesadorGestor.PararDeComprobarConectados();
				Servidor.PararEscucha();
				CerrarSesionAdmin();
				GuardarDatos();
				/*Form*/ Close();
			}
		}

	// ============================================================================================== //

		// Métodos Helper

		private void ActualizarEstiloBotonComenzarTerminarJornada()
		{
			ComenzarTerminarJornada.Text = Global.JornadaEnCurso ? TERMINAR_JORNADA_TEXT : COMENZAR_JORNADA_TEXT;
			ComenzarTerminarJornada.ForeColor = Global.JornadaEnCurso ? TERMINAR_JORNADA_FORECOLOR : COMENZAR_JORNADA_FORECOLOR;
			ComenzarTerminarJornada.BackColor = Global.JornadaEnCurso ? TERMINAR_JORNADA_BACKCOLOR : COMENZAR_JORNADA_BACKCOLOR;
		}

		private static void CerrarTodasLasSesiones()
		{
			GestionUsuarios.Usuarios.ForEach(async (u) =>
			{
				if(u.Conectado)
				{
					await Task.Run(() => {
						new Comando_JornadaTerminada().Enviar(u.IP); });

					u.Conectado = false;
				}
			});
		}

		private static void VaciarMesas()
		{
			GestionMesas.Mesas.ForEach(m => m.EstadoMesa = EstadosMesa.Vacia);
		}

		private static async void CerrarSesionAdmin()
		{
			var admin =
				GestionUsuarios.Usuarios
					.First(u => u.Rol == Roles.Administrador);

			if(admin.Conectado)
			{
				await Task.Run(() => {
					new Comando_JornadaTerminada().Enviar(admin.IP); });

				admin.Conectado = false;
			}
		}

		private static void GuardarDatos()
		{
			GestionUsuarios.Guardar();
			GestionMesas.Guardar();
			GestionArticulos.Guardar();
			GestionTareas.PararGuardadoAutomatico();
			GestionTareas.Guardar();
		}

		// ============================================================================================== //
	}
}
