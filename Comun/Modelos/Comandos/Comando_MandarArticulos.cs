
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

	public class Comando_MandarArticulos : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.MandarArticulos;

		[JsonProperty("1")] public Articulo[] Articulos { get; private set; }

		private void InicializarPropiedades(Articulo[] Articulos)
		{
			this.Articulos = Articulos;
		}

		public Comando_MandarArticulos(Articulo[] Articulos)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(Articulos);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_MandarArticulos(TiposComando TipoComandoJson, Articulo[] Articulos)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(Articulos);
		}
	}
}
