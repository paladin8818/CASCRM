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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace HTTPManager
{
	/// <summary>
	/// Description of HTTPManager.
	/// </summary>
	/// 
	

	
	public class HTTPRequest
	{
		private static HTTPRequest _instanse = null;
		private Uri uri;
		private Dictionary<string, object> parameters = 
			new Dictionary<string, object>();
		private bool useCookie = false;
		private string contentType = "application/x-www-form-urlencoded";
		private string accept = "application/json";
		private string authorization = null;
		
		public static string LastError {
			get; private set;
		}
		
		private static CookieContainer cookieContainer = new CookieContainer();
		
		public CookieContainer CurrentCookie {
			get {
				return HTTPRequest.cookieContainer;
			}
			set {
				HTTPRequest.cookieContainer = value;
			}
		}
		
		public Uri Uri {
			get {return this.uri;}
			set {this.uri = value;}
		}
		
		public Dictionary<string, object> Parameters {
			get {return this.parameters;}
			set {this.parameters = value;}
		}
		public bool UseCookie {
			get {return this.useCookie;}
			set {this.useCookie = value;}
		}
		private string [] contentTypes = {
			"application/x-www-form-urlencoded",
			"multipart/form-data",
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
		
		public string Authorization {
			get {
				return this.authorization;
			}
			set {
				this.authorization = value;
			}
		}
		
		
		private HTTPRequest() {}
		
		private static HTTPRequest create (Uri uri) {
			HTTPRequest._instanse = new HTTPRequest();
			HTTPRequest._instanse.uri = uri;
			return HTTPRequest._instanse;
		}
		
		public static HTTPRequest Create(string uri) {
			try {
				Uri newUri = new Uri(uri);
				return HTTPRequest.create(newUri);
			}
			catch(Exception ex) {
				HTTPRequest.LastError = ex.Message;
				return null;
			}
		}
		
		public static HTTPRequest Create (Uri uri) {
			return HTTPRequest.create(uri);
		}
		
		public void addParameter (string k, object v) {
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
		
		public string get (string partUri = null, string json = null) {
			HttpWebRequest request = createRequest(partUri);
			request.Method = "GET";
			setRequestParams(request, json);
			return this.exec(request);
		}
		
		public string post (string partUri = null, string json = null) {
			HttpWebRequest request = createRequest(partUri);
			request.Method = "POST";
			setRequestParams(request, json);
			return this.exec(request);
		}
		
		public string put (string partUri = null, string json = null) {
			HttpWebRequest request = createRequest(partUri);
			request.Method = "PUT";
			setRequestParams(request, json);
			return this.exec(request);
		}
		
		public string delete (string partUri = null, string json = null) {
			HttpWebRequest request = createRequest(partUri);
			request.Method = "DELETE";
			setRequestParams(request, json);
			return this.exec(request);
		}

		private HttpWebRequest createRequest (string partUri) {
			return (HttpWebRequest)WebRequest.Create(this.uri + ((partUri != null) ? "/" + partUri : ""));
		}
		
		private void setRequestParams (HttpWebRequest request, string json = null) {
			if(this.useCookie) {
				request.CookieContainer = HTTPRequest.cookieContainer;
			}
			request.ContentType = this.contentType;
			request.Accept = this.accept;
			if(this.authorization != null) {
				request.Headers.Add("Authorization", this.authorization);
			}
			if(json != null) {
				this.writeByteParameters(json, request);
			}
			else {
				this.writeByteParameters(this.parametersToRequestString(), request);	
			}
		}
		
		//TODO: Реализовать асинхронное выполнение запросов
		//TODO: Реализовать загрузчик файлов
		
		
		private string exec (HttpWebRequest request, string json = null) {
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
			foreach(KeyValuePair<string, object> p in this.parameters) {
				parameters.Add(String.Format(p.Key + "={0}", p.Value.ToString()));
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
			HTTPRequest.cookieContainer = new CookieContainer();
		}
		
		public void ClearParameters () {
			this.parameters.Clear();
		}
	}
}