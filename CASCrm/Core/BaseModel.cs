/*
 * Создано в SharpDevelop.
 * Пользователь: paladin
 * Дата: 04/25/2016
 * Время: 19:37
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using CASLib;
using HTTPManager;

namespace CASCrm.Core
{
	/// <summary>
	/// Description of BaseModel.
	/// </summary>
	public abstract class BaseModel : INotifyPropertyChanged, IModel
	{
		
		public int Id {get;set;}
		public abstract string Link { get; }
		
		public BaseModel() {}
	    protected void RaisePropertyChanged(string name, object v) {
	        if (PropertyChanged != null) {
	            PropertyChanged(this, new PropertyChangedEventArgs(name));
	        }
	    }
		
	    public event PropertyChangedEventHandler PropertyChanged;
	    
	    protected abstract Dictionary<string, object> prepareSaveParams ();
	    
	    protected abstract void afterSave ();
	    protected abstract void afterRemove ();
	    
		public bool save()
		{
			HTTPRequest request = prepareHTTPRequest();
			try {
				string json = RequestConverter.Convert(prepareSaveParams());				
				if(Id == 0) {
					string response = request.post(null, json);
				}
				else {
					string response = request.put("/" + Id.ToString(), json);
				}
				afterSave();
				return true;
			}
			catch (Exception ex) {
				Log.WriteError("При выполнении удаленного запроса произошли ошибки. " + ex.Message);
				return false;
			}
		}
		
		public bool @remove()
		{
			HTTPRequest request = prepareHTTPRequest();
			try {
				string response = request.delete();
				afterRemove();
				return true;
			}
			catch (Exception ex) {
				Log.WriteError("При выполнении удаленного запроса произошли ошибки. " + ex.Message);
				return false;
			}
		}
		
		protected HTTPRequest prepareHTTPRequest () {
			HTTPRequest request = HTTPRequest.Create(Settings.uri + getLink());
			request.Authorization = "Bearer " + ToBase64(CurrentUser.instance().AccessToken);
			
			return request;
		}
		
		protected string ToBase64 (string token) {
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
		}
		
		protected string getLink () {
			if(Link[0] == '/') return Link;
			return "/" + Link;
		}
		
	}
}
