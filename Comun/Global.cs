
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

		public static readonly ReadOnlyCollection<KeyValuePair<Roles,TiposTareas>> RolesTareas =
			new( new KeyValuePair<Roles,TiposTareas>[]
		{
			new KeyValuePair<Roles,TiposTareas>( Roles.Camarero , TiposTareas.ServirArticulos ),
			new KeyValuePair<Roles,TiposTareas>( Roles.Camarero , TiposTareas.LimpiarMesa ),
			new KeyValuePair<Roles,TiposTareas>( Roles.Barista  , TiposTareas.PrepararArticulos ),
			new KeyValuePair<Roles,TiposTareas>( Roles.Cocinero , TiposTareas.PrepararArticulos ),
		});

		public static readonly ReadOnlyCollection<KeyValuePair<Roles,TiposAcciones>> RolesAcciones =
			new( new KeyValuePair<Roles,TiposAcciones>[]
		{
			new KeyValuePair<Roles,TiposAcciones>( Roles.Camarero , TiposAcciones.TomarNota ),
			new KeyValuePair<Roles,TiposAcciones>( Roles.Camarero , TiposAcciones.Cobrar ),
			new KeyValuePair<Roles,TiposAcciones>( Roles.Barista  , TiposAcciones.Cobrar ),
			new KeyValuePair<Roles,TiposAcciones>( Roles.Barista  , TiposAcciones.CambiarDisponibilidadArticulo ),
			new KeyValuePair<Roles,TiposAcciones>( Roles.Cocinero , TiposAcciones.CambiarDisponibilidadArticulo ),
		});
		
		public static readonly ReadOnlyDictionary<TiposTareas, string> TareasTitulos
			= new( new Dictionary<TiposTareas, string>
		{
			{ TiposTareas.ServirArticulos, "Servir artículos" },
			{ TiposTareas.LimpiarMesa, "Limpiar mesa" },
			{ TiposTareas.PrepararArticulos, "Preparar artículos" },
		});

		public static readonly ReadOnlyDictionary<TiposAcciones, string> AccionesTitulos
			= new( new Dictionary<TiposAcciones, string>
		{
			{ TiposAcciones.TomarNota, "Tomar nota" },
			{ TiposAcciones.Cobrar, "Cobrar" },
			{ TiposAcciones.CambiarDisponibilidadArticulo, "Cambiar estado\nde artículo\n(acabado/disponible)" },
		});

		public static AdaptadorDeRed[] Get_AdaptadoresDeRedDisponibles()
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

			return adaptadoresDeRedDisponibles.ToArray();
		}

		public static string Get_MiIP_Windows()
		{
			return
				Get_AdaptadoresDeRedDisponibles()
					.Where(ar => ar.IPs[0].ToString().Contains("192"))
					.Select(ar => ar.IPs[0])
					.First();
		}

		public static string Get_MiIP_Xamarin()
		{
			return 
				Dns.GetHostAddresses(Dns.GetHostName())
					.Where(IP => IP.ToString().Contains("192.168"))
					.First()
					.ToString();
		}
	}
}
