
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

	public class Comando_PedirArticulos : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.PedirArticulos;

		//

		//private void InicializarPropiedades()
		//{
			
		//}

		public Comando_PedirArticulos()
			: base(TipoComandoInit)
		{
			//InicializarPropiedades(Usuarios);
		}

		[JsonConstructor]
		private Comando_PedirArticulos(TiposComando TipoComandoJson)
			: base(TipoComandoJson)
		{
			//InicializarPropiedades(Usuarios);
		}
	}
}
