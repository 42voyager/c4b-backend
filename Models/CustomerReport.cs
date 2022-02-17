using System.ComponentModel.DataAnnotations;
using backend.Attributes;

namespace backend.Models
{
	public class CustomerReport
	{
		public string Limit { get; set; }

		public string Installment { get; set; }

		public string Reason { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public string Cellphone { get; set; }

		public string Cnpj { get; set; }

		public string Company { get; set; }

		public string Status { get; set; }
	}
}