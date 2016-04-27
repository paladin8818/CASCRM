/*
 * Создано в SharpDevelop.
 * Пользователь: paladin
 * Дата: 04/25/2016
 * Время: 19:09
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;

namespace CASCrm.Models
{
	/// <summary>
	/// Description of Category.
	/// </summary>
	public class Category : Core.BaseModel
	{
		
		public int Id { get; set; }
		public string Title { get; set; }
		
		public Category(){}
		
		public static void load () {
			
		}
		
		public void save () {
			
		}
		
		public void remove () {
			
		}
	}
}
