using Supabase.Gotrue.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace ShareBill.Shared.Errors.AuthErrors

{
    public static class GoTrueExceptionExtensions
    {
        public static AuthAppErrorInfo ExtractErrorCode(this Supabase.Gotrue.Exceptions.GotrueException goTrueEx)
        {
            try
            {
                string? error = JsonSerializer.Deserialize<ErrorJsonMessage>(goTrueEx.Message ?? string.Empty)?.Msg?.ToTitleCase();
                return AuthErrorResolver.Resolve(error ?? string.Empty);
            }
            catch (Exception ex)
            {

                return AuthErrorResolver.FromException(new Exception("Failed to extract GoTrue error code.", ex));
            }
        }


        private static string ToTitleCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            var words = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpperInvariant(words[i][0]) + words[i][1..].ToLowerInvariant();
                }
            }
            return string.Join("", words);
        }
        private class ErrorJsonMessage
        {
            [JsonPropertyName("code")]
            public int Code { get; set; }
            [JsonPropertyName("error_code")]
            public string? ErrorCode { get; set; }
            [JsonPropertyName("msg")]
            public string? Msg { get; set; }
        }
    }
}
