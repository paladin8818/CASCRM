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
using System.Text;
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
		private Updater updater = new Updater();
		
		private Brush defaultBrush = new SolidColorBrush(Color.FromRgb(250, 250, 250));
		private Brush errorBrush = new SolidColorBrush(Color.FromRgb(250, 200, 200));
		
		private string version;
		
		public MainWindow()
		{
			InitializeComponent();
			
			this.init();
		}
		
		private void init() {
			if(this.settings.KeyExists("login")) 
				textBoxLogin.Text = this.settings.Read("login");
			if(this.settings.KeyExists("version"))
				this.version = this.settings.Read("version");
			if(this.settings.KeyExists("host")) {
				textBoxHost.Text = this.settings.Read("host");
			}
			else {
				this.toggleHostSettingsVisible(true);
			}
			
			textBoxLogin.TextChanged += 
				delegate { textBoxLogin.Background = defaultBrush; };
			passwordBoxPassword.PasswordChanged +=
				delegate { passwordBoxPassword.Background = defaultBrush; };
			textBoxHost.TextChanged += 
				delegate { textBoxHost.Background = defaultBrush; };
			passwordBoxPassword.KeyUp += 
				(sender, e) => { if(e.Key == Key.Enter) this.login(); };
			
			buttonLogIn.Click += delegate { this.login(); };
			
			checkBoxExtSettingsToggle.Click +=
				delegate { this.toggleHostSettingsVisible((bool)checkBoxExtSettingsToggle.IsChecked); };
			
		}
		
		private void toggleHostSettingsVisible (bool visible) {
			if(visible) {
				this.Height = 280;
				this.groupBoxExtSettings.Visibility = Visibility.Visible;
			}
			else {
				this.Height = 225;
				this.groupBoxExtSettings.Visibility = Visibility.Collapsed;
			}
		}
		
		private void login () {
			if(!this.validateFields()) return;
			
			Auth auth = new Auth(textBoxHost.Text);
			auth.UserEntity = "Auth/auth";
			auth.UserLogin = textBoxLogin.Text;
			auth.UserPassword = passwordBoxPassword.Password;
			if(auth.auth()) {
				this.memberAuthData();
			}
			else {
				MessageBox.Show(auth.LastError);
			}
			
		}
		
		private bool validateFields() {
			if(textBoxLogin.Text == "") {
				textBoxLogin.Background = errorBrush;
				return false;
			}
			if(passwordBoxPassword.Password == "") {
				passwordBoxPassword.Background = errorBrush;
				return false;
			}
			if(textBoxHost.Text == "") {
				textBoxHost.Background = errorBrush;
				return false;
			}
			return true;
		}
		
		private void memberAuthData () {
			settings.Write("save", "true");
			settings.Write("login", textBoxLogin.Text);
			settings.Write("host", textBoxHost.Text);
		}
	}
}