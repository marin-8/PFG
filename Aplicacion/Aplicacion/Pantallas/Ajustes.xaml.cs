
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class Ajustes : ContentPage
	{
		public Ajustes()
		{
			InitializeComponent();
		}

		private async void ComenzarJornadaConArticulosDisponibles_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			UserDialogs.Instance.ShowLoading("Modificando Nombre de Usuario...");

			bool estaActivado = ((SwitchCell)sender).On;

			await Task.Run(() =>
			{
				new Comando_ModificarAjusteComenzarJornadaConArticulosDisponibles(estaActivado).Enviar(Global.IPGestor);
			});

			UserDialogs.Instance.HideLoading();
		}
	}
}
