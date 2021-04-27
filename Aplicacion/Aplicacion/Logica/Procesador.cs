
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PFG.Comun;

using Acr.UserDialogs;

namespace PFG.Aplicacion
{
	public class Procesador
	{
		public Procesador()
		{

		}

		public void Procesar(string IP, string ComandoString)
		{
			// TODO - Procesar (App)

			var parametrosComando = ComandoString.Split(',');
			var tipoComando = (TiposComando)Enum.Parse(typeof(TiposComando), parametrosComando[0]);

			switch(tipoComando)
			{
				case TiposComando.ResultadoDelIntentoDeIniciarSesion:
				{
					var comando = new Comando_ResultadoIntentoIniciarSesion(ComandoString);

					if(!comando.Correcto)
					{
						if(comando.ResultadIntentoIniciarSesion == ResultadosIntentoIniciarSesion.UsuarioNoExiste)
						{
							UserDialogs.Instance.Alert("El usuario no existe", "Alerta", "Aceptar");
						}
						else if(comando.ResultadIntentoIniciarSesion == ResultadosIntentoIniciarSesion.ContrasenaIncorrecta)
						{
							UserDialogs.Instance.Alert("Contraseña incorrecta", "Alerta", "Aceptar");
						}
					}
					else
					{
						Global.RolActual = comando.Rol;
					}

					break;
				}

				//case TiposComando.XXXXX:
				//{
				//	var comando = new Comando_XXXXX(ComandoString);

				//	//

				//	break;
				//}
			}
		}
	}
}
