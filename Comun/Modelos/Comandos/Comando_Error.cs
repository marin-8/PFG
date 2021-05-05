
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

	public class Comando_Error : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.Error;

		[JsonProperty("1")] public string Mensaje { get; private set; }

		private void InicializarPropiedades(string Mensaje)
		{
			this.Mensaje = Mensaje;
		}

		public Comando_Error(string Mensaje)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(Mensaje);
		}

		[JsonConstructor]
		private Comando_Error(TiposComando TipoComandoJson, string Mensaje)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(Mensaje);
		}
	}
}
