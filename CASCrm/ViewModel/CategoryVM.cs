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
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;

using CASCrm.Core;
using CASCrm.Models;
using CASLib;
using HTTPManager;

namespace CASCrm.ViewModel
{
	/// <summary>
	/// Description of CategoryVM.
	/// </summary>
	public class CategoryVM : BaseViewModel
	{
		private static CategoryVM _instance = null;
		ObservableCollection<Category> _collection ;
		public ObservableCollection<Category> Collection {
			get {
				return _collection;
			}
			private set {
				_collection = value;
			}
		}

		private CategoryVM() {
			this.load();
		}
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

		public void remove (Category category) {
			if(this._collection.Contains(category)) {
				this._collection.Remove(category);
			}
		}
		
		protected override bool load()
		{
			HTTPRequest request = prepareHTTPRequest();
			try {				
				ResponseParser parser = new ResponseParser(request.get());
				if(!parser.Parse()) {
					Log.WriteError(parser.LastError);
					return false;
				}
				Collection = parser.ResponseDataToObject<ObservableCollection<Category>>();
				return true;
			}
			catch (Exception ex) {
				Log.WriteError("При выполнении удаленного запроса произошли ошибки. " + ex.Message);
				return false;
			}
		}
		
		public override string Link {
			get {
				return "/categories";
			}
		}
	}
}
