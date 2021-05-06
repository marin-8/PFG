
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

	public class Comando_ModificarUsuarioRol : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ModificarUsuarioRol;

		[JsonProperty("1")] public string Usuario { get; private set; }
		[JsonProperty("2")] public Roles NuevoRol { get; private set; }

		private void InicializarPropiedades(string Usuario, Roles NuevoRol)
		{
			this.Usuario = Usuario;
			this.NuevoRol = NuevoRol;
		}

		public Comando_ModificarUsuarioRol(string Usuario, Roles NuevoRol)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(Usuario, NuevoRol);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_ModificarUsuarioRol(TiposComando TipoComandoJson, string Usuario, Roles NuevoRol)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(Usuario, NuevoRol);
		}
	}
}
	