
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

	public class Comando_ModificarUsuarioNombre : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ModificarUsuarioNombre;

		[JsonProperty("1")] public string Usuario { get; private set; }
		[JsonProperty("2")] public string NuevoNombre { get; private set; }

		private void InicializarPropiedades(string Usuario, string NuevoNombre)
		{
			this.Usuario = Usuario;
			this.NuevoNombre = NuevoNombre;
		}

		public Comando_ModificarUsuarioNombre(string Usuario, string NuevoNombre)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(Usuario, NuevoNombre);
		}

		[JsonConstructor]
		private Comando_ModificarUsuarioNombre(TiposComando TipoComandoJson, string Usuario, string NuevoNombre)
			: base(TipoComandoJson)
		{
			InicializarPropiedades(Usuario, NuevoNombre);
		}
	}
}
	