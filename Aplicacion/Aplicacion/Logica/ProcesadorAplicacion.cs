﻿
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
	public class ProcesadorAplicacion
	{
		public ProcesadorAplicacion()
		{

		}

		#pragma warning disable IDE0060
		public string Procesar(string IP, string ComandoJson)
		#pragma warning restore IDE0060
		{
			var tipoComando = Comando.Get_TipoComando_De_Json(ComandoJson);

			string comandoRespuesta = "OK";

			switch(tipoComando)
			{
				case TiposComando.EnviarTarea:
				{
					Procesar_EnviarTarea(
						Comando.DeJson
							<Comando_EnviarTarea>
								(ComandoJson));

					break;
				}

				case TiposComando.RefrescarDisponibilidadArticulos:
				{
					Procesar_RefrescarDisponibilidadArticulos(
						Comando.DeJson
							<Comando_RefrescarDisponibilidadArticulos>
								(ComandoJson));

					break;
				}

				//case TiposComando.XXXXX:
				//{
				//	comandoRespuesta =
				//		Procesar_XXXXX(
				//			Comando.DeJson
				//				<Comando_XXXXX>
				//					(ComandoJson));

				//	break;
				//}

				default:
				{
					UserDialogs.Instance.Alert("Se ha recibido un comando que no se puede procesar. Contacta con el desarollador para que solucione el problema", "Alerta", "Aceptar");

					break;
				}
			}

			return comandoRespuesta;
		}

		private void Procesar_EnviarTarea(Comando_EnviarTarea Comando)
		{
			lock(Global.TareasPersonalesLock)
			{
				Global.TareasPersonales.Add(Comando.Tarea);

				Global.TareasPersonales.Ordenar();
			}
		}

		private async void Procesar_RefrescarDisponibilidadArticulos(Comando_RefrescarDisponibilidadArticulos Comando)
		{
			await Device.InvokeOnMainThreadAsync(async () => 
				await Global.Get_Articulos() );
		}

		//private string Procesar_XXXXX(Comando_XXXXX Comando)
		//{
		//	//
		//}
	}
}
