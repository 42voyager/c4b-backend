using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
	public class Contract
	{
		[Key]
		[ForeignKey("Customer")]
		public int CustomerID { get; set; }
		public string ContractPdf { get; set; }
        public System.DateTime SignDate { get; set; }
		[Required]
		[Range(typeof(bool), "true", "true", 
			ErrorMessage = "Precisa aceitar os termos e condições")]
		public bool AcceptTerms { get; set; }
		[Required]
		[Range(typeof(bool), "true", "true", 
			ErrorMessage = "Precisa autorizar a consulta de SCR")]
		public bool AuthorizeSCR { get; set; }
		[Required]
		[Range(typeof(bool), "true", "true", 
			ErrorMessage = "Precisa responder a pergunta sobre a PEP")]
		public bool ExistsPEP { get; set; }
	}
}