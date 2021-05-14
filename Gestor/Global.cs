﻿
using System.Linq;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class Global
	{
		public static bool JornadaEnCurso = false;

		private static int _nuevoIDTarea = 1;
		public static int NuevoIDTarea
		{
			get => _nuevoIDTarea++;
			private set { _nuevoIDTarea = value; }
		}

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
	}
}
