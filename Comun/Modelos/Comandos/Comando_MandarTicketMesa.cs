
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

	public class Comando_MandarTicketMesa : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.MandarTicketMesa;

		[JsonProperty("1")] public ItemTicket[] ItemsTicket { get; private set; }

		private void InicializarPropiedades(ItemTicket[] ItemsTicket)
		{
			this.ItemsTicket = ItemsTicket;
		}

		public Comando_MandarTicketMesa(ItemTicket[] ItemsTicket)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(ItemsTicket);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_MandarTicketMesa(TiposComando TipoComandoJson, ItemTicket[] ItemsTicket)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(ItemsTicket);
		}
	}
}
