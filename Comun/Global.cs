
using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace PFG.Comun
{
	public static class Global
	{
		public const int MAX_CARACTERES_LOGIN = 20;

		public static List<AdaptadorDeRed> Get_AdaptadoresDeRedDisponibles()
		{
			List<AdaptadorDeRed> adaptadoresDeRedDisponibles = new();

			foreach(NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
			{
				if((networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
					networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
				   && networkInterface.OperationalStatus == OperationalStatus.Up)
				{
					AdaptadorDeRed nuevoAdaptadorDeRedDisponible = new() { Nombre=networkInterface.Name };

					foreach (UnicastIPAddressInformation ip in networkInterface.GetIPProperties().UnicastAddresses)
					{
						if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
						{
							nuevoAdaptadorDeRedDisponible.IPs.Add(ip.Address.ToString());
						}
					}

					adaptadoresDeRedDisponibles.Add(nuevoAdaptadorDeRedDisponible);
				}  
			}

			return adaptadoresDeRedDisponibles;
		}

		public static string Get_MiIP_Windows()
		{
			return
			(
				from ip in Get_AdaptadoresDeRedDisponibles()
				where ip.IPs[0].ToString().Contains("192")
				select ip.IPs[0]
			)
			.First();
		}

		public static string Get_MiIP_Xamarin()
		{
			return Dns.GetHostAddresses(Dns.GetHostName())
				.Where(IP => IP.ToString().Contains("192.168"))
				.First()
				.ToString();
		}
	}
}
