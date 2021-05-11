
namespace PFG.Comun
{
	public enum TiposComando : byte
	{
		ResultadoGenerico,

		IniciarSesion,
		ResultadoIniciarSesion,
		CerrarSesion,

		PedirUsuarios,
		MandarUsuarios,
		CrearUsuario,
		ModificarUsuarioNombre,
		ModificarUsuarioNombreUsuario,
		ModificarUsuarioContrasena,
		ModificarUsuarioRol,
		EliminarUsuario,

		PedirMesas,
		MandarMesas,
		EditarMapaMesas,
		CrearMesa,
		ModificarMesaNumero,
		ModificarMesaSitio,
		EliminarMesa,

		PedirArticulos,
		MandarArticulos,
		CrearArticulo,
		ModificarArticuloNombre,
		ModificarArticuloPrecio,
		ModificarArticuloCategoria,
		EliminarArticulo,

		TomarNota,

		EnviarTarea
	}
}
