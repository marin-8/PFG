
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

	public class Comando_PedirMesas : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.PedirMesas;

		//

		//private void InicializarPropiedades()
		//{
			
		//}

		public Comando_PedirMesas()
			: base(TipoComandoInit)
		{
			//InicializarPropiedades(Mesas);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_PedirMesas(TiposComando TipoComandoJson)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			//InicializarPropiedades(Mesas);
		}
	}
}
