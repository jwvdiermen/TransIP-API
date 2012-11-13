using TransIp.Api.Dto;

namespace TransIp.Api
{
	/// <summary>
	/// The service for domain related tasks.
	/// </summary>
	public class DomainService : ClientBase
	{
		/// <summary>
		/// Create a new client for communicating with the TransIP domain service.
		/// </summary>
		/// <param name="login">The login name from TransIP.</param>
		/// <param name="mode">The mode.</param>
		/// <param name="privateKey">The private key.</param>
		public DomainService(string login, ClientMode mode, string privateKey)
			: base("DomainService", "https://api.transip.nl/wsdl/?service=DomainService", login, mode, privateKey)
		{
			
		}

		/// <summary>
		/// Gets the whois of a domain name.
		/// </summary>
		/// <param name="domain">The domain name to get the whois for.</param>
		/// <returns>The whois data for the domain.</returns>
		public string GetWhois(string domain)
		{
			return Send<string>(domain);
		}
	}
}
