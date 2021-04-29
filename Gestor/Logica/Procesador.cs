
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
		private readonly ListBox RegistroIPs;
		private readonly ListBox RegistroComandos;

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
					Procesar_IntentarIniciarSesion(
						new Comando_IntentarIniciarSesion(ComandoString), IP);

					break;
				}

				case TiposComando.CerrarSesion:
				{
					Procesar_CerrarSesion(
						new Comando_CerrarSesion(ComandoString));

					break;
				}

				//case TiposComando.XXXXX:
				//{
				//	Procesar_XXXXX(
				//		new Comando_XXXXX(ComandoString));

				//	break;
				//}
			}
		}

		private static void Procesar_IntentarIniciarSesion(Comando_IntentarIniciarSesion Comando, string IP)
		{
			var nombresUsuarios = Global.Usuarios
								  .Select(u => u.Nombre);

			if(nombresUsuarios.Contains(Comando.Usuario))
			{
				var usuario = Global.Usuarios
						   	  .Where(u => u.Nombre.Equals(Comando.Usuario))
							  .Select(u => u)
							  .First();

				if(!usuario.Conectado)
				{
					if(usuario.Contrasena.Equals(Comando.Contrasena))
					{
						usuario.IP = IP;
						usuario.Conectado = true;

						new Comando_ResultadoIntentoIniciarSesion
						(
							ResultadosIntentoIniciarSesion.Correcto,
							usuario.Rol
						)
						.Enviar(IP);
					}
					else
					{
						new Comando_ResultadoIntentoIniciarSesion
						(
							ResultadosIntentoIniciarSesion.ContrasenaIncorrecta,
							Roles.Ninguno
						)
						.Enviar(IP);
					}
				}
				else
				{
					new Comando_ResultadoIntentoIniciarSesion
					(
						ResultadosIntentoIniciarSesion.UsuarioYaConectado,
						Roles.Ninguno
					)
					.Enviar(IP);
				}	
			}
			else
			{
				new Comando_ResultadoIntentoIniciarSesion
				(
					ResultadosIntentoIniciarSesion.UsuarioNoExiste,
					Roles.Ninguno
				)
				.Enviar(IP);
			}
		}

		private static void Procesar_CerrarSesion(Comando_CerrarSesion Comando)
		{
			var usuario = Global.Usuarios
						 .Where(u => u.Nombre.Equals(Comando.Usuario))
						 .Select(u => u)
						 .First();

			usuario.Conectado = false;
		}

		//private static void Procesar_XXXXX(Comando_XXXXX Comando)
		//{
		//	//
		//}
	}
}
