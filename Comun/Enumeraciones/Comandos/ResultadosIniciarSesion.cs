
namespace PFG.Comun
{
	public enum ResultadosIniciarSesion : byte
	{
		Correcto,
		UsuarioNoExiste,
		ContrasenaIncorrecta,
		JornadaEnEstadoNoPermitido,
		UsuarioYaConectado
	}
}
