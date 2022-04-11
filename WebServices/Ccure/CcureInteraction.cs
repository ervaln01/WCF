namespace WebServices.Ccure
{
	using Newtonsoft.Json;

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Net.Http;

	/// <summary>
	/// Класс взаимодействия с ccure.
	/// </summary>
	public class CcureInteraction
	{
		/// <summary>
		/// Базовый адрес ccure.
		/// </summary>
		private const string httpBaseAddress = "http://tempUri/";
		
		/// <summary>
		/// Ссылка на API для получения информации.
		/// </summary>
		private const string urlCardInfoGet = "Api/Personnel/GetPersonnelDetail/";
		
		/// <summary>
		/// Номер карты сотрудника.
		/// </summary>
		private readonly string _cardno;

		/// <summary>
		/// Конструктор класса <see cref="CcureInteraction"/>.
		/// </summary>
		/// <param name="cardno">Номер карты сотрудника.</param>
		public CcureInteraction(string cardno) => _cardno = CardNoFormatting.GetCardNoFromAnyString(cardno);

		/// <summary>
		/// Получение информации о сотруднике.
		/// </summary>
		/// <returns>Словарь данных о сотруднике.</returns>
		public Dictionary<string, string> GetEmployeeInfo()
		{
			if (string.IsNullOrEmpty(_cardno))
				return null;

			var employeeInfo = new EmployeeInfo() { CardNo = _cardno };
			var stringContent = new StringContent(JsonConvert.SerializeObject(employeeInfo), Encoding.UTF8, "application/json");
			string response;

			try
			{
				using (var httpClient = new HttpClient { BaseAddress = new Uri(httpBaseAddress) })
				{
					var postResult = httpClient.PostAsync(urlCardInfoGet, stringContent).GetAwaiter().GetResult();
					response = postResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
				}
			}
			catch
			{
				return null;
			}

			if (string.IsNullOrEmpty(response))
				return null;

			try
			{
				var responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
				var information = responseDict.First().Value.ToString().Trim('{', '}');
				var splitedInfo = information
					.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
					.Select(x => x.Replace(",", string.Empty).Replace("\"", string.Empty).Trim());
				return splitedInfo.ToDictionary(x => x.Split(':').First().Trim(), x => x.Split(':').Last().Trim());
			}
			catch
			{
				return null;
			}
		}
	}
}