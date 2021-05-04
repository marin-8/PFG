
using System;


namespace PFG.Comun
{
	public static class ControladorRedExtensions
	{
		public static string  Enviar(this Comando comando, string IP)
		{
			return ControladorRed.Enviar(IP, comando.ToString());
		}
	}
}
