
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

	public class Comando_ResultadoGenerico : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ResultadoGenerico;

		[JsonProperty("1")] public bool Correcto { get; private set; }
		[JsonProperty("2")] public string Mensaje { get; private set; }

		private void InicializarPropiedades(bool Correcto, string Mensaje)
		{
			this.Correcto = Correcto;
			this.Mensaje = Mensaje;
		}

		public Comando_ResultadoGenerico(bool Correcto, string Mensaje)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(Correcto, Mensaje);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_ResultadoGenerico(TiposComando TipoComandoJson, bool Correcto, string Mensaje)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(Correcto, Mensaje);
		}
	}
}
