/*
 * Сделано в SharpDevelop.
 * Пользователь: Дмитрий
 * Дата: 22.06.2016
 * Время: 16:19
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using CASLib;
using HTTPManager;

namespace CASCrm.Core
{
	/// <summary>
	/// Description of BaseViewModel.
	/// </summary>
	public abstract class BaseViewModel
	{
		public abstract string Link { get; }
		protected abstract bool load ();
		
		
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
