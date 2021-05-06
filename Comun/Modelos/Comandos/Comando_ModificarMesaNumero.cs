
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

	public class Comando_ModificarMesaNumero : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ModificarMesaNumero;

		[JsonProperty("1")] public byte NumeroMesa { get; private set; }
		[JsonProperty("2")] public byte NuevoNumeroMesa { get; private set; }

		private void InicializarPropiedades(byte NumeroMesa, byte NuevoNumeroMesa)
		{
			this.NumeroMesa = NumeroMesa;
			this.NuevoNumeroMesa = NuevoNumeroMesa;
		}

		public Comando_ModificarMesaNumero(byte NumeroMesa, byte NuevoNumeroMesa)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NumeroMesa, NuevoNumeroMesa);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_ModificarMesaNumero(TiposComando TipoComandoJson, byte NumeroMesa, byte NuevoNumeroMesa)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NumeroMesa, NuevoNumeroMesa);
		}
	}
}
	