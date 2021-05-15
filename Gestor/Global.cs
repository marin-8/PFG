
using System;
using System.Linq;
using System.Threading.Tasks;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class Global
	{
		#pragma warning disable CA2211
		public static bool JornadaEnCurso = false;
		#pragma warning restore CA2211

		public static Usuario Get_UsuarioConectadoConMenosTareas(Roles rol)
		{
			return 
				GestionUsuarios.Usuarios
					.Where(u => u.Conectado && u.Rol == rol)
					.OrderBy(u =>
						GestionTareas.Tareas
							.Where(t => t.NombreUsuario == u.NombreUsuario && !t.Completada)
							.Count())
					.ThenBy(u => 
						GestionTareas.Tareas
							.Where(t => t.NombreUsuario == u.NombreUsuario && t.Completada)
							.Count())
					.FirstOrDefault();
		}

		public static async void ReasignarEIntentarEnviarTarea(Roles[] PrioridadRoles, Tarea Tarea)
		{
			await Task.Run(() =>
			{
				Usuario usuarioAsignar;

				bool tareaEnviadaONull = false;

				do
				{
					usuarioAsignar =
						Get_UsuarioConectadoConMenosTareas_PorPrioridadRoles(PrioridadRoles);

					if(usuarioAsignar != null)
					{
						Tarea.Reasignar(usuarioAsignar.NombreUsuario);

						tareaEnviadaONull = null !=
							new Comando_EnviarTarea(Tarea).Enviar(usuarioAsignar.IP, true);

						if(!tareaEnviadaONull)
							usuarioAsignar.Conectado = false;
					}
					else
					{
						Tarea.Reasignar(null);

						tareaEnviadaONull = true;
					}
				}
				while(!tareaEnviadaONull);
			});
		}

		public static async void EnviarGuardarNuevaTareaAsync(Roles[] PrioridadRoles, TiposTareas TipoTarea, byte NumeroMesa, Articulo[] Articulos = null, bool Guardar = true)
		{
			await Task.Run(() =>
			{
				Usuario usuarioAsignar;
				Tarea nuevaTarea;

				bool tareaEnviada = false;

				do
				{
					usuarioAsignar =
						Get_UsuarioConectadoConMenosTareas_PorPrioridadRoles(PrioridadRoles);
		
					nuevaTarea = new Tarea(
						GestionTareas.NuevoIDTarea,
						DateTime.Now,
						TipoTarea,
						usuarioAsignar.NombreUsuario,
						Articulos,
						NumeroMesa);

					tareaEnviada = null !=
						new Comando_EnviarTarea(nuevaTarea).Enviar(usuarioAsignar.IP, true);

					if(!tareaEnviada)
						usuarioAsignar.Conectado = false;
				}
				while(!tareaEnviada);

				if(Guardar)
					GestionTareas.Tareas.Add(nuevaTarea);
			});
		}

		public static Usuario Get_UsuarioConectadoConMenosTareas_PorPrioridadRoles(Roles[] PrioridadRoles)
		{
			for(int r = 0 ; r < PrioridadRoles.Length ; r++)
			{
				if(GestionUsuarios.Usuarios.Any(u => u.Conectado && u.Rol == PrioridadRoles[r]))
					return Get_UsuarioConectadoConMenosTareas(PrioridadRoles[r]);
			}

			return null;				
		}
	}
}
