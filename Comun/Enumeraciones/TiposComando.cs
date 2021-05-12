
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
		ModificarArticuloCategoria,
		ModificarArticuloPrecio,
		ModificarArticuloSitioDePreparacion,
		EliminarArticulo,

		PedirTicketMesa,
		MandarTicketMesa,

		TomarNota,
		Cobrar,

		EnviarTarea,
		PedirTareas,
		MandarTareas,
		TareaCompletada,
	}
}
