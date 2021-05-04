
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

		public string Procesar(string IP, string ComandoJson)
		{
			RegistroIPs.Invoke(new Action(() =>
			{ 
				RegistroIPs.Items.Add(IP);
				RegistroComandos.Items.Add(ComandoJson);
			}));

			var tipoComando = Comando.Get_TipoComando_De_Json(ComandoJson);

			string comandoRespuesta = "";

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
					Procesar_EliminarUsuario(
						Comando.DeJson
							<Comando_EliminarUsuario>
								(ComandoJson), IP);

					break;
				}

				case TiposComando.PedirMesas:
				{
					Procesar_PedirMesas(IP);

					break;
				}

				case TiposComando.EditarMapaMesas:
				{
					Procesar_EditarMapaMesas(
						Comando.DeJson
							<Comando_EditarMapaMesas>
								(ComandoJson), IP);

					break;
				}

				case TiposComando.CrearMesa:
				{
					Procesar_CrearMesa(
						Comando.DeJson
							<Comando_CrearMesa>
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
			ResultadosCrearUsuario resultado;

			if(GestionUsuarios.Usuarios.Select(u => u.NombreUsuario).Contains(Comando.NuevoUsuario.NombreUsuario))
			{
				resultado = ResultadosCrearUsuario.UsuarioYaExiste;
			}
			else
			{
				GestionUsuarios.Usuarios.Add(Comando.NuevoUsuario);
				resultado = ResultadosCrearUsuario.Correcto;
			}

			return new Comando_ResultadoCrearUsuario(resultado).ToString();
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

		private static string Procesar_EliminarUsuario(Comando_EliminarUsuario Comando, string IP)
		{
			ResultadosEliminarUsuario resultado;

			var usuarioAEliminar =
				GestionUsuarios.Usuarios
					.Where(u => u.NombreUsuario.Equals(Comando.Usuario))
					.First();

			if(usuarioAEliminar.Conectado)
			{
				resultado = ResultadosEliminarUsuario.UsuarioConectado;
			}
			else
			{
				GestionUsuarios.Usuarios.Remove(usuarioAEliminar);
				resultado = ResultadosEliminarUsuario.Correcto;
			}

			return new Comando_ResultadoEliminarUsuario(resultado).ToString();
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

		private static string Procesar_EditarMapaMesas(Comando_EditarMapaMesas Comando, string IP)
		{
			ResultadosEditarMapaMesas resultado = ResultadosEditarMapaMesas.Correcto;

			switch(Comando.TipoEdicionMapaMesas)
			{
				case TiposEdicionMapaMesas.AnadirColumna:
				{					
					if(GestionMesas.AnchoGrid == Comun.Global.MAXIMO_COLUMNAS_MESAS)
						resultado = ResultadosEditarMapaMesas.MaximoColumnas;

					else GestionMesas.AnchoGrid++;

					break;
				}
				case TiposEdicionMapaMesas.QuitarColumna:
				{
					if(GestionMesas.AnchoGrid == GestionMesas.MINIMO_COLUMNAS)
						resultado = ResultadosEditarMapaMesas.MinimoColumnas;

					else if(GestionMesas.Mesas.Any(m => m.SitioX == GestionMesas.AnchoGrid))
						resultado = ResultadosEditarMapaMesas.MesaBloquea;

					else GestionMesas.AnchoGrid--;

					break;
				}
				case TiposEdicionMapaMesas.AnadirFila:
				{
					if(GestionMesas.AltoGrid == Comun.Global.MAXIMO_FILAS_MESAS)
						resultado = ResultadosEditarMapaMesas.MaximoFilas;

					else GestionMesas.AltoGrid++;

					break;
				}
				case TiposEdicionMapaMesas.QuitarFila:
				{
					if(GestionMesas.AltoGrid == GestionMesas.MINIMO_FILAS)
						resultado = ResultadosEditarMapaMesas.MinimoFilas;

					else if(GestionMesas.Mesas.Any(m => m.SitioY == GestionMesas.AltoGrid))
						resultado = ResultadosEditarMapaMesas.MesaBloquea;

					else GestionMesas.AltoGrid--;

					break;
				}
			}

			return new Comando_ResultadoEditarMapaMesas(resultado).ToString();
		}

		private static string Procesar_CrearMesa(Comando_CrearMesa Comando, string IP)
		{
			ResultadosCrearMesa resultado;

			var nuevaMesa = Comando.NuevaMesa;

			if(GestionMesas.Mesas.Where(m => m.Numero == nuevaMesa.Numero).Any())
			{
				resultado = ResultadosCrearMesa.MesaYaExisteConMismoNumero;
			}
			else if(GestionMesas.Mesas.Where(m => m.SitioX == nuevaMesa.SitioX && m.SitioY == nuevaMesa.SitioY).Any())
			{
				resultado = ResultadosCrearMesa.MesaYaExisteConMismoSitio;
			}
			else
			{
				GestionMesas.Mesas.Add(nuevaMesa);
				resultado = ResultadosCrearMesa.Correcto;
			}

			return new Comando_ResultadoCrearMesa(resultado).ToString();
		}

		//private static void Procesar_XXXXX(Comando_XXXXX Comando)
		//{
		//	//
		//}
	}
}
