using System;
using System.Text;

namespace PaymentIntegration.Domain
{
    public static class Helper
    {
        public static string ReferenceNumber()
        {
            string reference = string.Empty;
            string[] AllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sTempChars = String.Empty;
            int requestIdDigits = 14;

            Random rand = new Random();

            for (int i = 0; i < requestIdDigits; i++)
            {

                sTempChars = AllowedCharacters[rand.Next(0, AllowedCharacters.Length)];

                reference += sTempChars;

            }

            return reference;
        }

        public static string GetAuthKey(string apiKey, string clientSecret)
        {
            string keyPrep = $"{apiKey}:{clientSecret}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(keyPrep.ToCharArray()));
        }
    }
}
