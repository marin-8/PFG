
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

	public class Comando_IntentarCrearUsuario : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.IntentarCrearUsuario;

		[JsonProperty("1")] public Usuario NuevoUsuario { get; private set; }

		private void InicializarPropiedades(Usuario NuevoUsuario)
		{
			this.NuevoUsuario = NuevoUsuario;
		}

		public Comando_IntentarCrearUsuario(Usuario NuevoUsuario)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NuevoUsuario);
		}

		[JsonConstructor]
		private Comando_IntentarCrearUsuario(TiposComando TipoComandoJson,  Usuario NuevoUsuario)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NuevoUsuario);
		}
	}
}
