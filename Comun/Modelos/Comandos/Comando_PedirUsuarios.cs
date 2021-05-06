
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

	public class Comando_PedirUsuarios : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.PedirUsuarios;

		//

		//private void InicializarPropiedades()
		//{
			
		//}

		public Comando_PedirUsuarios()
			: base(TipoComandoInit)
		{
			//InicializarPropiedades(Usuarios);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_PedirUsuarios(TiposComando TipoComandoJson)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			//InicializarPropiedades(Usuarios);
		}
	}
}
