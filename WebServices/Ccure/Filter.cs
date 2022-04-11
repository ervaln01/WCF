namespace WebServices.Ccure
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Класс фильтрации необходимой информации.
	/// </summary>
	public class Filter
	{
		/// <summary>
		/// Фильтрует информацию о сутруднике.
		/// </summary>
		/// <param name="unfilteredDict">Не отфильтрованный словарь.</param>
		/// <returns>Отфильтрованный словарь.</returns>
		public static Dictionary<string, string> FilterInfo(Dictionary<string, string> unfilteredDict)
		{
			var filteredDict = new Dictionary<string, string>
			{
				{ "PersonnelNumber", unfilteredDict["Text1"] }
			};

			var firstName = GetBilingualField(unfilteredDict["FirstName"], "FirstName");
			if (firstName != null)
				firstName.ToList().ForEach(x => filteredDict.Add(x.Key, x.Value));

			var lastName = GetBilingualField(unfilteredDict["LastName"], "LastName");
			if (lastName != null)
				lastName.ToList().ForEach(x => filteredDict.Add(x.Key, x.Value));
			return filteredDict;
		}

		/// <summary>
		/// Разделение двуязычных полей.
		/// </summary>
		/// <param name="field">Поле.</param>
		/// <param name="name">Название поля.</param>
		/// <returns>Словарь с разделенными двуязычными полями.</returns>
		private static Dictionary<string, string> GetBilingualField(string field, string name)
		{
			var dict = new Dictionary<string, string>();
			var splitedField = field.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			switch (splitedField.Length)
			{
				case 1:
					dict.Add($"{name}{GetLang(splitedField.First())}", splitedField.First());
					return dict;
				case 2:
					dict.Add($"{name}{GetLang(splitedField.First())}", splitedField.First());
					dict.Add($"{name}{GetLang(splitedField.Last())}", splitedField.Last());
					return dict;
				default:
					return null;
			}
		}

		/// <summary>
		/// Выбор языка поля.
		/// </summary>
		/// <param name="value">Значение поля.</param>
		/// <returns>Язык.</returns>
		private static string GetLang(string value) => (value[0] >= 'a' && value[0] <= 'z') || (value[0] >= 'A' && value[0] <= 'Z') ? "En" : "Ru";
	}
}