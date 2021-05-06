
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

	public class Comando_EliminarMesa : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.EliminarMesa;

		[JsonProperty("1")] public byte NumeroMesa { get; private set; }

		private void InicializarPropiedades(byte NumeroMesa)
		{
			this.NumeroMesa = NumeroMesa;
		}

		public Comando_EliminarMesa(byte NumeroMesa)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NumeroMesa);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_EliminarMesa(TiposComando TipoComandoJson, byte NumeroMesa)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NumeroMesa);
		}
	}
}
