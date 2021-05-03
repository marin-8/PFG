
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

		public void Procesar(string IP, string ComandoJson)
		{
			RegistroIPs.Invoke(new Action(() =>
			{ 
				RegistroIPs.Items.Add(IP);
				RegistroComandos.Items.Add(ComandoJson);
			}));

			var tipoComando = Comando.Get_TipoComando_De_Json(ComandoJson);

			switch(tipoComando)
			{
				case TiposComando.IntentarIniciarSesion:
				{
					Procesar_IntentarIniciarSesion(
						Comando.DeJson
							<Comando_IntentarIniciarSesion>
								(ComandoJson), IP);

					break;
				}

				case TiposComando.CerrarSesion:
				{
					Procesar_CerrarSesion(
						Comando.DeJson
							<Comando_CerrarSesion>
								(ComandoJson));

					break;
				}

				case TiposComando.PedirUsuarios:
				{
					Procesar_PedirUsuarios(IP);

					break;
				}

				case TiposComando.IntentarCrearUsuario:
				{
					Procesar_IntentarCrearUsuario(
						Comando.DeJson
							<Comando_IntentarCrearUsuario>
								(ComandoJson), IP);

					break;
				}

				case TiposComando.ModificarUsuarioNombre:
				{
					Procesar_ModificarUsuarioNombre(
						Comando.DeJson
							<Comando_ModificarUsuarioNombre>
								(ComandoJson));

					break;
				}

				case TiposComando.ModificarUsuarioNombreUsuario:
				{
					Procesar_ModificarUsuarioNombreUsuario(
						Comando.DeJson
							<Comando_ModificarUsuarioNombreUsuario>
								(ComandoJson));

					break;
				}

				case TiposComando.ModificarUsuarioContrasena:
				{
					Procesar_ModificarUsuarioContrasena(
						Comando.DeJson
							<Comando_ModificarUsuarioContrasena>
								(ComandoJson));

					break;
				}

				case TiposComando.ModificarUsuarioRol:
				{
					Procesar_ModificarUsuarioRol(
						Comando.DeJson
							<Comando_ModificarUsuarioRol>
								(ComandoJson));

					break;
				}

				case TiposComando.IntentarEliminarUsuario:
				{
					Procesar_IntentarEliminarUsuario(
						Comando.DeJson
							<Comando_IntentarEliminarUsuario>
								(ComandoJson), IP);

					break;
				}

				case TiposComando.PedirMesas:
				{
					Procesar_PedirMesas(IP);

					break;
				}

				case TiposComando.IntentarEditarMapaMesas:
				{
					Procesar_IntentarEditarMapaMesas(
						Comando.DeJson
							<Comando_IntentarEditarMapaMesas>
								(ComandoJson), IP);

					break;
				}

				case TiposComando.IntentarCrearMesa:
				{
					Procesar_IntentarCrearMesa(
						Comando.DeJson
							<Comando_IntentarCrearMesa>
								(ComandoJson), IP);

					break;
				}

				//case TiposComando.XXXXX:
				//{
				//	Procesar_XXXXX(
				//		Comando.DeJson
				//			<Comando_XXXXX>
				//				(ComandoJson));

				//	break;
				//}
			}
		}

		private static void Procesar_IntentarIniciarSesion(Comando_IntentarIniciarSesion Comando, string IP)
		{
			ResultadosIntentoIniciarSesion resultado;
			Usuario usuario = null;

			var nombresUsuarios = GestionUsuarios.Usuarios
								  .Select(u => u.NombreUsuario);

			if(nombresUsuarios.Contains(Comando.Usuario))
			{
				usuario = GestionUsuarios.Usuarios
						   	  .Where(u => u.NombreUsuario.Equals(Comando.Usuario))
							  .Select(u => u)
							  .First();

				if(!usuario.Conectado)
				{
					if(usuario.Contrasena.Equals(Comando.Contrasena))
					{
						usuario.IP = IP;
						usuario.Conectado = true;

						resultado = ResultadosIntentoIniciarSesion.Correcto;
					}
					else
					{
						resultado = ResultadosIntentoIniciarSesion.ContrasenaIncorrecta;
					}
				}
				else
				{
					resultado = ResultadosIntentoIniciarSesion.UsuarioYaConectado;
				}	
			}
			else
			{
				resultado = ResultadosIntentoIniciarSesion.UsuarioNoExiste;
			}

			new Comando_ResultadoIntentoIniciarSesion
			(
				resultado,
				usuario
			)
			.Enviar(IP);
		}

		private static void Procesar_CerrarSesion(Comando_CerrarSesion Comando)
		{
			var usuario = GestionUsuarios.Usuarios
							 .Where(u => u.NombreUsuario.Equals(Comando.Usuario))
							 .First();

			usuario.Conectado = false;
		}

		private static void Procesar_PedirUsuarios(string IP)
		{
			new Comando_MandarUsuarios
			(
				GestionUsuarios.Usuarios
					.Where(u => u.Rol != Roles.Desarrollador)
					.OrderByDescending(u => (byte)u.Rol)
					.ThenBy(u => u.Nombre)
					.ToArray()
			)
			.Enviar(IP);
		}

		private static void Procesar_IntentarCrearUsuario(Comando_IntentarCrearUsuario Comando, string IP)
		{
			ResultadosIntentoCrearUsuario resultado;

			if(GestionUsuarios.Usuarios.Select(u => u.NombreUsuario).Contains(Comando.NuevoUsuario.NombreUsuario))
			{
				resultado = ResultadosIntentoCrearUsuario.UsuarioYaExiste;
			}
			else
			{
				GestionUsuarios.Usuarios.Add(Comando.NuevoUsuario);
				resultado = ResultadosIntentoCrearUsuario.Correcto;
			}

			new Comando_ResultadoIntentoCrearUsuario
			(
				resultado
			)
			.Enviar(IP);
		}

		private static void Procesar_ModificarUsuarioNombre(Comando_ModificarUsuarioNombre Comando)
		{
			GestionUsuarios.Usuarios
				.Where(u => u.NombreUsuario.Equals(Comando.Usuario))
				.First()
					.Nombre = Comando.NuevoNombre;
		}

		private static void Procesar_ModificarUsuarioNombreUsuario(Comando_ModificarUsuarioNombreUsuario Comando)
		{
			GestionUsuarios.Usuarios
				.Where(u => u.NombreUsuario.Equals(Comando.Usuario))
				.First()
					.NombreUsuario = Comando.NuevoNombreUsuario;
		}

		private static void Procesar_ModificarUsuarioContrasena(Comando_ModificarUsuarioContrasena Comando)
		{
			GestionUsuarios.Usuarios
				.Where(u => u.NombreUsuario.Equals(Comando.Usuario))
				.First()
					.Contrasena = Comando.NuevaContrasena;
		}

		private static void Procesar_ModificarUsuarioRol(Comando_ModificarUsuarioRol Comando)
		{
			GestionUsuarios.Usuarios
				.Where(u => u.NombreUsuario.Equals(Comando.Usuario))
				.First()
					.Rol = Comando.NuevoRol;
		}

		private static void Procesar_IntentarEliminarUsuario(Comando_IntentarEliminarUsuario Comando, string IP)
		{
			ResultadosIntentoEliminarUsuario resultado;

			var usuarioAEliminar =
				GestionUsuarios.Usuarios
					.Where(u => u.NombreUsuario.Equals(Comando.Usuario))
					.First();

			if(usuarioAEliminar.Conectado)
			{
				resultado = ResultadosIntentoEliminarUsuario.UsuarioConectado;
			}
			else
			{
				GestionUsuarios.Usuarios.Remove(usuarioAEliminar);
				resultado = ResultadosIntentoEliminarUsuario.Correcto;
			}

			new Comando_ResultadoIntentoEliminarUsuario(resultado).Enviar(IP);
		}

		private static void Procesar_PedirMesas(string IP)
		{
			new Comando_MandarMesas
			(
				GestionMesas.AnchoGrid,
				GestionMesas.AltoGrid,
				GestionMesas.Mesas.ToArray()
			)
			.Enviar(IP);
		}

		private static void Procesar_IntentarEditarMapaMesas(Comando_IntentarEditarMapaMesas Comando, string IP)
		{
			ResultadosIntentoEditarMapaMesas resultado = ResultadosIntentoEditarMapaMesas.Correcto;

			switch(Comando.TipoEdicionMapaMesas)
			{
				case TiposEdicionMapaMesas.AnadirColumna:
				{					
					if(GestionMesas.AnchoGrid == Comun.Global.MAXIMO_COLUMNAS_MESAS)
						resultado = ResultadosIntentoEditarMapaMesas.MaximoColumnas;

					else GestionMesas.AnchoGrid++;

					break;
				}
				case TiposEdicionMapaMesas.QuitarColumna:
				{
					if(GestionMesas.AnchoGrid == GestionMesas.MINIMO_COLUMNAS)
						resultado = ResultadosIntentoEditarMapaMesas.MinimoColumnas;

					else GestionMesas.AnchoGrid--;

					break;
				}
				case TiposEdicionMapaMesas.AnadirFila:
				{
					if(GestionMesas.AltoGrid == Comun.Global.MAXIMO_FILAS_MESAS)
						resultado = ResultadosIntentoEditarMapaMesas.MaximoFilas;

					else GestionMesas.AltoGrid++;

					break;
				}
				case TiposEdicionMapaMesas.QuitarFila:
				{
					if(GestionMesas.AltoGrid == GestionMesas.MINIMO_FILAS)
						resultado = ResultadosIntentoEditarMapaMesas.MinimoFilas;

					else GestionMesas.AltoGrid--;

					break;
				}
			}

			new Comando_ResultadoIntentoEditarMapaMesas(resultado).Enviar(IP);
		}

		private static void Procesar_IntentarCrearMesa(Comando_IntentarCrearMesa Comando, string IP)
		{
			ResultadosIntentoCrearMesa resultado;

			var nuevaMesa = Comando.NuevaMesa;

			if(GestionMesas.Mesas.Where(m => m.Numero == nuevaMesa.Numero).Any())
			{
				resultado = ResultadosIntentoCrearMesa.MesaYaExisteConMismoNumero;
			}
			else if(GestionMesas.Mesas.Where(m => m.GridX == nuevaMesa.GridX && m.GridY == nuevaMesa.GridY).Any())
			{
				resultado = ResultadosIntentoCrearMesa.MesaYaExisteConMismoSitio;
			}
			else
			{
				GestionMesas.Mesas.Add(nuevaMesa);
				resultado = ResultadosIntentoCrearMesa.Correcto;
			}

			new Comando_ResultadoIntentoCrearMesa(resultado).Enviar(IP);
		}

		//private static void Procesar_XXXXX(Comando_XXXXX Comando)
		//{
		//	//
		//}
	}
}
