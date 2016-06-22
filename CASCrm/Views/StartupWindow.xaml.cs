/*
 * Created by SharpDevelop.
 * User: paladin
 * Date: 25.04.2016
 * Time: 19:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using CASCrm.Core;
using CASCrm.Models;
using CASCrm.ViewModel;
using CASLib;

namespace CASCrm.Views
{
	/// <summary>
	/// Interaction logic for StartupWindow.xaml
	/// </summary>
	public partial class StartupWindow : Window
	{
		public StartupWindow()
		{
			InitializeComponent();
			
			string[] args = Environment.GetCommandLineArgs();			
			if(args.Count() != 4 ) {
				MessageBox.Show("Из лаунчера не были переданы необходимые аргументы.\nПриложение будет закрыто.");
				Log.WriteError("Из лаунчера не были переданы необходимые аргументы. Приложение будет закрыто.");
				Close();
			}
			try {
				Settings.uri = new Uri(args[1]);
			}
			catch (Exception ex) {
				MessageBox.Show("Получен некорректный адрес сервера.\nПриложение будет закрыто.");
				Log.WriteError("Получен некорректный адрес сервера. Приложение будет закрыто. " + ex.Message);
				Close();
			}
			CurrentUser currentUser = CurrentUser.instance();
			currentUser.AccessToken = args[2];
			try {
				currentUser.Id = Int32.Parse(args[3]);
			}
			catch (Exception ex) {
				MessageBox.Show("Получен некорректный идентификатор пользователя.\nПриложение будет закрыто.");
				Log.WriteError("Получен некорректный идентификатор пользователя. Приложение будет закрыто. " + ex.Message);
				Close();
			}
			
			loadModels();
			loadMainWindow();
		}
		
		private void loadMainWindow () {
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
			Close();
		}
		
		private void loadModels () {
			CategoryVM.instance();
		}
	}
}