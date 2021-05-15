
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

		public static Usuario Get_UsuarioConectadoConMenosTareasPendientesYMenosTareasCompletadas(Roles rol)
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

		public static async void EnviarGuardarNuevaTareaAsync(Roles[] PrioridadRoles, TiposTareas TipoTarea, byte NumeroMesa, Articulo[] Articulos = null, bool Guardar = true)
		{
			await Task.Run(() =>
			{
				Usuario usuarioAsignar;
				Tarea nuevaTarea;

				bool tareaEnviada = false;

				do
				{
					if(GestionUsuarios.Usuarios.Any(u => u.Conectado && u.Rol == PrioridadRoles[0]))
						usuarioAsignar = Get_UsuarioConectadoConMenosTareasPendientesYMenosTareasCompletadas(PrioridadRoles[0]);
					else if(GestionUsuarios.Usuarios.Any(u => u.Conectado && u.Rol == PrioridadRoles[1]))
						usuarioAsignar = Get_UsuarioConectadoConMenosTareasPendientesYMenosTareasCompletadas(PrioridadRoles[1]);
					else
						usuarioAsignar = Get_UsuarioConectadoConMenosTareasPendientesYMenosTareasCompletadas(PrioridadRoles[2]);
		
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
	}
}
