/*
 * Создано в SharpDevelop.
 * Пользователь: paladin
 * Дата: 04/25/2016
 * Время: 19:09
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.Generic;
using CASCrm.ViewModel;

namespace CASCrm.Models
{
	/// <summary>
	/// Description of Category.
	/// </summary>
	public class Category : Core.BaseModel
	{
		public override string Link {
			get {
				return "/categories";
			}
		}
		private static CategoryVM viewModel = CategoryVM.instance();
		
		public string Title { get; set; }
		public int Row_order { get; set; }
		
		public Category(){}
		
		protected override Dictionary<string, object> prepareSaveParams()
		{
			Dictionary<string, object> parameters =
				new Dictionary<string, object> ();
			parameters.Add("title", Title);
			parameters.Add("row_order", Row_order);
			
			return parameters;
		}
		
		protected override void afterSave()
		{
			viewModel.add(this);
		}
		
		protected override void afterRemove()
		{
			viewModel.remove(this);
		}
	}
}
