using System.Linq;
using TransIp.Api.Dto;
using TransIp.Api.Examples.Shared;

namespace TransIp.Api.Examples.DomainService
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var domainService = new Api.DomainService(ApiSettings.Login, ApiSettings.Mode, ApiSettings.PrivateKey);

			// Use the code below to experiment with the API by changing values and removing the comments.

			//var whois = domainService.GetWhois("crealuz.nl");
			//whois = domainService.GetWhois("crealuz.org");

			//var dnsEntries = new[]
			//{
			//	new DnsEntry { Name = "@", Expire = 86400, Type = DnsEntryType.A, Content = "127.0.0.1" },
			//	new DnsEntry { Name = "www", Expire = 86400, Type = DnsEntryType.CNAME, Content = "@" }
			//};
			//domainService.SetDnsEntries("my-crm.nl", dnsEntries);

			//var availability = domainService.BatchCheckAvailability(new[]
			//{
			//	"crealuz.nl",
			//	"crealuz.com",
			//	"crealuz.org",
			//	"sdfkhgdfhgsdkfhf.nl",
			//	"dflkjhsdguhsfsdifoj.com"
			//});

			//var tldIfnos = domainService.GetAllTldInfos();

			//var domainNames = domainService.GetDomainNames();
			//var tldInfo = domainService.GetTldInfo("org");
			//var info = domainService.GetInfo("crealuz.org");
			//var singleAvailability = domainService.CheckAvailability("crealuz.nl");

			//var info = domainService.GetInfo("j-cl.eu");
			//var entries = info.DnsEntries.ToList();
			//entries.Add(new DnsEntry
			//{
			//	Name = "local",
			//	Type = DnsEntryType.A,
			//	Expire = 3600, // 1 hour
			//	Content = "127.0.0.1"
			//});
			//domainService.SetDnsEntries("j-cl.eu", entries.ToArray());
		}
	}
}