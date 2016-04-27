/*
 * Создано в SharpDevelop.
 * Пользователь: paladin
 * Дата: 25.04.2016
 * Время: 19:46
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HTTPManager
{
	/// <summary>
	/// Description of HTTPManager.
	/// </summary>
	/// 
	
	public enum ContentType : int {
		ApplicationXWWWFormUrlencoded,
		TextHtml,
		ApplicationJSON
	};
	
	public class HTTPManager
	{
		private static HTTPManager _instanse = null;
		private string uri;
		private Dictionary<string, string> parameters = 
			new Dictionary<string, string>();
		private bool useCookie = true;
		private string contentType = "application/x-www-form-urlencoded";
		
		private static CookieContainer cookieContainer = new CookieContainer();
		
		public string Uri {
			get {return this.uri;}
			set {this.uri = value;}
		}
		
		public Dictionary<string, string> Parameters {
			get {return this.parameters;}
			set {this.parameters = value;}
		}
		public bool UseCookie {
			get {return this.useCookie;}
			set {this.useCookie = value;}
		}
		private string [] contentTypes = {
			"application/x-www-form-urlencoded",
			"text/html",
			"application/json"
		};
		public ContentType ContentType {
			get {
				int methodIndex = Array.IndexOf(this.contentTypes, this.contentType);
				return (ContentType)methodIndex;
			}
			set {
				this.contentType = this.contentTypes[(int)value];
			}
		}
		
		private HTTPManager() {}
		
		private static HTTPManager create (string uri) {
			HTTPManager._instanse = new HTTPManager();
			HTTPManager._instanse.uri = uri;
			return HTTPManager._instanse;
		}
		
		public static HTTPManager Create(string uri) {
			return HTTPManager.create(uri);
		}
		
		public void addParameter (string k, string v) {
			if(this.parameters.ContainsKey(k)) {
				this.parameters[k] = v;
			}
			else {
				this.parameters.Add(k, v);
			}
		}
		
		public void removeParameter (string k) {
			if(this.parameters.ContainsKey(k)) {
				this.parameters.Remove(k);
			}
		}
		
		public string get () {
			HttpWebRequest request = 
				(HttpWebRequest)WebRequest.Create(this.uri + "?" + this.parametersToRequestString());
			request.Method = "GET";
			if(this.useCookie) {
				request.CookieContainer = HTTPManager.cookieContainer;
			}
			request.ContentType = this.contentType;
			return this.exec(request);
		}
		
		public string post () {
			HttpWebRequest request = 
				(HttpWebRequest)WebRequest.Create(this.uri);
			request.Method = "POST";
			if(this.useCookie) {
				request.CookieContainer = HTTPManager.cookieContainer;
			}
			request.ContentType = this.contentType;
			this.writeByteParameters(this.parametersToRequestString(), request);			
			return this.exec(request);
		}
		
		//TODO: Реализовать методы put и delete
		//TODO: Реализовать асинхронное выполнение запросов
		//TODO: Реализовать загрузчик файлов
		
		
		private string exec (HttpWebRequest request) {
			WebResponse response = request.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream(),
			                                       Encoding.UTF8);
			string responseString = reader.ReadToEnd();
			response.Close();
			reader.Close();
			this.ClearParameters();
			return responseString;
		}
		
		private string parametersToRequestString () {
			// disable once LocalVariableHidesMember
			ArrayList parameters = new ArrayList();
			foreach(KeyValuePair<string, string> p in this.parameters) {
				parameters.Add(String.Format(p.Key + "={0}", p.Value));
			}
			return String.Join("&", parameters.ToArray());
		}
		
		// disable once ParameterHidesMember
		private void writeByteParameters (string parameters, HttpWebRequest request) {
			byte [] byteParameters = Encoding.UTF8.GetBytes(parameters);
			if(byteParameters.Length == 0) {
				return;
			}
			request.ContentLength = byteParameters.Length;
			Stream dataStream = request.GetRequestStream();
			dataStream.Write(byteParameters, 0, byteParameters.Length);
			dataStream.Close();
		}
		
		public static void ClearCookie () {
			HTTPManager.cookieContainer = new CookieContainer();
		}
		
		public void ClearParameters () {
			this.parameters.Clear();
		}
	}
}