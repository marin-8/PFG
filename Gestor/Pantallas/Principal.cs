
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
		private ControladorRed Servidor;
		private ProcesadorGestor ProcesadorMensajesRecibidos;

		public Principal()
		{
			InitializeComponent();

			FormClosing += new FormClosingEventHandler(Principal_Closing);

			void SincronizarScrolling(object s, EventArgs e)
			{
				if (s == RegistroIPs)
					RegistroComandos.TopIndex = RegistroIPs.TopIndex;
				if (s == RegistroComandos)
					RegistroIPs.TopIndex = RegistroComandos.TopIndex;
			}

			RegistroIPs.MouseCaptureChanged += SincronizarScrolling;
			RegistroComandos.MouseCaptureChanged += SincronizarScrolling;
			RegistroIPs.SelectedIndexChanged += SincronizarScrolling;
			RegistroComandos.SelectedIndexChanged += SincronizarScrolling;
		}

		private void Principal_Load(object sender, EventArgs e)
		{
			GestionUsuarios.Cargar();
			GestionMesas.Cargar();
			GestionArticulos.Cargar();

			ProcesadorMensajesRecibidos = new(RegistroIPs, RegistroComandos);

			string servidorIP = Comun.Global.Get_MiIP_Windows();

			Servidor = new(servidorIP, ProcesadorMensajesRecibidos.Procesar, true);

			IPGestor.Text = servidorIP;
		}

		private void Principal_Closing(object sender, EventArgs e)
		{
			Servidor.Cerrar();

			GestionUsuarios.Guardar();
			GestionMesas.Guardar();
			GestionArticulos.Guardar();
		}

		private void CerrarYSalir_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void CerrarSesionDesarrollador_Click(object sender, EventArgs e)
		{
			var usuarioDesarrollador = GestionUsuarios.Usuarios
									  .Where(u => u.Rol == Roles.Desarrollador)
									  .First();

			if(usuarioDesarrollador.Conectado)
			{
				usuarioDesarrollador.Conectado = false;

				RegistroIPs.Items.Add("Gestor");
				RegistroComandos.Items.Add("Cerrada sesión de Desarrollador");
			}
		}
	}
}
