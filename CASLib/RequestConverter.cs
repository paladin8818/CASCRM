/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 22.06.2016
 * Время: 15:42
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using Newtonsoft.Json;

namespace CASLib
{
	/// <summary>
	/// Description of RequestConverter.
	/// </summary>
	public class RequestConverter
	{
		private RequestConverter() {}
		
		public static string Convert <T> (T t) {
			return JsonConvert.SerializeObject(t, Formatting.Indented);
		}
		
	}
}
