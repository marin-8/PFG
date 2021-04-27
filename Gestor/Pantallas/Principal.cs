
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
		private const int MAX_CARACTERES_REGISTRO = 62;

		private ControladorRed Servidor;
		private Procesador ProcesadorMensajesRecibidos;

		public Principal()
		{
			InitializeComponent();

			FormClosing += new FormClosingEventHandler(Principal_Closing);
		}

		private void Principal_Load(object sender, EventArgs e)
		{
			ProcesadorMensajesRecibidos = new(RegistroIPs, RegistroComandos);

			string servidorIP = Comun.Global.Get_MiIP_Windows();

			Servidor = new(servidorIP, ProcesadorMensajesRecibidos.Procesar, true);

			IPGestor.Text = servidorIP;
		}

		private void Principal_Closing(object sender, EventArgs e)
		{
			Servidor.Cerrar();
		}

		private void CerrarYSalir_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
