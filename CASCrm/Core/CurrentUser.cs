/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 06/22/2016
 * Время: 14:07
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;

namespace CASCrm.Core
{
	/// <summary>
	/// Description of CurrentUser.
	/// </summary>
	public class CurrentUser
	{
		private static CurrentUser _instance = null;
		
		public int Id { get; set; }
		public string Username { get; set; }
		public string AccessToken { get; set; }
		
		private CurrentUser() {}
		public static CurrentUser instance() {
			if(_instance == null) {
				_instance = new CurrentUser();
			}
			return _instance;
		}
	}
}
