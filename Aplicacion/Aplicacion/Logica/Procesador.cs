
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public class Procesador
	{
		public Procesador()
		{

		}

		public string Procesar(string IP, string ComandoJson)
		{
			var tipoComando = Comando.Get_TipoComando_De_Json(ComandoJson);

			switch(tipoComando)
			{
				case TiposComando.ResultadoEditarMapaMesas:
				{
					Procesar_ResultadoEditarMapaMesas(
						Comando.DeJson
							<Comando_ResultadoEditarMapaMesas>
								(ComandoJson));

					break;
				}

				case TiposComando.ResultadoCrearMesa:
				{
					Procesar_ResultadoCrearMesa(
						Comando.DeJson
							<Comando_ResultadoCrearMesa>
								(ComandoJson));

					break;
				}

				//case TiposComando.XXXXX:
				//{
				//	Procesar_XXXXX(
				//		Comando.DeJson
				//			<Comando_XXXXX>
				//				(ComandoJson));

				//	break;
				//}

				default:
				{
					UserDialogs.Instance.Alert("Se ha recibido un comando que no se puede procesar. Tranqui, no pasa nada serio. Contacta con el desarollador e infórmale sobre cómo ha pasado", "Información", "Aceptar");

					break;
				}
			}

			return "";
		}

		private void Procesar_ResultadoEditarMapaMesas(Comando_ResultadoEditarMapaMesas Comando)
		{
			UserDialogs.Instance.HideLoading();

			switch(Comando.ResultadoEditarMapaMesas)
			{
				case ResultadosEditarMapaMesas.Correcto:
				{
					Mesas.Instancia.PedirMesas();
					break;
				}
				case ResultadosEditarMapaMesas.MaximoColumnas:
				{
					UserDialogs.Instance.Alert("Máximo de columnas alcanzado", "Error", "Aceptar");
					break;
				}
				case ResultadosEditarMapaMesas.MinimoColumnas:
				{
					UserDialogs.Instance.Alert("Mínimo de columnas alcanzado", "Error", "Aceptar");
					break;
				}
				case ResultadosEditarMapaMesas.MaximoFilas:
				{
					UserDialogs.Instance.Alert("Máximo de filas alcanzado", "Error", "Aceptar");
					break;
				}
				case ResultadosEditarMapaMesas.MinimoFilas:
				{
					UserDialogs.Instance.Alert("Mínimo de filas alcanzado", "Error", "Aceptar");
					break;
				}
				case ResultadosEditarMapaMesas.MesaBloquea:
				{
					UserDialogs.Instance.Alert("Hay mesas que bloquean las celdas a eliminar", "Error", "Aceptar");
					break;
				}
			}
		}

		private void Procesar_ResultadoCrearMesa(Comando_ResultadoCrearMesa Comando)
		{
			UserDialogs.Instance.HideLoading();

			switch(Comando.ResultadoCrearMesa)
			{
				case ResultadosCrearMesa.Correcto:
				{
					Mesas.Instancia.PedirMesas();
					UserDialogs.Instance.Alert("Mesa creada correctamente", "Información", "Aceptar");
					break;
				}
				case ResultadosCrearMesa.MesaYaExisteConMismoNumero:
				{
					UserDialogs.Instance.Alert("Alguien ha creado una mesa con el mismo Número antes de que se haya introducido la que has creado, por lo que no se ha añadido la tuya", "Error", "Aceptar");
					break;
				}
				case ResultadosCrearMesa.MesaYaExisteConMismoSitio:
				{
					UserDialogs.Instance.Alert("Alguien ha creado una mesa en el mismo Sitio antes de que se haya introducido la que has creado, por lo que no se ha añadido la tuya", "Error", "Aceptar");
					break;
				}
			}
		}

		//private void Procesar_XXXXX(Comando_XXXXX Comando)
		//{
		//	//
		//}
	}
}
