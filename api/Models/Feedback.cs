using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
	public class Feedback
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "O nome é obrigatório!")]
		[StringLength(150, ErrorMessage = "O nome deve ser menor ou igual a 150 caracteres!")]
		[MinLength(2, ErrorMessage = "O nome deve ser maior ou igual a 2 caracteres!")]
		public string Name { get; set; }

		[Required(ErrorMessage = "O email é obrigatório!")]
		[EmailAddress(ErrorMessage = "O email é inválido!")]
		public string Email { get; set; }

		[Required(ErrorMessage = "A mensagem é obrigatória!")]
		[MinLength(5, ErrorMessage = "A mensagem deve ser maior ou igual a 5 caracteres!")]
		public string Message { get; set; }
	}
}