
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class GestionMesas
	{
		public const byte MINIMO_COLUMNAS = 1;
		public const byte MINIMO_FILAS = 1;

		const string RUTA_ARCHIVO_JSON_MESAS_GRID = @".\Guardado\MesasGrid.json";
		const string RUTA_ARCHIVO_JSON_MESAS = @".\Guardado\Mesas.json";

		private static readonly object GuardadoLock = new();

		private static byte[] _dimensionesGrid;
		public static byte AnchoGrid { get => _dimensionesGrid[0]; set => _dimensionesGrid[0] = value; }
		public static byte AltoGrid { get => _dimensionesGrid[1]; set => _dimensionesGrid[1] = value; }

		public static List<Mesa> Mesas { get; private set; } = new();

		public static void Cargar()
		{
			string mesasGridJsonString = File.ReadAllText(RUTA_ARCHIVO_JSON_MESAS_GRID);
			_dimensionesGrid = JsonConvert.DeserializeObject<byte[]>(mesasGridJsonString);

			string mesasJsonString = File.ReadAllText(RUTA_ARCHIVO_JSON_MESAS);
			Mesas = JsonConvert.DeserializeObject<List<Mesa>>(mesasJsonString);
		}

		public static void Guardar()
		{
			Task.Run(() =>
			{
				lock(GuardadoLock)
				{
					using StreamWriter archivo1 = File.CreateText(RUTA_ARCHIVO_JSON_MESAS_GRID);
					new JsonSerializer().Serialize(archivo1, _dimensionesGrid);

					using StreamWriter archivo2 = File.CreateText(RUTA_ARCHIVO_JSON_MESAS);
					new JsonSerializer().Serialize(archivo2, Mesas);
				}
			});
		}
	}
}
