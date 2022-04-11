namespace WebServices
{
	using System.Collections.Generic;
	using System.ServiceModel;

	/// <summary>
	/// Интерфейс подписки для подключения к службе.
	/// </summary>
	[ServiceContract]
	public interface ICcureService
	{
		/// <summary>
		/// Получение информации о сотруднике.
		/// </summary>
		/// <param name="cardno">Идентификатор пропуска.</param>
		/// <returns>Словарь данных о сотруднике.</returns>
		[OperationContract]
		Dictionary<string, string> GetEmployeeInfo(string cardno);
	}
}