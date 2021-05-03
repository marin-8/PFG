
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

	public class Comando_IntentarEditarMapaMesas : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.IntentarEditarMapaMesas;

		[JsonProperty("1")] public TiposEdicionMapaMesas TipoEdicionMapaMesas { get; private set; }

		private void InicializarPropiedades(TiposEdicionMapaMesas TipoEdicionMapaMesas)
		{
			this.TipoEdicionMapaMesas = TipoEdicionMapaMesas;
		}

		public Comando_IntentarEditarMapaMesas(TiposEdicionMapaMesas TipoEdicionMapaMesas)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(TipoEdicionMapaMesas);
		}

		[JsonConstructor]
		private Comando_IntentarEditarMapaMesas(TiposComando TipoComandoJson, TiposEdicionMapaMesas TipoEdicionMapaMesas)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(TipoEdicionMapaMesas);
		}
	}
}
