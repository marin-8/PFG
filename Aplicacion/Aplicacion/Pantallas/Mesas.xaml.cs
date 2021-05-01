
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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Acr.UserDialogs;

using PFG.Comun;

namespace PFG.Aplicacion
{
	public partial class Mesas : ContentPage
	{
		public Mesas()
		{
			InitializeComponent();

			InicializarMapaGrid();
		}

		private void InicializarMapaGrid()
		{
			int columnas = 3;
			int filas = 5;

			MapaGrid.ColumnDefinitions.Clear();
			MapaGrid.RowDefinitions.Clear();

			for(int c = 0 ; c < columnas-1; c++)
			{
				MapaGrid.ColumnDefinitions.Add(new());
				MapaGrid.ColumnDefinitions.Add(new(){Width=new(20)});
			}

			MapaGrid.ColumnDefinitions.Add(new());

			for(int f = 0 ; f < filas-1 ; f++)
			{
				MapaGrid.RowDefinitions.Add(new());
				MapaGrid.RowDefinitions.Add(new(){Height=new(20)});
			}

			MapaGrid.RowDefinitions.Add(new());

			for(int c = 0 ; c < columnas; c++)
			{
				for(int f = 0 ; f < filas ; f++)
				{
					var button = new Button(){
						FontAttributes=FontAttributes.Bold,
						TextColor=Color.White, 
						FontSize=16,
						BackgroundColor=Color.Blue,
						BindingContext=$"{c+1}.{f+1}"};
					button.Clicked += Cell_Clicked;

					MapaGrid.Children.Add(button, c*2, f*2);
				}
			}
		}

		private void Cell_Clicked(object sender, EventArgs e)
		{
			var mesa = (string)((Button)sender).BindingContext;
		}

		private void AnadirFila_Clicked(object sender, EventArgs e)
		{
			MapaGrid.RowDefinitions.Add(new(){Height=new(20)});
			MapaGrid.RowDefinitions.Add(new());

			int columnas = MapaGrid.ColumnDefinitions.Count;
			int filas = MapaGrid.RowDefinitions.Count;

			for (int c = 0; c < (columnas+1)/2; c++)
			{
				var button = new Button()
				{
					FontAttributes = FontAttributes.Bold,
					TextColor = Color.White,
					FontSize = 16,
					BackgroundColor = Color.Blue,
					BindingContext=$"{c+1}.{(filas+1)/2}"
				};
				button.Clicked += Cell_Clicked;

				MapaGrid.Children.Add(button, c*2, filas-1);
			}
		}

		private void EliminarFila_Clicked(object sender, EventArgs e)
		{

		}

		private void AnadirColumna_Clicked(object sender, EventArgs e)
		{
			MapaGrid.ColumnDefinitions.Add(new(){Width=new(20)});
			MapaGrid.ColumnDefinitions.Add(new());

			int columnas = MapaGrid.ColumnDefinitions.Count;
			int filas = MapaGrid.RowDefinitions.Count;

			for (int f = 0 ; f < (filas+1)/2; f++)
			{
				var button = new Button()
				{
					FontAttributes = FontAttributes.Bold,
					TextColor = Color.White,
					FontSize = 16,
					BackgroundColor = Color.Blue,
					BindingContext=$"{columnas+1}.{(f+1)/2}"
				};
				button.Clicked += Cell_Clicked;

				MapaGrid.Children.Add(button, columnas-1, f*2);
			}
		}

		private void EliminarColumna_Clicked(object sender, EventArgs e)
		{

		}
	}
}
