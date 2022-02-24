using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace backend.Services
{
    /// <summary>
    /// Class <c>HashService</c> gera o hash encriptogrado.
    /// Tambem pode desincriptar o hash que foi criado.
    /// </summary>
	public class HashService
	{
        /// <summary>
        /// Este método <c>GenerateKey</c> gera uma Key e converte para base 64 de String.
        /// </summary>
        /// <returns>A key em base 64.</returns>
		public static string GenerateKey()
		{
			byte [] key;

            using (Aes aes = Aes.Create())
			{
				aes.GenerateKey();
				key = aes.Key;
			}
			return Convert.ToBase64String(key);
		}

        /// <summary>
        /// Este método <c>EncryptString</c> recebe a <paramref name="key"/> e <paramref name="plainText"/>
        /// e encriptografa isso criando um hash.
        /// </summary>
        /// <param name="key">Chave de criptografia</param>
        /// <param name="plainText">Texto que deseja encriptar</param>
        /// <returns>O texto encriptografado em forma de hash. Em base 64</returns>
		public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }
            return (Convert.ToBase64String(array)).Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// Este método <c>DecryptString</c> utilizada para desincriptar os hash.
        /// Em casos de receber uma string que não seja base 64 haveria um erro.
        /// Para evitar temos um try catch.
        /// </summary>
        /// <param name="key"> Chave para desincriptar o hash. </param>
        /// <param name="cipherText"> Hash que deseja desencriptar. </param>
        /// <returns> Em caso de sucesso o param cipherText desencriptado. </returns>
        /// <returns> Em caso de falha retorna null. </returns>
        public static string DecryptString(string key, string cipherText)
        {
            // if (IsBase64String(cipherText) is false)
            //     return null;
            byte[] iv = new byte[16];
            byte[] buffer;
            try
            {
                buffer = Convert.FromBase64String(cipherText.Replace('-', '+').Replace('_', '/'));
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ocorreu um erro ao tentar desincriptar o hash {cipherText}:\n\n{exception}\n");
                return null;
            }
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <sumary>
        /// Este método <c>GetIdFromHash</c> desencripta o hash, separa o id do cnpj
        /// e por fim retorna o id do hash.
        /// </sumary>
        /// <param name="key"> Chave para desencriptar o hash </param>
        /// <param name="hash"> O hash que vc do qual você precisa pegar o id </param>
        /// <returns> Com sucesso retorna o id escondido no hash. </return>
        /// <returns> Em caso de falha retorna -1 </return>
        public static int GetIdFromHash(string key, string hash){
            if (hash == null)
                return -1;
            string decriptedHash = HashService.DecryptString(key, hash);
            if (decriptedHash == null)
                return -1;
			var id = int.Parse(decriptedHash.Substring(0, decriptedHash.Length - 18));
            return id;
        }

	}
}