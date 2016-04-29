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
		public int Id {get; private set;}
		public string FullName {get; private set;}
		public string Login {get; private set;}
		public int IdRole {get; private set;}
		
		//Последняя ошибка, произошедшая при попытке авторизации или загрузке пользователя
		public string LastError {get; private set;}
		
		//Хранит куки текущего авторизованного пользователя
		private CookieContainer cookie;

		//Авторизован ли пользователь
		public bool IsAuth {get; private set;}
		
		//Значения, введенные пользователем для авторизации
		public string UserEntity {get;set;}
		public string UserLogin {get;set;}
		public string UserPassword {get;set;}
		
		public Auth(string url) {
			Settings.url = url;
		}
		
		public bool auth () {
			if(!this.checkAuthData()) {
				return false;
			}
			
			HTTPRequest http = HTTPRequest.Create(Settings.url);
			http.addParameter("entity", UserEntity);
			http.addParameter("login", UserLogin);
			http.addParameter("password", UserPassword);
			
			try {
				string response = http.post();
				
				if(this.parseResponse(response)) {
					IsAuth = true;
					cookie = http.CurrentCookie;
					return true;
				}
				else return false;
			}
			catch (Exception ex) {
				LastError = "Ошибка при выполнении http запроса: \n" + ex.Message ;
				return false;
			}
		}
		
		public bool loadCurrentUser () {
			if(Settings.url == "" || Settings.url == null) {
				LastError = "Пустой URL сервера данных.";
				return false;
			}
			HTTPRequest http = HTTPRequest.Create(Settings.url);
			http.addParameter("entity", UserEntity);
			
			try {
				string response = http.post();
				if(this.parseResponse(response)) {
					IsAuth = true;	
					cookie = http.CurrentCookie;					
					return true;
				}
				else return false;
			}
			catch (Exception ex) {
				LastError = "Ошибка при выполнении http запроса: \n" + ex.Message ;
				return false;
			}
		}
		
		private bool checkAuthData () {
			if(Settings.url == "" || Settings.url == null) {
				LastError = "Пустой URL сервера данных.";
				return false;
			}
			if(UserEntity == "" || UserEntity == null) {
				LastError = "Необходимый параметр (метод-контроллер) не был получен.";
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
			try {
				JArray result = JArray.Parse(response);
				if((Int32)result[0] != 0) {
					LastError = result[1].ToString();
					return false;
				}
				try {
					Dictionary<string, string> userData =
						result[1].ToObject<Dictionary<string, string>>();
					if(this.parseUserData(userData))
						return true;
					else
						return false;
				}
				catch(Exception ex) {
					LastError = ex.Message;
					return false;
				}
			}
			catch (Exception ex) {
				LastError = ex.Message;
				return false;
			}
		}
		
		private bool parseUserData (Dictionary<string, string> userData) {
			
			//Проверка, вернул ли сервер Id пользователя
			
			if(userData.ContainsKey("id")) {	
				try {
					Id = Int32.Parse(userData["id"]);
				}
				catch (Exception ex) {
					LastError = "Поступили некорректные данные (Id) от сервера.\n" +
						ex.Message;
					return false;
				}
			}
			else {
				LastError = "Сервер не вернул Id пользователя";
				return false;
			}
			
			//Проверка, вернул ли сервер корректный FullName пользователя
			
			if(userData.ContainsKey("name")) {
				FullName = userData["name"];
			}
			else {
				LastError = "Сервер не вернул Name пользователя";
				return false;
			}
			
			//Проверка, вернул ли сервер корректный Login пользователя
			
			if(userData.ContainsKey("login")) {
				Login = userData["login"];
			}
			else {
				LastError = "Сервер не вернул Login пользователя";
				return false;
			}
			
			//Проверка, вернул ли сервер корректный Role пользователя
			
			if(userData.ContainsKey("role")) {
				try {
					IdRole = Int32.Parse(userData["role"]);
				}
				catch (Exception ex) {
					LastError = "Поступили некорректные данные (Role) от сервера.\n" +
						ex.Message;
					return false;
				}
			}
			else {
				LastError = "Сервер не вернул Role пользователя";
				return false;
			}
			
			return true;
			
		}
	}
}
