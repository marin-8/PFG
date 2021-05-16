
using System;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using System.Threading.Tasks;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class ProcesadorGestor
	{
	// ============================================================================================== //

		// Variables y constantes

		private const double INTERVALO_COMPROBACION_CONECTADOS = 1000 * 5; // 5 segundos

		private static bool TimerInicializado = false;
		private static readonly System.Timers.Timer Timer = new(INTERVALO_COMPROBACION_CONECTADOS);

	// ============================================================================================== //

        // Métodos de comprobación de conectados

		public static void EmpezarAComprobarConectados()
		{
			if(!TimerInicializado)
			{
				Timer.Elapsed += ComprobarConectados;
				Timer.AutoReset = true;

				TimerInicializado = true;
			}

			Timer.Enabled = true;
		}

		private static void ComprobarConectados(object source, ElapsedEventArgs e)
		{
			var usuariosSupuestamenteConectados =
				GestionUsuarios.Usuarios
					.Where(u => u.Conectado && u.Rol != Roles.Administrador);

			foreach(var usuarioSupuestamenteConectado in usuariosSupuestamenteConectados)
			{
				bool estaConectado = null !=
					new Comando_ComprobarConectado().Enviar(usuarioSupuestamenteConectado.IP, true);

				if(!estaConectado)
				{
					usuarioSupuestamenteConectado.Conectado = false;

					var tareasAReasignar = 
						GestionTareas.Tareas
							.Where(t => !t.Completada && t.NombreUsuario == usuarioSupuestamenteConectado.NombreUsuario);

					foreach(var tarea in tareasAReasignar)
					{
						Global.ReasignarEIntentarEnviarTarea(
							Comun.Global.TareasPrioridadesRoles[tarea.TipoTarea].ToArray(),
							tarea);
					}
				}
			}
		}

		public static void PararDeComprobarConectados()
		{
			Timer.Enabled = false;
		}

	// ============================================================================================== //

        // Método distribuidor de comandos a los procesadores

		public static string ProcesarComandosRecibidos(string IP, string ComandoJson)
		{
			var tipoComando = Comando.Get_TipoComando_De_Json(ComandoJson);

			string comandoRespuesta = new Comando_ResultadoGenerico(true, "Correcto").ToString();

			switch(tipoComando)
			{
				case TiposComando.IniciarSesion:
				{
					comandoRespuesta =
						Procesar_IniciarSesion(
							Comando.DeJson
								<Comando_IniciarSesion>
									(ComandoJson), IP)
										.Result;

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
						Procesar_PedirMesas();

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

				case TiposComando.PedirArticulos:
				{
					comandoRespuesta =
						Procesar_PedirArticulos();

					break;
				}

				case TiposComando.CrearArticulo:
				{
					comandoRespuesta =
						Procesar_CrearArticulo(
							Comando.DeJson
								<Comando_CrearArticulo>
									(ComandoJson));

					break;
				}

				case TiposComando.ModificarArticuloNombre:
				{
					Procesar_ModificarArticuloNombre(
						Comando.DeJson
							<Comando_ModificarArticuloNombre>
								(ComandoJson));

					break;
				}

				case TiposComando.ModificarArticuloCategoria:
				{
					Procesar_ModificarArticuloCategoria(
						Comando.DeJson
							<Comando_ModificarArticuloCategoria>
								(ComandoJson));

					break;
				}

				case TiposComando.ModificarArticuloPrecio:
				{
					Procesar_ModificarArticuloPrecio(
						Comando.DeJson
							<Comando_ModificarArticuloPrecio>
								(ComandoJson));

					break;
				}

				case TiposComando.ModificarArticuloSitioDePreparacion:
				{
					Procesar_ModificarArticuloSitioDePreparacion(
						Comando.DeJson
							<Comando_ModificarArticuloSitioDePreparacion>
								(ComandoJson));

					break;
				}

				case TiposComando.EliminarArticulo:
				{
					Procesar_EliminarArticulo(
						Comando.DeJson
							<Comando_EliminarArticulo>
								(ComandoJson));

					break;
				}

				case TiposComando.TomarNota:
				{
					Procesar_TomarNota(
						Comando.DeJson
							<Comando_TomarNota>
								(ComandoJson));

					break;
				}

				case TiposComando.PedirTareas:
				{
					comandoRespuesta =
						Procesar_PedirTareas(
							Comando.DeJson
								<Comando_PedirTareas>
									(ComandoJson));

					break;
				}

				case TiposComando.PedirTicketMesa:
				{
					comandoRespuesta =
						Procesar_PedirTicketMesa(
							Comando.DeJson
								<Comando_PedirTicketMesa>
									(ComandoJson));

					break;
				}

				case TiposComando.TareaCompletada:
				{
					Procesar_TareaCompletada(
						Comando.DeJson
							<Comando_TareaCompletada>
								(ComandoJson));

					break;
				}

				case TiposComando.ReasignarTarea:
				{
					comandoRespuesta =
						Procesar_ReasignarTarea(
							Comando.DeJson
								<Comando_ReasignarTarea>
									(ComandoJson));

					break;
				}

				case TiposComando.CobrarMesa:
				{
					Procesar_CobrarMesa(
						Comando.DeJson
							<Comando_CobrarMesa>
								(ComandoJson));

					break;
				}

				case TiposComando.CambiarDisponibilidadArticulo:
				{
					Procesar_CambiarDisponibilidadArticulo(
						Comando.DeJson
							<Comando_CambiarDisponibilidadArticulo>
								(ComandoJson));

					break;
				}

				case TiposComando.ModificarAjusteComenzarJornadaConArticulosDisponibles:
				{
					Procesar_ModificarAjusteComenzarJornadaConArticulosDisponibles(
						Comando.DeJson
							<Comando_ModificarAjusteComenzarJornadaConArticulosDisponibles>
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

	// ============================================================================================== //

        // Métodos Procesar

		private static async Task<string> Procesar_IniciarSesion(Comando_IniciarSesion Comando, string IP)
		{
			ResultadosIniciarSesion resultado;
			Usuario usuario = null;

			var nombresUsuarios =
				GestionUsuarios.Usuarios
					.Select(u => u.NombreUsuario);


			if(!nombresUsuarios.Contains(Comando.Usuario))
			{
				resultado = ResultadosIniciarSesion.UsuarioNoExiste;
				return new Comando_ResultadoIniciarSesion(resultado,usuario).ToString();
			}
			
			usuario =
				GestionUsuarios.Usuarios
					.Where(u => u.NombreUsuario == Comando.Usuario)
					.Select(u => u)
					.First();
			
			if(usuario.Contrasena != Comando.Contrasena)
			{
				resultado = ResultadosIniciarSesion.ContrasenaIncorrecta;
			}
			else if(usuario.Conectado)
			{
				resultado = ResultadosIniciarSesion.UsuarioYaConectado;
			}
			else if(!(Global.JornadaEnCurso ^ usuario.Rol == Roles.Administrador)) // Qué guay el operador ^ (xor)
			{
				resultado = ResultadosIniciarSesion.JornadaEnEstadoNoPermitido;
			}
			else
			{
				usuario.IP = IP;
				usuario.Conectado = true;

				resultado = ResultadosIniciarSesion.Correcto;

				// ===== //

				var tareasSinAsignar =
					GestionTareas.Tareas
						.Where(t => !t.Completada && t.NombreUsuario == null)
						.ToArray();

				if(tareasSinAsignar.Length > 0)
				{
					foreach(var tareaSinAsignar in tareasSinAsignar)
					{
						tareaSinAsignar.Reasignar(usuario.NombreUsuario);
					}

					await Task.Run(() => Global.GuardarEstado());
				}
			}

			return new Comando_ResultadoIniciarSesion(resultado,usuario).ToString();
		}

		private static void Procesar_CerrarSesion(Comando_CerrarSesion Comando)
		{
			var usuario =
				GestionUsuarios.Usuarios
					.Where(u => u.NombreUsuario == Comando.Usuario)
					.First();

			usuario.Conectado = false;

			// ===== //

			var tareasARepartir = 
				GestionTareas.Tareas
					.Where(t => !t.Completada && t.NombreUsuario == Comando.Usuario);

			foreach(var tarea in tareasARepartir)
			{
				Global.ReasignarEIntentarEnviarTarea(
					Comun.Global.TareasPrioridadesRoles[tarea.TipoTarea].ToArray(),
					tarea);
			}
		}

		private static string Procesar_PedirUsuarios()
		{
			return new Comando_MandarUsuarios
			(
				GestionUsuarios.Usuarios
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
				.Where(u => u.NombreUsuario == Comando.Usuario)
				.First()
					.Nombre = Comando.NuevoNombre;
		}

		private static void Procesar_ModificarUsuarioNombreUsuario(Comando_ModificarUsuarioNombreUsuario Comando)
		{
			GestionUsuarios.Usuarios
				.Where(u => u.NombreUsuario == Comando.Usuario)
				.First()
					.NombreUsuario = Comando.NuevoNombreUsuario;
		}

		private static void Procesar_ModificarUsuarioContrasena(Comando_ModificarUsuarioContrasena Comando)
		{
			GestionUsuarios.Usuarios
				.Where(u => u.NombreUsuario == Comando.Usuario)
				.First()
					.Contrasena = Comando.NuevaContrasena;
		}

		private static void Procesar_ModificarUsuarioRol(Comando_ModificarUsuarioRol Comando)
		{
			GestionUsuarios.Usuarios
				.Where(u => u.NombreUsuario == Comando.Usuario)
				.First()
					.Rol = Comando.NuevoRol;
		}

		private static string Procesar_EliminarUsuario(Comando_EliminarUsuario Comando)
		{
			bool correcto = true;
			string mensaje = "";

			var usuarioAEliminar =
				GestionUsuarios.Usuarios
					.Where(u => u.NombreUsuario == Comando.Usuario)
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

		private static string Procesar_PedirMesas()
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

			bool consultaMesaEnSitioDestino(Mesa m) => m.SitioX == Comando.NuevoSitioX && m.SitioY == Comando.NuevoSitioY;

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

		private static string Procesar_PedirArticulos()
		{
			return new Comando_MandarArticulos
			(
				GestionArticulos.Articulos.ToArray()
			)
			.ToString();
		}

		private static string Procesar_CrearArticulo(Comando_CrearArticulo Comando)
		{
			bool correcto = true;
			string mensaje = "";

			if(GestionArticulos.Articulos.Select(a => a.Nombre).Contains(Comando.NuevoArticulo.Nombre))
			{
				correcto = false;
				mensaje = "Alguien ha creado un artículo con el mismo Nombre antes de que se haya introducido el que has creado, por lo que no se ha añadido el tuyo";
			}
			else
			{
				GestionArticulos.Articulos.Add(Comando.NuevoArticulo);
			}

			return new Comando_ResultadoGenerico(correcto, mensaje).ToString();
		}

		private static void Procesar_ModificarArticuloNombre(Comando_ModificarArticuloNombre Comando)
		{
			GestionArticulos.Articulos
				.Where(a => a.Nombre == Comando.NombreActual)
				.First()
					.Nombre = Comando.NombreNuevo;
		}

		private static void Procesar_ModificarArticuloCategoria(Comando_ModificarArticuloCategoria Comando)
		{
			GestionArticulos.Articulos
				.Where(a => a.Nombre == Comando.NombreArticulo)
				.First()
					.Categoria = Comando.NuevaCategoria;
		}

		private static void Procesar_ModificarArticuloPrecio(Comando_ModificarArticuloPrecio Comando)
		{
			GestionArticulos.Articulos
				.Where(a => a.Nombre == Comando.NombreArticulo)
				.First()
					.Precio = Comando.NuevoPrecio;
		}

		private static void Procesar_ModificarArticuloSitioDePreparacion(Comando_ModificarArticuloSitioDePreparacion Comando)
		{
			GestionArticulos.Articulos
				.Where(a => a.Nombre == Comando.NombreArticulo)
				.First()
					.SitioPreparacionArticulo = Comando.NuevoSitioPreparacion;
		}

		private static void Procesar_EliminarArticulo(Comando_EliminarArticulo Comando)
		{
			var articuloAEliminar =
				GestionArticulos.Articulos
					.Where(a => a.Nombre == Comando.NombreArticulo)
					.First();

			GestionArticulos.Articulos.Remove(articuloAEliminar);
		}		

		private static void Procesar_TomarNota(Comando_TomarNota Comando)
		{
			GestionMesas.Mesas
				.First(m => m.Numero == Comando.NumeroMesa)
					.EstadoMesa = EstadosMesa.Esperando;

			// ===== //

			var articulosPreparacionBarra = Comando.Articulos.Where(a => a.SitioPreparacionArticulo == SitioPreparacionArticulo.Barra).ToArray();			
			
			if(articulosPreparacionBarra.Any())
			{
				TiposTareas tipoTarea = TiposTareas.PrepararArticulosBarra;

				Global.EnviarGuardarNuevaTareaAsync
				(
					Comun.Global.TareasPrioridadesRoles[tipoTarea].ToArray(),
					tipoTarea,
					Comando.NumeroMesa,
					articulosPreparacionBarra
				);
			}

			// ===== //

			var articulosPreparacionCocina = Comando.Articulos.Where(a => a.SitioPreparacionArticulo == SitioPreparacionArticulo.Cocina).ToArray();			
			
			if(articulosPreparacionCocina.Any())
			{
				TiposTareas tipoTarea = TiposTareas.PrepararArticulosCocina;

				Global.EnviarGuardarNuevaTareaAsync
				(
				    Comun.Global.TareasPrioridadesRoles[tipoTarea].ToArray(),
					tipoTarea,
					Comando.NumeroMesa,
					articulosPreparacionCocina
				);
			}
		}

		private static string Procesar_PedirTareas(Comando_PedirTareas Comando)
		{
			return new Comando_MandarTareas
			(
				GestionTareas.Tareas
					.Where(
						t => !t.Completada && 
						t.NombreUsuario == Comando.NombreUsuario)
					.ToArray()
			)
			.ToString();
		}

		private static string Procesar_PedirTicketMesa(Comando_PedirTicketMesa Comando)
		{
			var tareaUltimaLimpiezaDeMesa =
				GestionTareas.Tareas
					.Where(t => t.NumeroMesa == Comando.NumeroMesa
						     && t.TipoTarea == TiposTareas.LimpiarMesa)
					.OrderBy(t => t.FechaHoraCreacion)
					.LastOrDefault();

			var fechaHoraUltimaLimpiezaMesa =
				tareaUltimaLimpiezaDeMesa == null
				? new DateTime(2000, 1, 1, 0, 0, 0)
				: tareaUltimaLimpiezaDeMesa.FechaHoraCreacion;

			return new Comando_MandarTicketMesa
			(
				// Me dirás que no es beia' esta consulta <3

				GestionTareas.Tareas
					.Where(t => t.NumeroMesa == Comando.NumeroMesa
							 && t.TipoTarea == TiposTareas.ServirArticulos
					         && t.FechaHoraCreacion > fechaHoraUltimaLimpiezaMesa)
					.SelectMany(t => t.Articulos)
						.GroupBy(a => a.Nombre)
						.Select(g => 
							new ItemTicket
							(
								g.Sum(a => a.Unidades),
								g.Key,
								g.First().Precio
							))
						.OrderBy(it => it.NombreArticulo)
						.ToArray()
			)
			.ToString();
		}

		private static async void Procesar_TareaCompletada(Comando_TareaCompletada Comando)
		{
			var tareaCompletada =
				GestionTareas.Tareas
					.First(t => t.ID == Comando.ID);

			tareaCompletada.CompletarTarea();

			await Task.Run(() => Global.GuardarEstado());

			switch(tareaCompletada.TipoTarea)
			{
				case TiposTareas.ServirArticulos:
				{
					if(!GestionTareas.Tareas
						.Any(t => !t.Completada
						       && (t.TipoTarea == TiposTareas.PrepararArticulosBarra
							      || t.TipoTarea == TiposTareas.PrepararArticulosCocina
							      || t.TipoTarea == TiposTareas.ServirArticulos)
							   && t.NumeroMesa == tareaCompletada.NumeroMesa))
					{
						GestionMesas.Mesas
							.First(m => m.Numero == tareaCompletada.NumeroMesa)
								.EstadoMesa = EstadosMesa.Ocupada;
					}

					break;
				}
				case TiposTareas.LimpiarMesa:
				{
					GestionMesas.Mesas
						.First(m => m.Numero == tareaCompletada.NumeroMesa)
							.EstadoMesa = EstadosMesa.Vacia;

					break;
				}
				case TiposTareas.PrepararArticulosBarra:
				case TiposTareas.PrepararArticulosCocina:
				{
					TiposTareas tipoTarea = TiposTareas.ServirArticulos;

					Global.EnviarGuardarNuevaTareaAsync
					(
						Comun.Global.TareasPrioridadesRoles[tipoTarea].ToArray(),
						tipoTarea,
						tareaCompletada.NumeroMesa,
						tareaCompletada.Articulos
					);

					break;
				}
			}
		}

		private static string Procesar_ReasignarTarea(Comando_ReasignarTarea Comando)
		{
			bool correcto = true;
			string mensaje = "";

			// Detectar si el nuevo usuario es el mismo
			// que el anterior, notificar al reasignante
			// y no realizar el envío (ya que sería innecesario)

			var tareaAReasignar =
				GestionTareas.Tareas
					.Where(t => t.ID == Comando.ID)
					.First();

			Global.ReasignarEIntentarEnviarTarea(
					Comun.Global.TareasPrioridadesRoles[tareaAReasignar.TipoTarea].ToArray(),
					tareaAReasignar);

			return new Comando_ResultadoGenerico(correcto, mensaje).ToString();
		}

		private static void Procesar_CobrarMesa(Comando_CobrarMesa Comando)
		{
			GestionMesas.Mesas
				.First(m => m.Numero == Comando.NumeroMesa)
					.EstadoMesa = EstadosMesa.Sucia;

			TiposTareas tipoTarea = TiposTareas.LimpiarMesa;

			Global.EnviarGuardarNuevaTareaAsync
			(
				Comun.Global.TareasPrioridadesRoles[tipoTarea].ToArray(),
				tipoTarea,
				Comando.NumeroMesa,
				null
			);
		}

		private static async void Procesar_CambiarDisponibilidadArticulo(Comando_CambiarDisponibilidadArticulo Comando)
		{
			var articuloAModificar = 
				GestionArticulos.Articulos
					.First(a => a.Nombre == Comando.NombreArticulo);
					
			articuloAModificar.Disponible = !articuloAModificar.Disponible;

			var usuariosConectados =
				GestionUsuarios.Usuarios
					.Where(u => u.Conectado);

			foreach(var usuarioConectado in usuariosConectados)
			{
				await Task.Run(() =>
				{
					if(new Comando_RefrescarDisponibilidadArticulos().Enviar(usuarioConectado.IP, true) == null)
						usuarioConectado.Conectado = false;
				});
			}
			
			await Task.Run(() => Global.GuardarEstado());
		}

		private static void Procesar_ModificarAjusteComenzarJornadaConArticulosDisponibles(Comando_ModificarAjusteComenzarJornadaConArticulosDisponibles Comando)
		{
			Ajustes.ComenzarJornadaConArticulosDisponibles = Comando.EstaActivado;

			Ajustes.Guardar();
		}

		//private static void Procesar_XXXXX(Comando_XXXXX Comando)
		//{
		//	//
		//}

	// ============================================================================================== //
	}
}
