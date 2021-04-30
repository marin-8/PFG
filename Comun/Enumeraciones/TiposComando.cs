
namespace PFG.Comun
{
	public enum TiposComando : byte
	{
		IntentarIniciarSesion = 1,
		ResultadoDelIntentoDeIniciarSesion = 2,
		CerrarSesion = 3,
		PedirUsuarios = 4,
		MandarUsuarios = 5
	}
}
