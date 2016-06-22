/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 04.05.2016
 * Время: 17:52
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using Newtonsoft.Json.Linq;

namespace CASLib
{
	/// <summary>
	/// Класс парсинга json объекта ответа
	/// </summary>
	public class ResponseParser
	{
		private string jsonString;
		
		public string LastError {
			get;
			private set;
		}
		public bool IsServerError {
			get;
			private set;
		}
		public string ServerError {
			get;
			private set;
		}
		
		private JToken ServerResponse = null; 
		
		public ResponseParser(string jsonString) {
			this.jsonString = jsonString;
		}
		
		public bool Parse() {
			try {
				ServerResponse = JToken.Parse(this.jsonString);
				return true;
			}
			catch (Exception ex) {
				this.LastError = ex.Message + " (while parsing)";
				return false;
			}
		}
		
		public T ResponseDataToObject <T> () {
			try {
				if(this.ServerResponse == null) {
					this.LastError = "Server response data is null!";
					return default(T);
				}
				return this.ServerResponse.ToObject<T>();
			}
			catch (Exception ex) {
				this.LastError = ex.Message + " (during conversion)";
				return default(T);
			}
		}
	}
}
