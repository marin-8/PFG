
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ProyectoFinal.Comun
{
	public class ControladorRed
	{
		public const ushort PUERTO = 1600;
		private const ushort MAX_BUFFER_SIZE = 300;

		public bool Recibiendo { get; private set; } = false;

		private readonly Socket Servidor;
		private readonly Action<string,string> FuncionAlRecibir;
		private readonly byte[] Buffer = new byte[MAX_BUFFER_SIZE];

		public static string Enviar(string IP, string Mensaje)
		{
			Socket socketDestino = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			/*int intentosDeConexion =*/ Enviar_ConectarConDestino(socketDestino, IP);
			Enviar_EnviarMensaje(socketDestino, Mensaje);
			string respuestaDelServidor = Enviar_RecibirRespuesta(socketDestino);
			Enviar_CerrarSockets(socketDestino);

			return respuestaDelServidor;
		}

		public ControladorRed(string IP, Action<string,string> FuncionAlRecibir, bool EmpezarRecibir)
		{
			Servidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			Servidor.Bind(new IPEndPoint(IPAddress.Parse(IP), PUERTO));

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

		private static /*int*/ void Enviar_ConectarConDestino(Socket Destino, string IP)
		{
			// int intentosDeConexion = 0;

			while(!Destino.Connected)
			{
				// intentosDeConexion++;

				try
				{
					Destino.Connect(new IPEndPoint(IPAddress.Parse(IP), PUERTO));
				}
				catch(SocketException) { }
			}

			// return intentosDeConexion;
		}

		private static void Enviar_EnviarMensaje(Socket Destino, string Mensaje)
		{
			byte[] data = Encoding.ASCII.GetBytes(Mensaje);

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
			string respuestaDestino = Encoding.ASCII.GetString(data);

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

            string mensajeRecibido = Encoding.ASCII.GetString(bufferRecibido);

			byte[] data = Encoding.ASCII.GetBytes("OK");
            cliente.Send(data);

			IPEndPoint clienteInfo = (IPEndPoint)cliente.RemoteEndPoint;
			string ipCliente = clienteInfo.Address.ToString();
			FuncionAlRecibir(ipCliente, mensajeRecibido);
        }

		#endregion
	}
}
