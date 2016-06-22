/*
 * Создано в SharpDevelop.
 * Пользователь: paladin
 * Дата: 25.04.2016
 * Время: 19:46
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HTTPManager
{
	public enum ContentType : int {
		ApplicationXWWWFormUrlencoded,
		MultipartFormData,
		TextHtml,
		ApplicationJSON
	};
}
