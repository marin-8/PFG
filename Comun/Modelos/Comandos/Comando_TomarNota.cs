
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

	public class Comando_TomarNota : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.TomarNota;

		[JsonProperty("1")] public byte NumeroMesa { get; private set; }
		[JsonProperty("2")] public Articulo[] Articulos { get; private set; }

		private void InicializarPropiedades(byte NumeroMesa, Articulo[] Articulos)
		{
			this.NumeroMesa = NumeroMesa;
			this.Articulos = Articulos;
		}

		public Comando_TomarNota(byte NumeroMesa, Articulo[] Articulos)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NumeroMesa, Articulos);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_TomarNota(TiposComando TipoComandoJson, byte NumeroMesa, Articulo[] Articulos)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NumeroMesa, Articulos);
		}
	}
}
