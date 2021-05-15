
using System.IO;
using System.Timers;
using System.Collections.Generic;

using Newtonsoft.Json;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class GestionTareas
	{
		private const double INTERVALO_GUARDADO_AUTOMATICO = 1000 * 10; // 10 segundos

		const string RUTA_ARCHIVO_JSON = @".\Archivos\Tareas.json";

		private static uint _nuevoIDTarea;
		public static uint NuevoIDTarea
		{
			get => _nuevoIDTarea++;
			private set { _nuevoIDTarea = value; }
		}

		private static bool TimerInicializado = false;
		private static readonly Timer Timer = new(INTERVALO_GUARDADO_AUTOMATICO);

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
			using StreamWriter archivo = File.CreateText(RUTA_ARCHIVO_JSON);
			new JsonSerializer().Serialize(archivo, Tareas);
		}

		public static void ComenzarGuardadoAutomatico()
		{
			if(!TimerInicializado)
			{
				Timer.Elapsed += GuardadoAutomatico;
				Timer.AutoReset = true;

				TimerInicializado = true;
			}

			Timer.Enabled = true;
		}

		private static void GuardadoAutomatico(object source, ElapsedEventArgs e)
		{
			Guardar();
		}

		public static void PararGuardadoAutomatico()
		{
			Timer.Enabled = false;
		}
	}
}
