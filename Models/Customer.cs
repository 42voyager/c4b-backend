using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
	public class Customer
	{
		public int Id { get; set; }

		[Required]
		[StringLength(150, ErrorMessage = "O nome deve ser menor ou igual a 150 caracteres!")]
		[MinLength(2, ErrorMessage = "O nome deve ser maior ou igual a 2 caracteres!")]
		public string Nome { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[RegularExpression("^\\([1-9]{2}\\) (?:[2-8]|9[1-9])[0-9]{3}\\-[0-9]{4}$", 
		ErrorMessage = "O celular deve seguir o formato: (XX) XXXXX-XXXX")]
		public string Cellphone { get; set; }

		[Required]
		[RegularExpression("^\\d{2}\\.\\d{3}\\.\\d{3}\\/\\d{4}\\-\\d{2}$",
		ErrorMessage = "O Cnpj deve seguir o formato: XX.XXX.XXX/XXXX-XX")]
		public string Cnpj { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "O nome da Company deve ser menor ou igual a 100 caracteres!")]
		[MinLength(2, ErrorMessage = "O nome da Company deve ser maior ou igual a 2 caracteres!")]
		public string Company { get; set; }

		[Required]
		public bool Optin { get; set; }
	}
}
