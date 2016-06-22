/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 06/22/2016
 * Время: 14:35
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;

namespace CASCrm.Models
{
	/// <summary>
	/// Description of Man.
	/// </summary>
	public abstract class Man
	{
		public string SurName {get; set;}
		public string FirstName {get;set;}
		public string Patronymic {get;set;}
		public string FullName {
			get {
				return SurName + " " + FirstName + " " + Patronymic;
			}
		}
		
	}
}
