/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 27.04.2016
 * Время: 18:17
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;

namespace HTTPManager
{
	/// <summary>
	/// Description of Download.
	/// </summary>
	public class Download
	{
		private Uri uri;
		
		public string LastError {
			get;
			private set;
		}
		
		public Download(Uri uri) {
			this.uri = uri;
		}
		
		public Download(string uri){
			this.uri = new Uri(uri);
		}
	}
}
