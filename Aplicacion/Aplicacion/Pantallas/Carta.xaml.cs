
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class Carta : ContentPage
	{
	// ============================================================================================== //

		// Variables y constantes

	// ============================================================================================== //

		// Inicialización

		public Carta()
		{
			InitializeComponent();

			Shell.Current.Navigated += OnNavigatedTo;

			lock(Global.CategoriasLock)
				ListaArticulos.ItemsSource = Global.Categorias;
		}

		private async void OnNavigatedTo(object sender, ShellNavigatedEventArgs e)
		{
			if(e.Current.Location.OriginalString.Contains(((BaseShellItem)Parent).Route.ToString()))
			{
				await Global.Get_Articulos();
			}
		}

	// ============================================================================================== //

		// Eventos UI -> Barra navegación 

		private async void NuevoArticulo_Clicked(object sender, EventArgs e)
		{
			Articulo nuevoArticulo = new("", "", 0f);

			while(true)
			{
				string nombre = await Global.PedirAlUsuarioStringCorrecto("Nombre", 100, true);
				if(nombre == null) return;

				Articulo[] articulos;

				lock(Global.CategoriasLock)
					articulos = Global.Categorias.SelectMany(cl => cl).ToArray();

				if(articulos.Any(a => a.Nombre == nombre))
					await UserDialogs.Instance.AlertAsync("Ya existe un usuario con este Nombre de Usuario", "Alerta", "Aceptar");
				else
					{ nuevoArticulo.Nombre = nombre; break; }
			}

			List<string> categorias;

			lock(Global.CategoriasLock)
				categorias = Global.Categorias.Select(cl => ((GrupoArticuloCategoria)cl).Categoria).ToList();
			var categoriasExistentesMasOpcionNueva = categorias;
			categoriasExistentesMasOpcionNueva.Add("+ Nueva");

			string categoriaONueva = await UserDialogs.Instance.ActionSheetAsync("Categoría", "Cancelar", null, null, categoriasExistentesMasOpcionNueva.ToArray());
			if(categoriaONueva == "Cancelar") return;
			
			if(categoriaONueva != "+ Nueva")
				nuevoArticulo.Categoria = categoriaONueva;
			else
			{
				while(true)
				{
					string nuevaCategoria = await Global.PedirAlUsuarioStringCorrecto("Nueva categoría", 100, true);
					if(nuevaCategoria == null) return;

					if(categorias.Contains(nuevaCategoria))
						await UserDialogs.Instance.AlertAsync("Ya existe una Categoría con este nombre", "Alerta", "Aceptar");
					else
						{ nuevoArticulo.Categoria = nuevaCategoria; break; }
				}
			}

			while(true)
			{
				var configuracionPrompt = new PromptConfig
				{
					InputType = InputType.DecimalNumber,
					IsCancellable = true,
					Title = "Precio",
					Message = $"Mínimo: {Comun.Global.MINIMO_PRECIO_ARTICULO} €\nMáximo: {Comun.Global.MAXIMO_PRECIO_ARTICULO} €",
					OkText = "Aceptar",
					CancelText = "Cancelar",
					MaxLength = (int)Math.Floor(Math.Log10(Comun.Global.MAXIMO_PRECIO_ARTICULO*100+1)+1),
				};

				var resultado = await UserDialogs.Instance.PromptAsync(configuracionPrompt);
				if (!resultado.Ok) return;

				if(!float.TryParse(resultado.Text, out float precio) || float.Parse(resultado.Text) < Comun.Global.MINIMO_PRECIO_ARTICULO || float.Parse(resultado.Text) > Comun.Global.MAXIMO_PRECIO_ARTICULO) {
					await UserDialogs.Instance.AlertAsync("El número introducido no es válido", "Alerta", "Aceptar"); continue; }

				nuevoArticulo.Precio = precio;

				break;
			}

			string sitioPreparacionString = await UserDialogs.Instance.ActionSheetAsync("Se prepara en...", "Cancelar", null, null, SitiosPreparacionArticulo_ToStringArray());
			if(sitioPreparacionString == "Cancelar") return;
			nuevoArticulo.SitioPreparacionArticulo = (SitioPreparacionArticulo)Enum.Parse(typeof(SitioPreparacionArticulo), sitioPreparacionString);

			UserDialogs.Instance.ShowLoading("Creando artículo...");

			var comandoRespuesta = await Task.Run(() =>
			{
				string respuestaGestor = new Comando_CrearArticulo(nuevoArticulo).Enviar(Global.IPGestor);
				return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
			});
			Global.Procesar_ResultadoGenerico(comandoRespuesta, async () => await Global.Get_Articulos() );

			UserDialogs.Instance.HideLoading();
		}

		private async void Refrescar_Clicked(object sender, EventArgs e)
		{
			await Global.Get_Articulos();
		}

	// ============================================================================================== //

		// Eventos UI -> Contenido

		private async void ListaArticulos_Refresh(object sender, EventArgs e)
		{
			await Global.Get_Articulos();

			ListaArticulos.EndRefresh();
		}

		private static readonly string[] OpcionesArticulo = new string[]
		{
			"Cambiar Nombre",
			"Cambiar Categoría",
			"Cambiar Precio",
			"Cambiar Sitio de Preparación",
			"Eliminar"
		};

		private async void ListaArticulos_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var articuloPulsado = (Articulo)e.Item;

			string opcion = await UserDialogs.Instance.ActionSheetAsync($"{articuloPulsado.Nombre}", "Cancelar", null, null, OpcionesArticulo);
			if(opcion == "Cancelar") return;

			if(opcion == OpcionesArticulo[0]) // Cambiar Nombre
			{
				string nuevoNombre = "";

				while(true)
				{
					nuevoNombre = await Global.PedirAlUsuarioStringCorrecto($"Nuevo Nombre\n(actual = {articuloPulsado.Nombre})", 100, true);
					if(nuevoNombre == null) return;

					Articulo[] articulos;

					lock(Global.CategoriasLock)
						articulos = Global.Categorias.SelectMany(cl => cl).ToArray();

					if(articulos.Any(a => a.Nombre == nuevoNombre))
						await UserDialogs.Instance.AlertAsync("Ya existe un usuario con este Nombre de Usuario", "Alerta", "Aceptar");
					else
						break;
				}

				UserDialogs.Instance.ShowLoading("Cambiando nombre...");

				var comandoRespuesta = await Task.Run(() =>
				{
					string respuestaGestor = new Comando_ModificarArticuloNombre(articuloPulsado.Nombre, nuevoNombre).Enviar(Global.IPGestor);
					return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
				});
				Global.Procesar_ResultadoGenerico(comandoRespuesta, async () => await Global.Get_Articulos() );

				UserDialogs.Instance.HideLoading();

				return;
			}

			if(opcion == OpcionesArticulo[1]) // Cambiar Categoría
			{
				string nuevaCategoria = "";

				List<string> categorias;

				lock(Global.CategoriasLock)
					categorias = Global.Categorias.Select(cl => ((GrupoArticuloCategoria)cl).Categoria).ToList();
				var categoriasExistentesMasOpcionNueva = categorias;
				categoriasExistentesMasOpcionNueva.Add("+ Nueva");

				string categoriaONueva = await UserDialogs.Instance.ActionSheetAsync($"Nueva Categoría\n(actual = {articuloPulsado.Categoria})", "Cancelar", null, null, categoriasExistentesMasOpcionNueva.ToArray());
				if(categoriaONueva == "Cancelar") return;
			
				if(categoriaONueva != "+ Nueva")
					nuevaCategoria = categoriaONueva;
				else
				{
					while(true)
					{
						string nuevaNuevaCategoria = await Global.PedirAlUsuarioStringCorrecto("Nueva categoría", 100, true);
						if(nuevaNuevaCategoria == null) return;

						if(categorias.Contains(nuevaNuevaCategoria))
							await UserDialogs.Instance.AlertAsync("Ya existe una Categoría con este nombre", "Alerta", "Aceptar");
						else
							{ nuevaCategoria = nuevaNuevaCategoria; break; }
					}
				}

				UserDialogs.Instance.ShowLoading("Cambiando categoria...");

				var comandoRespuesta = await Task.Run(() =>
				{
					string respuestaGestor = new Comando_ModificarArticuloCategoria(articuloPulsado.Nombre, nuevaCategoria).Enviar(Global.IPGestor);
					return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
				});
				Global.Procesar_ResultadoGenerico(comandoRespuesta, async () => await Global.Get_Articulos() );

				UserDialogs.Instance.HideLoading();

				return;
			}

			if(opcion == OpcionesArticulo[2]) // Cambiar Precio
			{
				float nuevoPrecio = 0f;

				while(true)
				{
					var configuracionPrompt = new PromptConfig
					{
						InputType = InputType.DecimalNumber,
						IsCancellable = true,
						Title = $"Nuevo Precio\n(actual = {articuloPulsado.Precio} €)",
						Message = $"Mínimo: {Comun.Global.MINIMO_PRECIO_ARTICULO} €\nMáximo: {Comun.Global.MAXIMO_PRECIO_ARTICULO} €",
						OkText = "Aceptar",
						CancelText = "Cancelar",
						MaxLength = (int)Math.Floor(Math.Log10(Comun.Global.MAXIMO_PRECIO_ARTICULO*100+1)+1),
					};

					var resultado = await UserDialogs.Instance.PromptAsync(configuracionPrompt);
					if (!resultado.Ok) return;

					if(!float.TryParse(resultado.Text, out nuevoPrecio) || float.Parse(resultado.Text) < Comun.Global.MINIMO_PRECIO_ARTICULO || float.Parse(resultado.Text) > Comun.Global.MAXIMO_PRECIO_ARTICULO) {
						await UserDialogs.Instance.AlertAsync("El número introducido no es válido", "Alerta", "Aceptar"); continue; }

					break;
				}

				UserDialogs.Instance.ShowLoading("Cambiando precio...");

				var comandoRespuesta = await Task.Run(() =>
				{
					string respuestaGestor = new Comando_ModificarArticuloPrecio(articuloPulsado.Nombre, nuevoPrecio).Enviar(Global.IPGestor);
					return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
				});
				Global.Procesar_ResultadoGenerico(comandoRespuesta, async () => await Global.Get_Articulos() );

				UserDialogs.Instance.HideLoading();

				return;
			}

			if(opcion == OpcionesArticulo[3]) // Cambiar Sitio de Preparación
			{
				SitioPreparacionArticulo nuevoSitioPreparacion;

				var sitiosPreparacionArticulo_menosActual = 
					SitiosPreparacionArticulo_ToStringArray()
						.Where(s => s != articuloPulsado.SitioPreparacionArticulo.ToString())
						.ToArray();

				string nuevoSitioPreparacionString =
					await UserDialogs.Instance.ActionSheetAsync(
						$"Cambiar a preparar en...\n(actualmente en {articuloPulsado.SitioPreparacionArticulo})",
						"Cancelar", 
						null, null, 
						sitiosPreparacionArticulo_menosActual);

				if(nuevoSitioPreparacionString == "Cancelar")
					return;
				
				nuevoSitioPreparacion = (SitioPreparacionArticulo)Enum.Parse(typeof(SitioPreparacionArticulo), nuevoSitioPreparacionString);

				UserDialogs.Instance.ShowLoading("Cambiando sitio de preparación...");

				var comandoRespuesta = await Task.Run(() =>
				{
					string respuestaGestor = new Comando_ModificarArticuloSitioDePreparacion(articuloPulsado.Nombre, nuevoSitioPreparacion).Enviar(Global.IPGestor);
					return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
				});
				Global.Procesar_ResultadoGenerico(comandoRespuesta, async () => await Global.Get_Articulos() );

				UserDialogs.Instance.HideLoading();

				return;
			}

			if(opcion == OpcionesArticulo[4]) // Eliminar
			{
				if(await UserDialogs.Instance.ConfirmAsync($"¿Eliminar el artículo '{articuloPulsado.Nombre}'?", "Confirmar eliminación", "Eliminar", "Cancelar"))
				{
					UserDialogs.Instance.ShowLoading("Eliminando artículo...");

					var comandoRespuesta = await Task.Run(() =>
					{
						string respuestaGestor = new Comando_EliminarArticulo(articuloPulsado.Nombre).Enviar(Global.IPGestor);
						return Comando.DeJson<Comando_ResultadoGenerico>(respuestaGestor);
					});
					Global.Procesar_ResultadoGenerico(comandoRespuesta, async () => await Global.Get_Articulos() );

					UserDialogs.Instance.HideLoading();
				}

				return;
			}
		}

	// ============================================================================================== //

		// Métodos helper

		private static string[] SitiosPreparacionArticulo_ToStringArray()
		{
			return
				Enum.GetValues(typeof(SitioPreparacionArticulo))
					.Cast<SitioPreparacionArticulo>()
						.Select(s => s.ToString())
						.ToArray();
		}

	// ============================================================================================== //

		// Métodos Procesar

	// ============================================================================================== //
	}
}
