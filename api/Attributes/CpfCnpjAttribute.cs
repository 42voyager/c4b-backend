using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Attributes {

	/// <summary>Classe <c>CustomCPFAttribute</c> herda <c>ValidationAttribute</c>
	/// verifica se um CNPJ ou CPF digitado são válidos. (Não verifica se estão
	/// cadastrados na Receita Federal).</summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class CustomCPFAttribute : ValidationAttribute
	{
		/// <summary>
		/// Método <c>IsValid</c> sobreescreve o <c>ValidationAttribute</c>
		/// da classe com booleano <c>true</c> se os dados passados são válidos
		/// e <c>false</c> se não são válidos.
		/// </summary>
		/// <param name="cpfCnpj">String do CPF ou CNPJ a ser validado.</param>
		/// <returns>Retorna se é válido ou não.</returns>
		public override bool IsValid(object cpfCnpj)
		{
			var validation = (IsCpf((String)cpfCnpj) || IsCnpj((String)cpfCnpj));
			return (validation);
		}
		/// <summary>
		/// Verifica se o CPF passado é válido
		/// </summary>
		/// <param name="cpf">CPF a ser validado</param>
		/// <returns>Retorna <c>true</c> ou <c>false</c> dependendo da
		/// validação.</returns>
		private bool IsCpf(string cpf)
		{
			int[] multiplier1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplier2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

			cpf = cpf.Trim().Replace(".", "").Replace("-", "");
			if (cpf.Length != 11)
				return false;

			for (int j = 0; j < 10; j++)
				if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
					return false;

			string tempCpf = cpf.Substring(0, 9);
			int sum = 0;

			for (int i = 0; i < 9; i++)
				sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

			int reminder = sum % 11;
			if (reminder < 2)
				reminder = 0;
			else
				reminder = 11 - reminder;

			string digit = reminder.ToString();
			tempCpf = tempCpf + digit;
			sum = 0;
			for (int i = 0; i < 10; i++)
				sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

			reminder = sum % 11;
			if (reminder < 2)
				reminder = 0;
			else
				reminder = 11 - reminder;

			digit = digit + reminder.ToString();

			return cpf.EndsWith(digit);
		}
	/// <summary>
	/// Verifica se o CNPJ passado é válido
	/// </summary>
	/// <param name="cnpj">CNPJ a ser validado</param>
	/// <returns>Retorna <c>true</c> ou <c>false</c> dependendo da
		/// validação.</returns>
		private bool IsCnpj(string cnpj)
		{
			int[] multiplier1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplier2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

			cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
			if (cnpj.Length != 14)
				return false;

			string tempCnpj = cnpj.Substring(0, 12);
			int sum = 0;

			for (int i = 0; i < 12; i++)
				sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];

			int reminder = (sum % 11);
			if (reminder < 2)
				reminder = 0;
			else
				reminder = 11 - reminder;

			string digit = reminder.ToString();
			tempCnpj = tempCnpj + digit;
			sum = 0;
			for (int i = 0; i < 13; i++)
				sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];

			reminder = (sum % 11);
			if (reminder < 2)
				reminder = 0;
			else
				reminder = 11 - reminder;

			digit = digit + reminder.ToString();

			return cnpj.EndsWith(digit);
		}
	}
}