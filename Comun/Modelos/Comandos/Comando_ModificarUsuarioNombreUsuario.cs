
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

	public class Comando_ModificarUsuarioNombreUsuario : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.ModificarUsuarioNombreUsuario;

		[JsonProperty("1")] public string Usuario { get; private set; }
		[JsonProperty("2")] public string NuevoNombreUsuario { get; private set; }

		private void InicializarPropiedades(string Usuario, string NuevoNombreUsuario)
		{
			this.Usuario = Usuario;
			this.NuevoNombreUsuario = NuevoNombreUsuario;
		}

		public Comando_ModificarUsuarioNombreUsuario(string Usuario, string NuevoNombreUsuario)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(Usuario, NuevoNombreUsuario);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_ModificarUsuarioNombreUsuario(TiposComando TipoComandoJson, string Usuario, string NuevoNombreUsuario)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(Usuario, NuevoNombreUsuario);
		}
	}
}
	