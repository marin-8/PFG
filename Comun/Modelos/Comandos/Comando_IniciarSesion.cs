
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

	public class Comando_IniciarSesion : Comando
	{
		private const TiposComando TipoComandoInit = TiposComando.IniciarSesion;

		[JsonProperty("1")] public string Usuario { get; private set; }
		[JsonProperty("2")] public string Contrasena { get; private set; }

		private void InicializarPropiedades(string Usuario, string Contrasena)
		{
			this.Usuario = Usuario;
			this.Contrasena = Contrasena;
		}

		public Comando_IniciarSesion(string Usuario, string Contrasena)
			: base(TipoComandoInit)
		{
			InicializarPropiedades(Usuario, Contrasena);
		}

		[JsonConstructor]
		#pragma warning disable IDE0051
		private Comando_IniciarSesion(TiposComando TipoComandoJson, string Usuario, string Contrasena)
		#pragma warning restore IDE0051
			: base(TipoComandoJson)
		{
			InicializarPropiedades(Usuario, Contrasena);
		}
	}
}
	