
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PFG.Comun;

namespace PFG.Gestor
{
	public class Procesador
	{
		private ListBox RegistroIPs;
		private ListBox RegistroComandos;

		public Procesador(ListBox RegistroIPs, ListBox RegistroComandos)
		{
			this.RegistroIPs = RegistroIPs;
			this.RegistroComandos = RegistroComandos;
		}

		public void Procesar(string IP, string ComandoString)
		{
			// TODO - Procesar (Gestor)

			RegistroIPs.Invoke(new Action(() =>
			{ 
				RegistroIPs.Items.Add(IP);
				RegistroComandos.Items.Add(ComandoString);
			}));

			var parametrosComando = ComandoString.Split(',');
			var tipoComando = (TiposComando)Enum.Parse(typeof(TiposComando), parametrosComando[0]);

			switch(tipoComando)
			{
				case TiposComando.IntentarIniciarSesion:
				{
					var comando = new Comando_IntentarIniciarSesion(ComandoString);

					// TODO - Autenticación

					ControladorRed.Enviar(IP, Comando_ResultadoIntentoIniciarSesion.ParametrosToString
					(
						ResultadosIntentoIniciarSesion.Correcto,
						Roles.Administrador
					));

					break;
				}

				//case TiposComando.XXXXX:
				//{
				//	var comando = new Comando_XXXXX(ComandoString);

				//	//

				//	break;
				//}
			}
		}
	}
}
