
namespace PFG.Comun
{
	public enum TiposComando : byte
	{
		Error,
		ComprobarConectado,

		ResultadoGenerico,

		IniciarSesion,
		ResultadoIniciarSesion,
		CerrarSesion,
		JornadaTerminada,

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

		EnviarTarea,
		PedirTareas,
		MandarTareas,
		TareaCompletada,
		ReasignarTarea,

		TomarNota,
		CobrarMesa,
		CambiarDisponibilidadArticulo,
		RefrescarDisponibilidadArticulos,

		PedirTicketMesa,
		MandarTicketMesa,

		ModificarAjusteComenzarJornadaConArticulosDisponibles
	}
}
