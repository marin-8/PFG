
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace PFG.Comun
{
	public class ControladorRed
	{
		public const ushort PUERTO = 1600;
		private const ushort MAX_BUFFER_SIZE = 10000;
		// TODO - Ajustar esto
		private const byte MAX_INTENTOS_CONEXION = 2; // ~ 6s/intento

		public bool Recibiendo { get; private set; } = false;

		private readonly Socket Servidor;
		private readonly Func<string, string,string> FuncionAlRecibir;
		private readonly byte[] Buffer = new byte[MAX_BUFFER_SIZE];

		public static string Enviar(string IP, string Mensaje, bool LimitarIntentos)
		{
			Socket socketDestino = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			if(!Enviar_ConectarConDestino(socketDestino, IP, LimitarIntentos))
				return null;

			Enviar_EnviarMensaje(socketDestino, Mensaje);

			string respuestaDelServidor =
				Enviar_RecibirRespuesta(socketDestino);

			Enviar_CerrarSockets(socketDestino);

			return respuestaDelServidor;
		}

		public ControladorRed(string IP, Func<string, string,string> FuncionAlRecibir, bool EmpezarRecibir, ushort Puerto = PUERTO)
		{
			Servidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			Servidor.Bind(new IPEndPoint(IPAddress.Parse(IP), Puerto));

			this.FuncionAlRecibir = FuncionAlRecibir;
			
			if(EmpezarRecibir) this.EmpezarRecibir();
		}

		public void EmpezarRecibir()
		{
			Servidor.Listen((int)SocketOptionName.MaxConnections);
            Servidor.BeginAccept(Servidor_NuevaConexion, null);

			Recibiendo = true;
		}

		public void Cerrar()
		{
			Recibiendo = false;

			if(Servidor.Connected) Servidor.Shutdown(SocketShutdown.Both);
			Servidor.Close();
		}

		#region Enviar (Funciones Privadas)

		private static bool Enviar_ConectarConDestino(Socket Destino, string IP, bool LimitarIntentos)
		{
			byte intentosDeConexion = 0;

			while(!Destino.Connected && (!LimitarIntentos || intentosDeConexion < MAX_INTENTOS_CONEXION))
			{
				intentosDeConexion++;

				try
				{
					Destino.Connect(new IPEndPoint(IPAddress.Parse(IP), PUERTO));
				}
				catch(SocketException) { }
			}

			return Destino.Connected;
		}

		private static void Enviar_EnviarMensaje(Socket Destino, string Mensaje)
		{
			byte[] data = Encoding.UTF8.GetBytes(Mensaje);

			Destino.Send(data);
		}

		private static string Enviar_RecibirRespuesta(Socket Destino)
		{
			byte[] buffer = new byte[MAX_BUFFER_SIZE];

			Destino.ReceiveTimeout = 10 * 1000; // 10s

            int bytesRecibidos = Destino.Receive(buffer);

			if (bytesRecibidos == 0) return "";

			byte[] data = new byte[bytesRecibidos];
			Array.Copy(buffer, data, bytesRecibidos);
			string respuestaDestino = Encoding.UTF8.GetString(data);

			return respuestaDestino;
		}

		private static void Enviar_CerrarSockets(Socket Destino)
		{
			Destino.Shutdown(SocketShutdown.Both);
			Destino.Close();
		}

		#endregion

		#region Servidor (Funciones Privadas)

		private void Servidor_NuevaConexion(IAsyncResult AR)
        {
            Socket cliente;

            try { cliente = Servidor.EndAccept(AR); } catch (ObjectDisposedException) { return; }
			
			cliente.BeginReceive(Buffer, 0, MAX_BUFFER_SIZE, SocketFlags.None, Servidor_Recibir, cliente);

			Servidor.BeginAccept(Servidor_NuevaConexion, null);
        }

		private void Servidor_Recibir(IAsyncResult AR)
        {
            Socket cliente = (Socket)AR.AsyncState;
            int numeroBytesRecibidos;

            try { numeroBytesRecibidos = cliente.EndReceive(AR); } catch (SocketException) { cliente.Close(); return; }

            byte[] bufferRecibido = new byte[numeroBytesRecibidos];
            Array.Copy(Buffer, bufferRecibido, numeroBytesRecibidos);

            string mensajeRecibido = Encoding.UTF8.GetString(bufferRecibido);

			IPEndPoint clienteInfo = (IPEndPoint)cliente.RemoteEndPoint;
			string ipCliente = clienteInfo.Address.ToString();
			string resupuesta = FuncionAlRecibir(ipCliente, mensajeRecibido);

			byte[] respuestaData = Encoding.UTF8.GetBytes(resupuesta);
			cliente.Send(respuestaData);
        }

		#endregion
	}
}
