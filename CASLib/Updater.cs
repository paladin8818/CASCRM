/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 27.04.2016
 * Время: 17:07
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using HTTPManager;

namespace CASLib
{
	/// <summary>
	/// Description of Updater.
	/// </summary>
	public class Updater
	{
		private Uri uri;
		private Uri updatePath;
		
		public string LastError {
			get; 
			private set;
		}
		
		public Updater(Uri uri) {
			this.uri = uri;
		}

		//Функция проверки обновлений
		public bool CheckUpdate (string currentVersion) {
			//Проверка - авторизован ли пользователь
			if(!this.checkAuth()) {
				LastError = "Перед обновлением необходимо авторизоваться.";
				return false;
			}
			
			//Запрос на проверку версии
			HTTPRequest httpRequest = HTTPRequest.Create(this.uri);
			httpRequest.addParameter("entity", "Versions/check");
			httpRequest.addParameter("current", currentVersion);
			try {
				ResponseParser response = new ResponseParser(httpRequest.post());
				
				if(response.Parse() && response.IsServerError == false) {
					string uri = response.ResponseDataToObject<string>();
					this.updatePath = new Uri(uri);
					return true;
				}
				else {
					LastError = (response.ServerError == null) ?
						response.LastError : response.LastError;
					return false;
				}
			}
			catch (Exception ex) {
				LastError = ex.Message + " (" + this.uri.AbsoluteUri + ")";
				return false;
			}
		}
		
		//Функция, выполняющаяя обновление программы
		public bool Update(string path) {
			if(this.updatePath == null) {
				LastError = "Невозможно скачать обновление.\nСервер не вернул путь до обновления.";
				return false;
			}
			Downloader downloader = new Downloader(this.updatePath);
			if(downloader.Download(path)) {
				return true;
			}
			else {
				LastError = downloader.LastError;
				return false;
			}
		}
		
		//Функция проверки авторизован ли пользователь
		private bool checkAuth () {
			if(Auth.AuthByUri(this.uri) != null) {
				return true;
			}
			return false;
		}
		
	}
}
