/*
 * Создано в SharpDevelop.
 * Пользователь: paladin
 * Дата: 04/25/2016
 * Время: 19:09
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.ObjectModel;
using System.Linq;
using CASCrm.Models;

namespace CASCrm.ViewModel
{
	/// <summary>
	/// Description of CategoryVM.
	/// </summary>
	public class CategoryVM
	{
		private static CategoryVM _instance = null;
		ObservableCollection<Category> _collection = new ObservableCollection<Category>();
		private CategoryVM() {}
		public static CategoryVM instance () {
			if(CategoryVM._instance == null) {
				CategoryVM._instance = new CategoryVM();
			}
			return CategoryVM._instance;
		}
		public void add(Category category) {
			this._collection.Add(category);
		}
		public Category getById(int id) {
			return this._collection.FirstOrDefault(x => x.Id == id);
		}
		public ObservableCollection<Category> load () {
			
		}
		public void remove (Category category) {
			if(this._collection.Contains(category)) {
				this._collection.Remove(category);
			}
		}
	}
}
