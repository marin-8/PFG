
using Newtonsoft.Json;

namespace PFG.Comun
{
/*
	COSAS QUE HACER AL CREAR UN NUEVO COMANDO:

	- Renombrar Clase (CTRL+H)
	- Cambiar TipoComandoInit
	- Poner Propiedades
	- Enumerar JsonProperty's
	- Método InicializarPropiedades
	- Constructor public
	- Constructor private
*/

	public class Comando_PedirTareas : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.PedirTareas;

		[JsonProperty("1")] public string NombreUsuario { get; private set; }

		private void InicializarPropiedades(string NombreUsuario)
		{
			this.NombreUsuario = NombreUsuario;
		}

		public Comando_PedirTareas(string NombreUsuario)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NombreUsuario);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_PedirTareas(TiposComando TipoComandoJson, string NombreUsuario)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NombreUsuario);
		}
	}
}
