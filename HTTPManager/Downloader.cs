/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 27.04.2016
 * Время: 18:17
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.IO;
using System.Net;

namespace HTTPManager
{
	/// <summary>
	/// Description of Downloader.
	/// </summary>
	public class Downloader
	{
		private Uri uri;
		
		public string LastError {
			get;
			private set;
		}
		
		public Downloader(Uri uri) {
			this.uri = uri;
		}
		
		public Downloader(string uri){
			this.uri = new Uri(uri);
		}
		
		public bool Download (string path) {
			WebClient webClient = new WebClient();
			try {
				webClient.DownloadFile(this.uri, path);
				return true;
			}
			catch(Exception ex) {
				this.LastError = ex.Message;
				return false;
			}
		}
		
	}
}
