
using System;


namespace PFG.Comun
{
	public static class ControladorRedExtensions
	{
		public static void Enviar(this Comando comando, string IP)
		{
			ControladorRed.Enviar(IP, comando.ToString());
		}
	}
}
