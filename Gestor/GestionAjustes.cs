
using System.IO;
using System.Linq;
using System.Reflection;
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
			var ajustesCarga  = JsonConvert.DeserializeObject<AjustesObjeto>(usuariosJsonString);

			MapearObjetoAClaseEstatica(ajustesCarga);
		}

		public static void Guardar()
		{
			Task.Run(() =>
			{
				lock(GuardadoLock)
				{
					var ajustesGuardado = MapearClaseEstaticaAObjeto();

					using StreamWriter archivo = File.CreateText(RUTA_ARCHIVO_JSON);
					new JsonSerializer().Serialize(archivo, ajustesGuardado);
				}
			});
		}

		private static void MapearObjetoAClaseEstatica(AjustesObjeto AjustesCarga)
		{
			var propiedadesOrigen = AjustesCarga.GetType().GetProperties();

			var propiedadesDestino =
				typeof(GestionAjustes)
					.GetProperties(BindingFlags.Public | BindingFlags.Static);

			foreach(var propiedadOrigen in propiedadesOrigen)
			{
				var propiedadDestino =
					propiedadesDestino
						.Single(p => p.Name == propiedadOrigen.Name);

				propiedadDestino.SetValue(null, propiedadOrigen.GetValue(AjustesCarga));
			}
		}

		private static AjustesObjeto MapearClaseEstaticaAObjeto()
		{
			AjustesObjeto ajustesGuardado = new();

			var propiedadesOrigen =
				typeof(GestionAjustes)
					.GetProperties(BindingFlags.Public | BindingFlags.Static);

			var propiedadesDestino = ajustesGuardado.GetType().GetProperties();

			foreach(var propiedadOrigen in propiedadesOrigen)
			{
				var propiedadDestino =
					propiedadesDestino
						.Single(p => p.Name == propiedadOrigen.Name);

				var valor = propiedadOrigen.GetValue(null, null);

				propiedadDestino.SetValue(ajustesGuardado, valor);
			}

			return ajustesGuardado;
		}
	}
}
