
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PFG.Comun;

namespace PFG.Gestor
{
	public class ProcesadorGestor
	{
		private readonly ListBox RegistroIPs;
		private readonly ListBox RegistroComandos;

		public ProcesadorGestor(ListBox RegistroIPs, ListBox RegistroComandos)
		{
			this.RegistroIPs = RegistroIPs;
			this.RegistroComandos = RegistroComandos;
		}

		public string Procesar(string IP, string ComandoJson)
		{
			RegistroIPs.Invoke(new Action(() =>
			{ 
				RegistroIPs.Items.Add(IP);
				RegistroComandos.Items.Add(ComandoJson);
			}));

			var tipoComando = Comando.Get_TipoComando_De_Json(ComandoJson);

			string comandoRespuesta = "OK";

			switch(tipoComando)
			{
				case TiposComando.IniciarSesion:
				{
					comandoRespuesta =
						Procesar_IniciarSesion(
							Comando.DeJson
								<Comando_IniciarSesion>
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
					comandoRespuesta =
						Procesar_PedirUsuarios();

					break;
				}

				case TiposComando.CrearUsuario:
				{
					comandoRespuesta =
						Procesar_CrearUsuario(
							Comando.DeJson
								<Comando_CrearUsuario>
									(ComandoJson));

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

				case TiposComando.EliminarUsuario:
				{
					comandoRespuesta =
						Procesar_EliminarUsuario(
							Comando.DeJson
								<Comando_EliminarUsuario>
									(ComandoJson));

					break;
				}

				case TiposComando.PedirMesas:
				{
					comandoRespuesta =
						Procesar_PedirMesas(IP);

					break;
				}

				case TiposComando.EditarMapaMesas:
				{
					comandoRespuesta =
						Procesar_EditarMapaMesas(
							Comando.DeJson
								<Comando_EditarMapaMesas>
									(ComandoJson));

					break;
				}

				case TiposComando.CrearMesa:
				{
					comandoRespuesta =
						Procesar_CrearMesa(
							Comando.DeJson
								<Comando_CrearMesa>
									(ComandoJson));

					break;
				}

				case TiposComando.ModificarMesaNumero:
				{
					comandoRespuesta =
						Procesar_ModificarMesaNumero(
							Comando.DeJson
								<Comando_ModificarMesaNumero>
									(ComandoJson));

					break;
				}

				case TiposComando.ModificarMesaSitio:
				{
					comandoRespuesta =
						Procesar_ModificarMesaSitio(
							Comando.DeJson
								<Comando_ModificarMesaSitio>
									(ComandoJson));

					break;
				}

				case TiposComando.EliminarMesa:
				{
					comandoRespuesta =
						Procesar_EliminarMesa(
							Comando.DeJson
								<Comando_EliminarMesa>
									(ComandoJson));

					break;
				}

				//case TiposComando.XXXXX:
				//{
				//	comandoRespuesta =
				//		Procesar_XXXXX(
				//			Comando.DeJson
				//				<Comando_XXXXX>
				//					(ComandoJson));

				//	break;
				//}

				default:
				{
					MessageBox.Show
					(
						"Se ha recibido un comando que no se puede procesar. Contacta con el desarollador para que solucione el problema",
						"Alerta",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					break;
				}
			}

			return comandoRespuesta;
		}

		private static string Procesar_IniciarSesion(Comando_IniciarSesion Comando, string IP)
		{
			ResultadosIniciarSesion resultado;
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

						resultado = ResultadosIniciarSesion.Correcto;
					}
					else
					{
						resultado = ResultadosIniciarSesion.ContrasenaIncorrecta;
					}
				}
				else
				{
					resultado = ResultadosIniciarSesion.UsuarioYaConectado;
				}	
			}
			else
			{
				resultado = ResultadosIniciarSesion.UsuarioNoExiste;
			}

			return new Comando_ResultadoIniciarSesion
			(
				resultado,
				usuario
			)
			.ToString();
		}

		private static void Procesar_CerrarSesion(Comando_CerrarSesion Comando)
		{
			var usuario = GestionUsuarios.Usuarios
							 .Where(u => u.NombreUsuario.Equals(Comando.Usuario))
							 .First();

			usuario.Conectado = false;
		}

		private static string Procesar_PedirUsuarios()
		{
			return new Comando_MandarUsuarios
			(
				GestionUsuarios.Usuarios
					.Where(u => u.Rol != Roles.Desarrollador)
					.OrderByDescending(u => (byte)u.Rol)
					.ThenBy(u => u.Nombre)
					.ToArray()
			)
			.ToString();
		}

		private static string Procesar_CrearUsuario(Comando_CrearUsuario Comando)
		{
			bool correcto = true;
			string mensaje = "";

			if(GestionUsuarios.Usuarios.Select(u => u.NombreUsuario).Contains(Comando.NuevoUsuario.NombreUsuario))
			{
				correcto = false;
				mensaje = "Alguien ha creado un usuario con el mismo Nombre de Usuario antes de que se haya introducido el que has creado, por lo que no se ha añadido el tuyo";
			}
			else
			{
				GestionUsuarios.Usuarios.Add(Comando.NuevoUsuario);
			}

			return new Comando_ResultadoGenerico(correcto, mensaje).ToString();
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

		private static string Procesar_EliminarUsuario(Comando_EliminarUsuario Comando)
		{
			bool correcto = true;
			string mensaje = "";

			var usuarioAEliminar =
				GestionUsuarios.Usuarios
					.Where(u => u.NombreUsuario.Equals(Comando.Usuario))
					.First();

			if(usuarioAEliminar.Conectado)
			{
				correcto = false;
				mensaje = "No se puede eliminar el usuario porque está conectado";
			}
			else
			{
				GestionUsuarios.Usuarios.Remove(usuarioAEliminar);
			}

			return new Comando_ResultadoGenerico(correcto, mensaje).ToString();
		}

		private static string Procesar_PedirMesas(string IP)
		{
			return new Comando_MandarMesas
			(
				GestionMesas.AnchoGrid,
				GestionMesas.AltoGrid,
				GestionMesas.Mesas.ToArray()
			)
			.ToString();
		}

		private static string Procesar_EditarMapaMesas(Comando_EditarMapaMesas Comando)
		{
			bool correcto = true;
			string mensaje = "";

			switch(Comando.TipoEdicionMapaMesas)
			{
				case TiposEdicionMapaMesas.AnadirColumna:
				{					
					if(GestionMesas.AnchoGrid == Comun.Global.MAXIMO_COLUMNAS_MESAS)
					{
						correcto = false;
						mensaje = "Máximo de columnas alcanzado";
					}
					else GestionMesas.AnchoGrid++;

					break;
				}
				case TiposEdicionMapaMesas.QuitarColumna:
				{
					if(GestionMesas.AnchoGrid == GestionMesas.MINIMO_COLUMNAS)
					{
						correcto = false;
						mensaje = "Mínimo de columnas alcanzado";
					}
					else if(GestionMesas.Mesas.Any(m => m.SitioX == GestionMesas.AnchoGrid))
					{
						correcto = false;
						mensaje = "Hay mesas en la columna a eliminar";
					}
					else GestionMesas.AnchoGrid--;

					break;
				}
				case TiposEdicionMapaMesas.AnadirFila:
				{
					if(GestionMesas.AltoGrid == Comun.Global.MAXIMO_FILAS_MESAS)
					{
						correcto = false;
						mensaje = "Máximo de filas alcanzado";
					}
					else GestionMesas.AltoGrid++;

					break;
				}
				case TiposEdicionMapaMesas.QuitarFila:
				{
					if(GestionMesas.AltoGrid == GestionMesas.MINIMO_FILAS)
					{
						correcto = false;
						mensaje = "Mínimo de filas alcanzado";
					}
					else if(GestionMesas.Mesas.Any(m => m.SitioY == GestionMesas.AltoGrid))
					{
						correcto = false;
						mensaje = "Hay mesas en la fila a eliminar";
					}
					else GestionMesas.AltoGrid--;

					break;
				}
			}

			return new Comando_ResultadoGenerico(correcto, mensaje).ToString();
		}

		private static string Procesar_CrearMesa(Comando_CrearMesa Comando)
		{
			bool correcto = true;
			string mensaje = "";

			var nuevaMesa = Comando.NuevaMesa;

			if(GestionMesas.Mesas.Any(m => m.Numero == nuevaMesa.Numero))
			{
				correcto = false;
				mensaje = "Alguien ha creado una mesa con el mismo Número antes de que se haya introducido la que has creado, por lo que no se ha añadido la tuya";
			}
			else if(GestionMesas.Mesas.Any(m => m.SitioX == nuevaMesa.SitioX && m.SitioY == nuevaMesa.SitioY))
			{
				correcto = false;
				mensaje = "Alguien ha creado una mesa en el mismo Sitio antes de que se haya introducido la que has creado, por lo que no se ha añadido la tuya";
			}
			else
			{
				GestionMesas.Mesas.Add(nuevaMesa);
			}

			return new Comando_ResultadoGenerico(correcto, mensaje).ToString();
		}

		private static string Procesar_ModificarMesaNumero(Comando_ModificarMesaNumero Comando)
		{
			bool correcto = true;
			string mensaje = "";

			if(GestionMesas.Mesas.Any(m => m.Numero == Comando.NuevoNumeroMesa))
			{
				correcto = false;
				mensaje = "Alguien ha creado una mesa con el mismo Número antes de que se haya cambiado el número de la tuya, por lo que no se ha modificado";
			}
			else
			{
				GestionMesas.Mesas
					.Where(m => m.Numero == Comando.NumeroMesa)
					.First()
						.Numero = Comando.NuevoNumeroMesa;
			}

			return new Comando_ResultadoGenerico(correcto, mensaje).ToString();
		}

		private static string Procesar_ModificarMesaSitio(Comando_ModificarMesaSitio Comando)
		{
			bool correcto = true;
			string mensaje = "";

			Func<Mesa,bool> consultaMesaEnSitioDestino = (m) => m.SitioX == Comando.NuevoSitioX && m.SitioY == Comando.NuevoSitioY;

			var mesaOrigen = GestionMesas.Mesas.Where(m => m.Numero == Comando.NumeroMesa).First();

			if(GestionMesas.Mesas.Any(consultaMesaEnSitioDestino))
			{
				var mesaDestino = GestionMesas.Mesas.Where(consultaMesaEnSitioDestino).First();
			
				mesaDestino.SitioX = mesaOrigen.SitioX;
				mesaDestino.SitioY = mesaOrigen.SitioY;
			}

			mesaOrigen.SitioX = Comando.NuevoSitioX;
			mesaOrigen.SitioY = Comando.NuevoSitioY;

			return new Comando_ResultadoGenerico(correcto, mensaje).ToString();
		}

		private static string Procesar_EliminarMesa(Comando_EliminarMesa Comando)
		{
			bool correcto = true;
			string mensaje = "";

			var mesaAEliminar = GestionMesas.Mesas.Where(m => m.Numero == Comando.NumeroMesa).First();

			if(mesaAEliminar.EstadoMesa == EstadosMesa.Vacia)
			{
				GestionMesas.Mesas.Remove(mesaAEliminar);
			}
			else
			{
				correcto = false;
				mensaje = "Solo se puede eliminar una mesa si está vacía y limpia";
			}			

			return new Comando_ResultadoGenerico(correcto, mensaje).ToString();
		}

		//private static void Procesar_XXXXX(Comando_XXXXX Comando)
		//{
		//	//
		//}
	}
}
