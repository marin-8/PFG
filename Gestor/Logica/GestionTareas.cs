
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class GestionTareas
	{
		const string RUTA_ARCHIVO_JSON = @".\Guardado\Tareas.json";

		private static readonly object GuardadoLock = new();

		private static uint _nuevoIDTarea;
		public static uint NuevoIDTarea
		{
			get => _nuevoIDTarea++;
			private set { _nuevoIDTarea = value; }
		}

		public static List<Tarea> Tareas { get; private set; } = new();

		public static void ResetearTareas()
		{
			NuevoIDTarea = 1;
			Tareas.Clear();
		}

		public static bool Cargar()
		{
			string tareasJsonString = File.ReadAllText(RUTA_ARCHIVO_JSON);
			Tareas = JsonConvert.DeserializeObject<List<Tarea>>(tareasJsonString);

			NuevoIDTarea = (uint)Tareas.Count + 1;

			if(Tareas.Count > 0)
				foreach(var tarea in Tareas)
					tarea.Reasignar(null);

			return NuevoIDTarea == 1;
		}

		public static void Guardar()
		{
			Task.Run(() =>
			{
				lock(GuardadoLock)
				{
					using StreamWriter archivo = File.CreateText(RUTA_ARCHIVO_JSON);
					new JsonSerializer().Serialize(archivo, Tareas);
				}
			});
		}
	}
}
