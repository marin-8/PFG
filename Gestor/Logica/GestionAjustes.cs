
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class GestionAjustes
	{
		const string RUTA_ARCHIVO_JSON = @".\Guardado\Ajustes.json";

		private static readonly object GuardadoLock = new();

		#pragma warning disable CA2211
		public static AjustesObjeto Ajustes = new();
		#pragma warning restore CA2211

		public static void Cargar()
		{
			string usuariosJsonString = File.ReadAllText(RUTA_ARCHIVO_JSON);
			Ajustes  = JsonConvert.DeserializeObject<AjustesObjeto>(usuariosJsonString);
		}

		public static void Guardar()
		{
			Task.Run(() =>
			{
				lock(GuardadoLock)
				{
					using StreamWriter archivo = File.CreateText(RUTA_ARCHIVO_JSON);
					new JsonSerializer().Serialize(archivo, Ajustes);
				}
			});
		}
	}
}
