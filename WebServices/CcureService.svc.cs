namespace WebServices
{
	using System.Collections.Generic;

	using WebServices.Ccure;

	/// <summary>
	/// Класс методов сервиса.
	/// </summary>
	public class CcureService : ICcureService
	{
		/// <inheritdoc/>
		public Dictionary<string, string> GetEmployeeInfo(string cardno)
		{
			var ccure = new CcureInteraction(cardno);
			var employeeInfo = ccure.GetEmployeeInfo();
			return Filter.FilterInfo(employeeInfo);
		}
	}
}