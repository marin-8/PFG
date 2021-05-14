
using System.Collections.Generic;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class GestionTareas
	{
		private static uint _nuevoIDTarea = 1;
		public static uint NuevoIDTarea
		{
			get => _nuevoIDTarea++;
			private set { _nuevoIDTarea = value; }
		}

		public static List<Tarea> Tareas { get; private set; } = new();

		public static void ResetearIDs()
		{
			NuevoIDTarea = 1;
		}
	}
}
