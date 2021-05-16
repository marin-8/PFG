
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using PFG.Comun;

namespace PFG.Gestor
{
	public static class GestionUsuarios
	{
		private const string RUTA_ARCHIVO_JSON = @".\Guardado\Usuarios.json";

		private static readonly object GuardadoLock = new();

		public static List<Usuario> Usuarios { get; private set; } = new();

		public static void Cargar()
		{
			string usuariosJsonString = File.ReadAllText(RUTA_ARCHIVO_JSON);
			Usuarios = JsonConvert.DeserializeObject<List<Usuario>>(usuariosJsonString);

			foreach(var usuario in Usuarios)
				usuario.IP = "";
		}

		public static void Guardar()
		{
			Task.Run(() =>
			{
				lock(GuardadoLock)
				{
					using StreamWriter archivo = File.CreateText(RUTA_ARCHIVO_JSON);
					new JsonSerializer().Serialize(archivo, Usuarios);
				}
			});
		}
	}
}
