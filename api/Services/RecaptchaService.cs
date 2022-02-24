using backend.Interfaces;
using backend.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace backend.Services
{
	/// <summary>
	/// Class <c>RecaptchaService</c> implementa <c>IRecaptchaService</c> interface.
	/// </summary>
	public class RecaptchaService : IRecaptchaService
	{
		private readonly IConfiguration _configuration;
		private readonly string _secret;
		private readonly double _minScore;
		private readonly HttpClient _client;
		private readonly string _apiUrl;

		/// <summary>
		/// Este construtor inicializa o http <paramref name="client"/> e as todas
		/// as <paramref name="configuration"/> do arquivo appSettings.
		/// </summary>
		/// <param name="client">Instacia do HttpClient.</param>
		/// <param name="configuration">Instância do IConfiguration.</param>
		public RecaptchaService(HttpClient client, IConfiguration configuration)
		{
			_configuration = configuration;
			_secret = _configuration.GetSection("Recaptcha:Secret").Value;
			_minScore = double.Parse(_configuration.GetSection("Recaptcha:MinScore").Value);
			_client = client;
			_apiUrl = _configuration.GetSection("Recaptcha:ApiUrl").Value;
		}

		/// <inheritdoc />
		public async Task<bool> ValidateRecaptchaScore(string token)
		{
			if (token == null) return true;

			var values = new Dictionary<string, string>
				{
					{ "secret", _secret },
					{ "response", token }
				};

			var content = new FormUrlEncodedContent(values);
			var response = await _client.PostAsync(_apiUrl, content);
			var responseString = await response.Content.ReadAsStringAsync();
			RecaptchaResponse info = JsonConvert.DeserializeObject<RecaptchaResponse>(responseString);

			if (info.score >= _minScore)
				return (true);
			else
				return (false);
		}
	}
}