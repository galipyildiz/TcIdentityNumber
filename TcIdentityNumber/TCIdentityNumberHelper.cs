using System.Text;

namespace TCIdentityNumber
{
    public static class TCIdentityNumberHelper
    {
        private const string URL = @"https://tckimlik.nvi.gov.tr/Service/KPSPublic.asmx";
        private const string ACTION = @"http://tckimlik.nvi.gov.tr/WS/TCKimlikNoDogrula";
        /// <summary>
        /// This method calculates the accuracy of the TC Identity Number. It is not use any third party app. Just use algorithm.
        /// </summary>
        public static bool IsIdentityNumberCorrect(this long identityNumber)
        {

            if (!IsLengthEqualElevenAndFirstDigitNotEqualZero(identityNumber.ToString()))
                return false;

            if (!IsTenthDigitEqualCalculatedNumber(identityNumber.ToString()))
                return false;

            if (!IsEleventhDigitEqualCalculatedNumber(identityNumber.ToString()))
                return false;

            return true;
        }

        /// <summary>
        /// This method calculas the accuracy of the TC Identity Number. It use https://tckimlik.nvi.gov.tr/ soap service.
        /// </summary>
        public static async Task<bool> IsIdentityNumberCorrectAsync(this long identityNumber, string name, string surname, int birthYear)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || !IsBirthYearDigitEqualFour(birthYear.ToString()))
                return false;

            if (!identityNumber.IsIdentityNumberCorrect())
                return false;

            if (!await IsIdentityNumberCorrectWithService(name, surname, birthYear, identityNumber))
                return false;

            return true;
        }


        /// <summary>
        /// Check Identity Number with https://tckimlik.nvi.gov.tr/Service/KPSPublic.asmx soap service.
        /// </summary>
        private static async Task<bool> IsIdentityNumberCorrectWithService(string name, string surName, int birthyear, long identityNumber)
        {
            string xmlSOAP = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <TCKimlikNoDogrula xmlns=""http://tckimlik.nvi.gov.tr/WS"">
      <TCKimlikNo>{identityNumber}</TCKimlikNo>
      <Ad>{name}</Ad>
      <Soyad>{surName}</Soyad>
      <DogumYili>{birthyear}</DogumYili>
    </TCKimlikNoDogrula>
  </soap:Body>
</soap:Envelope>";
            string textResult = await PostSOAPRequestAsync(xmlSOAP);

            if (textResult == "Unexpected Error") throw new Exception("Failed to connect soap service");

            int startIndex = textResult.IndexOf("<TCKimlikNoDogrulaResult>") + "<TCKimlikNoDogrulaResult>".Length;
            int finishIndex = textResult.IndexOf("</TCKimlikNoDogrulaResult>");
            bool result = bool.Parse(textResult.Substring(startIndex, finishIndex - startIndex));
            return result;

        }
        private static async Task<string> PostSOAPRequestAsync(string text)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpContent content = new StringContent(text, Encoding.UTF8, "text/xml"))
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL))
                {
                    request.Headers.Add("SOAPAction", ACTION);
                    request.Content = content;
                    using (HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        HttpResponseMessage message = response.EnsureSuccessStatusCode(); // throws an Exception if 404, 500, etc.
                        if (message.IsSuccessStatusCode)
                            return await response.Content.ReadAsStringAsync();

                        return "Unexpected Error";
                    }
                }
            }
        }

        /// <summary>
        /// BirthYear must be 4 digit.
        /// </summary>
        private static bool IsBirthYearDigitEqualFour(string birthYear)
        {
            return birthYear.Length == 4;
        }

        /// <summary>
        /// length must be 11 and first number can not be 0.
        /// </summary>
        private static bool IsLengthEqualElevenAndFirstDigitNotEqualZero(string identityNumber)
        {
            return identityNumber.Length == 11 && identityNumber[0] != '0';
        }

        /// <summary>
        /// Tenth digit must be (single digits sum * 7 - couple digits sum) % 10
        /// </summary>
        private static bool IsTenthDigitEqualCalculatedNumber(string identityNumber)
        {
            int singleDigitsSum = 0, coupleDigitsSum = 0;

            for (int i = 0; i <= 8; i += 2)
                singleDigitsSum += int.Parse(identityNumber[i].ToString());

            for (int i = 0; i <= 7; i += 2)
                coupleDigitsSum += int.Parse(identityNumber[i].ToString());

            int calculatedTenthDigit = (singleDigitsSum * 7 - coupleDigitsSum) % 10;

            if (calculatedTenthDigit != int.Parse(identityNumber[9].ToString()))
                return false;

            return true;
        }

        /// <summary>
        /// Eleventh digit must be first ten digits sum % 10.
        /// </summary>
        private static bool IsEleventhDigitEqualCalculatedNumber(string identityNumber)
        {
            int firstTenDigitsSum = 0;
            for (int i = 0; i < identityNumber.Length - 1; i++)
                firstTenDigitsSum += int.Parse(identityNumber[i].ToString());

            if (firstTenDigitsSum % 10 != int.Parse(identityNumber[10].ToString()))
                return false;

            return true;
        }
    }
}
