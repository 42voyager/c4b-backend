using backend.Interfaces;
using backend.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace backend.Services
{
	public class RecaptchaService : IRecaptchaService
	{
		private readonly string _secret;
		private readonly double _minScore;
		private static readonly HttpClient _client = new HttpClient();
		private readonly string _apiUrl;
		public RecaptchaService()
		{
			_secret = "6LfrEXUdAAAAAMkL4sDemtSjhQu1uN66UGf3P_W3";
			_apiUrl = "https://www.google.com/recaptcha/api/siteverify";
			_minScore = 0.5;
		}
		public async Task<bool> ValidateRecaptchaScore(string token)
		{
			// For now, if the token fails we just return true 
			if (token == null) return true;

			var values = new Dictionary<string, string>
				{
					{ "secret", _secret },
					{ "response", token }
				};

			var content = new FormUrlEncodedContent(values);
			var response = await _client.PostAsync(_apiUrl, content);
			var responseString = await response.Content.ReadAsStringAsync();
			Recaptcha info = JsonConvert.DeserializeObject<Recaptcha>(responseString);

			if (info.score >= _minScore)
				return (true);
			else
				return (false);
		}
	}
}