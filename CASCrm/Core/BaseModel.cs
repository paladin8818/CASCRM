/*
 * Создано в SharpDevelop.
 * Пользователь: paladin
 * Дата: 04/25/2016
 * Время: 19:37
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.ComponentModel;

namespace CASCrm.Core
{
	/// <summary>
	/// Description of BaseModel.
	/// </summary>
	public class BaseModel : INotifyPropertyChanged
	{
		public BaseModel() {}
	    protected void RaisePropertyChanged(string name, object v) {
	        if (PropertyChanged != null) {
	            PropertyChanged(this, new PropertyChangedEventArgs(name));
	        }
	    }
	    public event PropertyChangedEventHandler PropertyChanged;
	    
		//TODO: Реализовать ф-ю выполнения удаленного запроса
	}
}
