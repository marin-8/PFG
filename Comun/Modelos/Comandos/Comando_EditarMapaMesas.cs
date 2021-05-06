
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

	public class Comando_EditarMapaMesas : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.EditarMapaMesas;

		[JsonProperty("1")] public TiposEdicionMapaMesas TipoEdicionMapaMesas { get; private set; }

		private void InicializarPropiedades(TiposEdicionMapaMesas TipoEdicionMapaMesas)
		{
			this.TipoEdicionMapaMesas = TipoEdicionMapaMesas;
		}

		public Comando_EditarMapaMesas(TiposEdicionMapaMesas TipoEdicionMapaMesas)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(TipoEdicionMapaMesas);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_EditarMapaMesas(TiposComando TipoComandoJson, TiposEdicionMapaMesas TipoEdicionMapaMesas)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(TipoEdicionMapaMesas);
		}
	}
}
