
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
					Procesar_CrearMesa(
						Comando.DeJson
							<Comando_CrearMesa>
								(ComandoJson));

					break;
				}

				case TiposComando.ModificarMesaNumero:
				{
					Procesar_ModificarMesaNumero(
						Comando.DeJson
							<Comando_ModificarMesaNumero>
								(ComandoJson));

					break;
				}

				case TiposComando.ModificarMesaSitio:
				{
					Procesar_ModificarMesaSitio(
						Comando.DeJson
							<Comando_ModificarMesaSitio>
								(ComandoJson));

					break;
				}

				case TiposComando.EliminarMesa:
				{
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

				case TiposComando.ModificarCategoriaNombre:
				{
					Procesar_ModificarCategoriaNombre(
						Comando.DeJson
							<Comando_ModificarCategoriaNombre>
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

		private static string Procesar_IniciarSesion(Comando_IniciarSesion Comando, string IP)
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
					.Single();
			
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
				}
			}

			return new Comando_ResultadoIniciarSesion(resultado,usuario).ToString();
		}

		private static void Procesar_CerrarSesion(Comando_CerrarSesion Comando)
		{
			var usuario =
				GestionUsuarios.Usuarios
					.Where(u => u.NombreUsuario == Comando.Usuario)
					.Single();

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
					.OrderBy(u => (byte)u.Rol)
					.ThenBy(u => u.Nombre)
					.ToArray()
			)
			.ToString();
		}

		private static void Procesar_CrearUsuario(Comando_CrearUsuario Comando)
		{
			GestionUsuarios.Usuarios.Add(Comando.NuevoUsuario);

			GestionUsuarios.Guardar();
		}

		private static void Procesar_ModificarUsuarioNombre(Comando_ModificarUsuarioNombre Comando)
		{
			GestionUsuarios.Usuarios
				.Where(u => u.NombreUsuario == Comando.Usuario)
				.Single()
					.Nombre = Comando.NuevoNombre;

			GestionUsuarios.Guardar();
		}

		private static void Procesar_ModificarUsuarioNombreUsuario(Comando_ModificarUsuarioNombreUsuario Comando)
		{
			GestionUsuarios.Usuarios
				.Where(u => u.NombreUsuario == Comando.Usuario)
				.Single()
					.NombreUsuario = Comando.NuevoNombreUsuario;

			GestionUsuarios.Guardar();
		}

		private static void Procesar_ModificarUsuarioContrasena(Comando_ModificarUsuarioContrasena Comando)
		{
			GestionUsuarios.Usuarios
				.Where(u => u.NombreUsuario == Comando.Usuario)
				.Single()
					.Contrasena = Comando.NuevaContrasena;

			GestionUsuarios.Guardar();
		}

		private static void Procesar_ModificarUsuarioRol(Comando_ModificarUsuarioRol Comando)
		{
			GestionUsuarios.Usuarios
				.Where(u => u.NombreUsuario == Comando.Usuario)
				.Single()
					.Rol = Comando.NuevoRol;

			GestionUsuarios.Guardar();
		}

		private static string Procesar_EliminarUsuario(Comando_EliminarUsuario Comando)
		{
			bool correcto = true;
			string mensaje = "";

			var usuarioAEliminar =
				GestionUsuarios.Usuarios
					.Where(u => u.NombreUsuario == Comando.Usuario)
					.Single();

			if(usuarioAEliminar.Conectado)
			{
				correcto = false;
				mensaje = "No se puede eliminar el usuario porque está conectado";
			}
			else
			{
				GestionUsuarios.Usuarios.Remove(usuarioAEliminar);

				GestionUsuarios.Guardar();
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

			if(correcto) GestionMesas.Guardar();

			return new Comando_ResultadoGenerico(correcto, mensaje).ToString();
		}

		private static void Procesar_CrearMesa(Comando_CrearMesa Comando)
		{
			GestionMesas.Mesas.Add(Comando.NuevaMesa);

			GestionMesas.Guardar();
		}

		private static void Procesar_ModificarMesaNumero(Comando_ModificarMesaNumero Comando)
		{
			GestionMesas.Mesas
				.Where(m => m.Numero == Comando.NumeroMesa)
				.Single()
					.Numero = Comando.NuevoNumeroMesa;

			GestionMesas.Guardar();
		}

		private static void Procesar_ModificarMesaSitio(Comando_ModificarMesaSitio Comando)
		{
			bool consultaMesaEnSitioDestino(Mesa m) => m.SitioX == Comando.NuevoSitioX && m.SitioY == Comando.NuevoSitioY;

			var mesaOrigen = GestionMesas.Mesas.Where(m => m.Numero == Comando.NumeroMesa).Single();

			if(GestionMesas.Mesas.Any(consultaMesaEnSitioDestino))
			{
				var mesaDestino = GestionMesas.Mesas.Where(consultaMesaEnSitioDestino).Single();
			
				mesaDestino.SitioX = mesaOrigen.SitioX;
				mesaDestino.SitioY = mesaOrigen.SitioY;
			}

			mesaOrigen.SitioX = Comando.NuevoSitioX;
			mesaOrigen.SitioY = Comando.NuevoSitioY;

			GestionMesas.Guardar();
		}

		private static void Procesar_EliminarMesa(Comando_EliminarMesa Comando)
		{
			var mesaAEliminar = GestionMesas.Mesas.Where(m => m.Numero == Comando.NumeroMesa).Single();

			GestionMesas.Mesas.Remove(mesaAEliminar);

			GestionMesas.Guardar();			
		}

		private static string Procesar_PedirArticulos()
		{
			return new Comando_MandarArticulos
			(
				GestionArticulos.Articulos.ToArray()
			)
			.ToString();
		}

		private static void Procesar_CrearArticulo(Comando_CrearArticulo Comando)
		{
			GestionArticulos.Articulos.Add(Comando.NuevoArticulo);

			GestionArticulos.Guardar();
		}

		private static void Procesar_ModificarArticuloNombre(Comando_ModificarArticuloNombre Comando)
		{
			GestionArticulos.Articulos
				.Where(a => a.Nombre == Comando.NombreActual)
				.Single()
					.Nombre = Comando.NombreNuevo;

			GestionArticulos.Guardar();
		}

		private static void Procesar_ModificarArticuloCategoria(Comando_ModificarArticuloCategoria Comando)
		{
			GestionArticulos.Articulos
				.Where(a => a.Nombre == Comando.NombreArticulo)
				.Single()
					.Categoria = Comando.NuevaCategoria;

			GestionArticulos.Guardar();
		}

		private static void Procesar_ModificarArticuloPrecio(Comando_ModificarArticuloPrecio Comando)
		{
			GestionArticulos.Articulos
				.Where(a => a.Nombre == Comando.NombreArticulo)
				.Single()
					.Precio = Comando.NuevoPrecio;

			GestionArticulos.Guardar();
		}

		private static void Procesar_ModificarArticuloSitioDePreparacion(Comando_ModificarArticuloSitioDePreparacion Comando)
		{
			GestionArticulos.Articulos
				.Where(a => a.Nombre == Comando.NombreArticulo)
				.Single()
					.SitioPreparacionArticulo = Comando.NuevoSitioPreparacion;

			GestionArticulos.Guardar();
		}

		private static void Procesar_ModificarCategoriaNombre(Comando_ModificarCategoriaNombre Comando)
		{
			var articulosDeEsaCategoria =
				GestionArticulos.Articulos
					.Where(a => a.Categoria == Comando.NombreActual)
					.ToArray();

			foreach(var articulo in articulosDeEsaCategoria)
			{
				articulo.Categoria = Comando.NuevoNombre;
			}

			GestionArticulos.Guardar();
		}

		private static void Procesar_EliminarArticulo(Comando_EliminarArticulo Comando)
		{
			var articuloAEliminar =
				GestionArticulos.Articulos
					.Where(a => a.Nombre == Comando.NombreArticulo)
					.Single();

			GestionArticulos.Articulos.Remove(articuloAEliminar);

			GestionArticulos.Guardar();
		}		

		private static void Procesar_TomarNota(Comando_TomarNota Comando)
		{
			GestionMesas.Mesas
				.Single(m => m.Numero == Comando.NumeroMesa)
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

			// ===== //

			GestionMesas.Guardar();
			GestionTareas.Guardar();
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

		private static void Procesar_TareaCompletada(Comando_TareaCompletada Comando)
		{
			var tareaCompletada =
				GestionTareas.Tareas
					.Single(t => t.ID == Comando.ID);

			tareaCompletada.CompletarTarea();

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
							.Single(m => m.Numero == tareaCompletada.NumeroMesa)
								.EstadoMesa = EstadosMesa.Ocupada;
					}

					break;
				}
				case TiposTareas.LimpiarMesa:
				{
					GestionMesas.Mesas
						.Single(m => m.Numero == tareaCompletada.NumeroMesa)
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

			GestionMesas.Guardar();
			GestionTareas.Guardar();
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
					.Single();

			Global.ReasignarEIntentarEnviarTarea(
					Comun.Global.TareasPrioridadesRoles[tareaAReasignar.TipoTarea].ToArray(),
					tareaAReasignar);

			GestionTareas.Guardar();

			return new Comando_ResultadoGenerico(correcto, mensaje).ToString();
		}

		private static void Procesar_CobrarMesa(Comando_CobrarMesa Comando)
		{
			GestionMesas.Mesas
				.Single(m => m.Numero == Comando.NumeroMesa)
					.EstadoMesa = EstadosMesa.Sucia;

			TiposTareas tipoTarea = TiposTareas.LimpiarMesa;

			Global.EnviarGuardarNuevaTareaAsync
			(
				Comun.Global.TareasPrioridadesRoles[tipoTarea].ToArray(),
				tipoTarea,
				Comando.NumeroMesa,
				null
			);

			GestionMesas.Guardar();
			GestionTareas.Guardar();
		}

		private static async void Procesar_CambiarDisponibilidadArticulo(Comando_CambiarDisponibilidadArticulo Comando)
		{
			var articuloAModificar = 
				GestionArticulos.Articulos
					.Single(a => a.Nombre == Comando.NombreArticulo);
					
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

			GestionArticulos.Guardar();
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
