
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

	public class Comando_MandarUsuarios : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.MandarUsuarios;

		[JsonProperty("1")] public Usuario[] Usuarios { get; private set; }

		private void InicializarPropiedades(Usuario[] Usuarios)
		{
			this.Usuarios = Usuarios;
		}

		public Comando_MandarUsuarios(Usuario[] Usuarios)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(Usuarios);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_MandarUsuarios(TiposComando TipoComandoJson, Usuario[] Usuarios)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(Usuarios);
		}
	}
}
