/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 27.04.2016
 * Время: 18:03
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Collections.Generic;
using System.Net;

using HTTPManager;
using Newtonsoft.Json.Linq;

namespace CASLib
{
	/// <summary>
	/// Description of Auth.
	/// </summary>
	public class Auth
	{
		private AuthData authData = null;
		
		public int? Id {
			get {
				if(authData != null) {
					return authData.Id;
				}
				return null;
			}
		}
		
		public string AccessToken {
			get {
				if(authData != null) {
					return authData.AccessToken;
				}
				return null;
			}
		}
		
		//Uri ресурса, на которм авторизуется пользователяь 
		private Uri uri;
		
		//Последняя ошибка, произошедшая при попытке авторизации или загрузке пользователя
		public string LastError {get; private set;}

		//Словарь "авторизаций"
		//Ключ - строка url, значение - экземпляр класса Auth 
		//с данными авторизованного пользователя
		// ПРЕДУСМАТРИВАЕТСЯ ВОЗМОЖНОСТЬ АВТОРИЗАЦИИ НА РАЗНЫХ ДОМЕНАХ В ОДНОМ ПРИЛОЖЕНИИ
		private static Dictionary<string, Auth> AuthDict = null;
		
		//Инициализация словаря "авторизаций"
		public void InitAuthDict () {
			if(Auth.AuthDict == null) {
				Auth.AuthDict = new Dictionary<string, Auth>();
			}
		}
		
		//Получить экземпляр класса Auth если пользователь авторизован на этом ресурсе
		//Иначе null
		public static Auth AuthByUri (Uri uri) {
			if(Auth.AuthDict.ContainsKey(uri.ToString())) {
				return Auth.AuthDict[uri.ToString()];
			}
			return null;
		}
		
		//Значения, введенные пользователем для авторизации
		public string UserLogin {get;set;}
		public string UserPassword {get;set;}
		
		public Auth(string uri) {
			this.uri = new Uri(uri);
			this.InitAuthDict();
		}

		public Auth(Uri uri) {
			this.uri = uri;			
			this.InitAuthDict();
		}
		
		public bool auth () {
			if(!this.checkAuthData()) {
				return false;
			}
			HTTPRequest http = HTTPRequest.Create(this.uri);
			http.Authorization = "Basic " + authDataToB64(UserLogin, UserPassword);
			try {
				string response = http.get();				
				if(this.parseResponse(response)) {
					Auth.AuthDict[this.uri.ToString()] = this;
					return true;
				}
				else {
					return false;
				}
			}
			catch (Exception ex) {
				LastError = "Ошибка при выполнении http запроса: \n" + ex.Message ;				
				return false;
			}
		}
				
		private bool checkAuthData () {
			if(this.uri == null) {
				LastError = "Пустой URL сервера данных.";
				return false;
			}
			if(UserLogin == "" || UserLogin == null) {
				LastError = "Необходимый параметр (логин) не был получен.";
				return false;
			}
			if(UserPassword == "" || UserPassword == null) {
				LastError = "Необходимый параметр (логин) не был получен.";
				return false;
			}
			return true;
		}
		
		private bool parseResponse(string response) {
			//парсим ответ			
			ResponseParser responseParser = new ResponseParser(response);
			//если нет ошибок
			if(responseParser.Parse() && responseParser.IsServerError == false) {
				authData = responseParser.ResponseDataToObject<AuthData>();
				if(authData != null) {
					return true;
				}
				else {
					LastError = (responseParser.LastError != null) ? responseParser.LastError : LastError;
					return false;
				}
			}
			else {
				LastError = "Ошибка 2: " + ((responseParser.ServerError == null) ? 
				                          responseParser.LastError : responseParser.ServerError);
				return false;
			}
		}
		
		private string authDataToB64 (string token, string password = null) {
			if(password != null) {
				return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(token + ":" + password));
			}
			return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(token));
		}
		
	}
}
