
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

	public class Comando_CrearUsuario : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.CrearUsuario;

		[JsonProperty("1")] public Usuario NuevoUsuario { get; private set; }

		private void InicializarPropiedades(Usuario NuevoUsuario)
		{
			this.NuevoUsuario = NuevoUsuario;
		}

		public Comando_CrearUsuario(Usuario NuevoUsuario)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(NuevoUsuario);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_CrearUsuario(TiposComando TipoComandoJson,  Usuario NuevoUsuario)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(NuevoUsuario);
		}
	}
}
