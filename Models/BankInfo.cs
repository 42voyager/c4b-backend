using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
	public class BankInfo
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "O nome do banco é obrigatório.")]
		public string BankName { get; set; }

		[Required(ErrorMessage = "A agência do banco é obrigatória.")]
		public string Branch { get; set; }

		[Required(ErrorMessage = "A conta é obrigatória.")]
		public string CheckingAccount { get; set; }

		public string hash { get; set; }
	}
}