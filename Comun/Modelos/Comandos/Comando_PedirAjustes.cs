
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

	public class Comando_PedirAjustes : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.PedirAjustes;

		//

		//private void InicializarPropiedades()
		//{
			
		//}

		public Comando_PedirAjustes()
			: base(TipoComandoInit)
		{
			//InicializarPropiedades(Mesas);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_PedirAjustes(TiposComando TipoComandoJson)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			//InicializarPropiedades(Mesas);
		}
	}
}
