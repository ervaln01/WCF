namespace WebServices.Ccure
{
	using System;
	using System.Linq;

	/// <summary>
	/// Форматирование номера карты сотрудника.
	/// </summary>
	public class CardNoFormatting
	{
		/// <summary>
		/// Проверочная строка для 16-ичной системы.
		/// </summary>
		private const string HexVerifyString = "0123456789abcdef";

		/// <summary>
		/// Проверочная строка для 2-ичной системы.
		/// </summary>
		private const string BinVerifyString = "01";

		/// <summary>
		/// Конфигурационные данные <add key="48CNSbit" value="25|23"/>. Начальная позиция вырезаемой строки.
		/// </summary>
		private const int BinStartPosition = 25;

		/// <summary>
		/// Конфигурационные данные <add key="48CNSbit" value="25|23"/>. Длина вырезаемой строки.
		/// </summary>
		private const int BinLength = 23;

		/// <summary>
		/// Конфигурационные данные <add key="48CNSbit" value="25|23"/>. Длина всей строки.
		/// </summary>
		private const int BinMaxLength = 48;

		/// <summary>
		/// Форматирует номер пропуска из любой полученной строки.
		/// </summary>
		/// <param name="cardNo">Номер пропуска сотрудника.</param>
		/// <returns>Отформатированный номер пропуска.</returns>
		public static string GetCardNoFromAnyString(string cardNo)
		{
			if (string.IsNullOrEmpty(cardNo)) return null;

			var enCardNo = ChangeLang(cardNo.ToLower());
			if (enCardNo.StartsWith("cardno")) return GetCardNoFromHex(enCardNo.Substring(6));
			if (enCardNo.All(x => BinVerifyString.Contains(x))) return GetCardNoFromBinary(enCardNo);
			if (enCardNo.All(x => char.IsDigit(x))) return GetCardNoFromDecimal(enCardNo);
			if (enCardNo.All(x => HexVerifyString.Contains(x))) return GetCardNoFromHex(enCardNo);
			return null;
		}

		/// <summary>
		/// Форматирует 16-ичную строку.
		/// </summary>
		/// <param name="hexString">16-ичная строка.</param>
		/// <returns>Отформатированный номер пропуска.</returns>
		private static string GetCardNoFromHex(string hexString)
		{
			if (string.IsNullOrEmpty(hexString)) return null;
			if (!hexString.All(x => HexVerifyString.Contains(x))) return null;
			return GetCardNoFromDecimal(Convert.ToInt64(hexString, 16));
		}

		/// <summary>
		/// Форматирует 16-ичное число.
		/// </summary>
		/// <param name="decimalNumber">16-ичное число.</param>
		/// <returns>Отформатированный номер пропуска.</returns>
		private static string GetCardNoFromDecimal(long decimalNumber)
		{
			var binaryString = Convert.ToString(decimalNumber, 2);
			return GetCardNoFromBinary(binaryString);
		}

		/// <summary>
		/// Форматирует 10-ичную строку.
		/// </summary>
		/// <param name="decimalString">10-ичная строка.</param>
		/// <returns>Отформатированный номер пропуска.</returns>
		private static string GetCardNoFromDecimal(string decimalString)
		{
			if (string.IsNullOrEmpty(decimalString)) return null;
			if (!decimalString.All(x => char.IsDigit(x))) return null;
			return GetCardNoFromDecimal(Convert.ToInt64(decimalString));
		}

		/// <summary>
		/// Форматирует 2-ичную строку.
		/// </summary>
		/// <param name="binaryString">2-ичная строка.</param>
		/// <returns>Отформатированный номер пропуска.</returns>
		private static string GetCardNoFromBinary(string binaryString)
		{
			if (string.IsNullOrEmpty(binaryString)) return null;
			if (!binaryString.All(x => BinVerifyString.Contains(x))) return null;
			if (binaryString.Length < BinMaxLength) binaryString = binaryString.PadLeft(BinMaxLength);
			var subBinaryString = binaryString.Substring(BinStartPosition - 1, BinLength);
			return Convert.ToInt64(subBinaryString, 2).ToString();
		}

		/// <summary>
		/// Изменение раскладки клавиатуры.
		/// </summary>
		/// <param name="cardNoAnyLang">Номер карты на английском или русском языке.</param>
		/// <returns>Номер карты на английском языке.</returns>
		private static string ChangeLang(string cardNoAnyLang)
		{
			var cardNoEn = string.Empty;
			foreach (var c in cardNoAnyLang)
			{
				switch (c)
				{
					case 'ф': cardNoEn += 'a'; break;
					case 'и': cardNoEn += 'b'; break;
					case 'с': cardNoEn += 'c'; break;
					case 'в': cardNoEn += 'd'; break;
					case 'у': cardNoEn += 'e'; break;
					case 'а': cardNoEn += 'f'; break;
					case 'к': cardNoEn += 'r'; break;
					case 'т': cardNoEn += 'n'; break;
					case 'щ': cardNoEn += 'o'; break;

					default: cardNoEn += c; break;
				}
			}
			return cardNoEn;
		}
	}
}