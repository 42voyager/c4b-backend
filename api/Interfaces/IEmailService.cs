using backend.Models;
using System.Collections.Generic;
using MimeKit;
using System.Threading.Tasks;

namespace backend.Interfaces
{

	/// <summary>
	/// Interface <c>IEmailService</c> tem os serviços que auxiliam os controllers
	/// nas requisições.
	/// Prepara o arquivo json e envia o email com os dados recebidos.
	/// </summary>
	/// <typeparam name="T">Tipo dos dados a serem adicionados no json.</typeparam>
	public interface IEmailService<T>
	{

		/// <summary>
		/// Este método <c>SendEmailAsync</c> envia um email recebido pelo param 
		/// <paramref name="email"/>.
		/// O envio é feito através do SmtpClient utilizando a library <c>MailKit</c>.
		/// Este método é assíncrono.
		/// </summary>
		/// <param name="email">Intância da classe <c>Email</c>
		/// a contendo todos os dados a serem enviado.</param>
		/// <returns>Uma task.</returns>
		Task SendEmailAsync(Email email);

		/// <summary>
		/// Este método <c>PrepareJsonAsync</c> cria um arquivo json com o <paramref name="file"/>.
		/// O arquivo será populado com o conteúdo da instância do <paramref name="entity"/>.
		/// Este método é assíncrono.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="file"></param>
		/// <returns></returns>
		Task PrepareJsonAsync(T newUser, string file);

	}
}