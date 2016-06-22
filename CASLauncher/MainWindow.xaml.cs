/*
 * Создано в SharpDevelop.
 * Пользователь: paladin
 * Дата: 27.04.2016
 * Время: 16:45
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using CASLib;

namespace CASLauncher
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IniFile settings = new IniFile("Settings.ini");
		
		private Brush defaultBrush = new SolidColorBrush(Color.FromRgb(158,158,158));
		private Brush errorBrush = new SolidColorBrush(Color.FromRgb(250, 200, 200));
		
		private string version;
		private string appName = "CASCrm.exe";
		
		public MainWindow()
		{
			InitializeComponent();
			this.init();
		}
		
		private void init() {
			if(this.settings.KeyExists("login")) 
				tbxLogin.Text = this.settings.Read("login");
			if(this.settings.KeyExists("version"))
				this.version = this.settings.Read("version");
			if(this.settings.KeyExists("host")) {
				tbxHost.Text = this.settings.Read("host");
			}
			if(this.settings.KeyExists("app")) {
				this.appName = this.settings.Read("app");
			}
			
			else {
				this.toggleHostSettingsVisible(true);
			}
			
			tbxLogin.TextChanged += 
				delegate { tbxLogin.Background = defaultBrush; };
			tbxPassword.PasswordChanged +=
				delegate { tbxPassword.Background = defaultBrush; };
			tbxHost.TextChanged += 
				delegate { tbxHost.Background = defaultBrush; };
			tbxPassword.KeyUp += 
				(sender, e) => { if(e.Key == Key.Enter) this.login(); };
			
			btnLogin.Click += delegate { this.login(); };
			
			lblClose.MouseUp += delegate { this.Close(); };
			
			cbxFullSettingsToggle.Click +=
				delegate { this.toggleHostSettingsVisible((bool)cbxFullSettingsToggle.IsChecked); };
			cbxFullSettingsToggle.Unchecked += 
				delegate { this.toggleHostSettingsVisible((bool)cbxFullSettingsToggle.IsChecked); };
			cbxFullSettingsToggle.Checked += 
				delegate { this.toggleHostSettingsVisible((bool)cbxFullSettingsToggle.IsChecked); };
			lblFullSettingToggle.MouseUp += 
				delegate { this.toggleHostSettingsVisible((bool)cbxFullSettingsToggle.IsChecked); };
		}
		
		private void toggleHostSettingsVisible (bool visible) {
			if(visible) {
				this.rowSettings.Visibility = Visibility.Visible;
			}
			else {
				this.rowSettings.Visibility = Visibility.Collapsed;
			}
		}
		
		private void login () {
			if(!this.validateFields()) return;
			Uri uri;
			try {
				uri = new Uri(tbxHost.Text);
				Settings.uri = uri;
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			Auth auth = new Auth(uri + "/auth");
			auth.UserLogin = tbxLogin.Text;
			auth.UserPassword = tbxPassword.Password;
			if(auth.auth()) {
				this.memberAuthData();
				if(!File.Exists(this.appName)) {
					this.version = "0.1";
				};
				//TODO: update //this.update();
				this.runApp(auth);
			}
			else {
				MessageBox.Show(auth.LastError);
			}
			
		}
		
		private bool validateFields() {
			if(tbxLogin.Text == "") {
				tbxLogin.Background = errorBrush;
				return false;
			}
			if(tbxPassword.Password == "") {
				tbxPassword.Background = errorBrush;
				return false;
			}
			if(tbxHost.Text == "") {
				tbxHost.Background = errorBrush;
				return false;
			}
			return true;
		}
		
		private void memberAuthData () {
			settings.Write("save", "true");
			settings.Write("login", tbxLogin.Text);
			settings.Write("host", tbxHost.Text);
			settings.Write("app", this.appName);
		}
		
		private void runApp (Auth auth) {
			Process app = new Process();
			app.StartInfo.FileName = this.appName;
			if(auth.AccessToken == null || auth.Id == null) {
				MessageBox.Show("Error");
				return;
			}
			app.StartInfo.Arguments = Settings.uri + " " + auth.AccessToken + " " + auth.Id.ToString();
			try {
				app.Start();
				this.Close();
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
		
		private void update () {
			Updater updater = new Updater(Settings.uri);
			this.lblCurrentStatus.Content = "Проверяю обновление...";
			Task<bool> checkUpdateTask = new Task<bool>(() => checkUpdate(updater));
			checkUpdateTask.Start();
			bool isExistUpdate = checkUpdateTask.Result;
			if(isExistUpdate) {
				
			}
			
			
			/*if(updater.CheckUpdate(this.version)) {
				if(updater.Update(this.appName)) {
					
				}
				else {
					MessageBox.Show(updater.LastError);
				}
			}*/
		}
		
		private bool checkUpdate (Updater updater) {
			bool result = updater.CheckUpdate(this.version);
			if(result == false && updater.LastError != null) {
				MessageBox.Show("Ошибка при проверке обновления: " + updater.LastError);
			}
			return result;
		}
	}
}