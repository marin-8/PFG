
using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;

namespace PFG.Comun
{
	public class Global
	{
		public const byte MAX_CARACTERES_LOGIN = 20;
		public const string CARACTERES_PERMITIDOS_LOGIN = "abcdefghijklmnopqrstuvwxyzáéíóúABCDEFGHIJKLMNOPQRSTUVWXYZÁÉÍÓÚ0123456789";

		public const byte MAXIMO_COLUMNAS_MESAS = 8;
		public const byte MAXIMO_FILAS_MESAS = 12;

		public const float MINIMO_PRECIO_ARTICULO = 0.01f;
		public const float MAXIMO_PRECIO_ARTICULO = 999.99f;

		public static readonly Dictionary<Roles, TiposTareas> RolesTareas = new()
		{
			{ Roles.Camarero , TiposTareas.ServirArticulos },
			{ Roles.Camarero , TiposTareas.LimpiarMesa },
			{ Roles.Barista  , TiposTareas.PrepararArticulos },
			{ Roles.Cocinero , TiposTareas.PrepararArticulos },
		};

		public static readonly Dictionary<Roles, TiposAcciones> RolesAcciones = new()
		{
			{ Roles.Camarero , TiposAcciones.TomarNota },
			{ Roles.Camarero , TiposAcciones.Cobrar },
			{ Roles.Barista  , TiposAcciones.Cobrar },
			{ Roles.Barista  , TiposAcciones.MarcarArticuloComoAcabado },
			{ Roles.Barista  , TiposAcciones.MarcarArticuloComoDisponible },
			{ Roles.Cocinero , TiposAcciones.MarcarArticuloComoAcabado },
			{ Roles.Cocinero , TiposAcciones.MarcarArticuloComoDisponible },
		};

		public static readonly Dictionary<TiposTareas, string> TareasTitulos = new()
		{
			{ TiposTareas.ServirArticulos, "Servir artículos" },
			{ TiposTareas.LimpiarMesa, "Limpiar mesa" },
			{ TiposTareas.PrepararArticulos, "Preparar artículos" },
		};

		public static readonly Dictionary<TiposAcciones, string> AccionesTitulos = new()
		{
			{ TiposAcciones.TomarNota, "Tomar nota" },
			{ TiposAcciones.Cobrar, "Cobrar" },
			{ TiposAcciones.MarcarArticuloComoAcabado, "Marcar articulo como acabado" },
			{ TiposAcciones.MarcarArticuloComoDisponible, "Marcar articulo como disponible" },
		};

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
