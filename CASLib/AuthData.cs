/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 22.06.2016
 * Время: 11:45
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;

namespace CASLib
{
	/// <summary>
	/// Description of AuthData.
	/// </summary>
	internal class AuthData
	{
		public int Id {get; set;}
		//public string FullName {get; set;}
		public string Username {get; set;}
		//public int IdRole {get; set;}
		public string AccessToken {get; set;}
		
		
		public AuthData() {}
	}
}
