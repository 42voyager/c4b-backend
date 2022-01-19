using System.ComponentModel.DataAnnotations;
using backend.Attributes;

namespace backend.Models
{
	public class Customer
	{
		public int Id { get; set; }

		[Required]
		public string Limit { get; set; }

		[Required]
		public string Installment { get; set; }

		public string Reason { get; set; }

		[Required(ErrorMessage = "O nome é obrigatório.")]
		[StringLength(150, ErrorMessage = "O nome deve ser menor ou igual a 150 caracteres!")]
		[MinLength(2, ErrorMessage = "O nome deve ser maior ou igual a 2 caracteres!")]
		public string Name { get; set; }

		[Required(ErrorMessage = "O email é obrigatório.")]
		[EmailAddress(ErrorMessage = "O email é inválido.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "O celular é obrigatório.")]
		[RegularExpression("^\\([1-9]{2}\\) (?:[2-8]|9[1-9])[0-9]{3}\\-[0-9]{4}$",
		ErrorMessage = "O celular deve seguir o formato: (XX) XXXXX-XXXX")]
		public string Cellphone { get; set; }

		[Required(ErrorMessage = "O cnpj é obrigatório.")]
		[RegularExpression("^\\d{2}\\.\\d{3}\\.\\d{3}\\/\\d{4}\\-\\d{2}$",
		ErrorMessage = "O CNPJ deve seguir o formato: XX.XXX.XXX/XXXX-XX")]
		[CustomCPF(ErrorMessage = "O CNPJ não é válido.")]
		public string Cnpj { get; set; }

		[Required(ErrorMessage = "O nome da empresa obrigatório.")]
		[StringLength(100, ErrorMessage = "O nome da empresa deve ser menor ou igual a 100 caracteres!")]
		[MinLength(2, ErrorMessage = "O nome da empresa deve ser maior ou igual a 2 caracteres!")]
		public string Company { get; set; }

		[Required]
		[Range(typeof(bool), "true", "true", ErrorMessage = "Precisa aceitar os termos e condições")]
		public bool Optin { get; set; }
		public string IPAddress { get; set; }
		public string OperatingSystem { get; set; }
		public System.DateTime Timestamp { get; set; }
	}
}
