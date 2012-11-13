using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransIp.Api.Examples.Shared;

namespace TransIp.Api.Examples.DomainService
{
	class Program
	{
		static void Main(string[] args)
		{
			var domainService = new Api.DomainService(ApiSettings.Login, ApiSettings.Mode, ApiSettings.PrivateKey);
			var whois = domainService.GetWhois("crealuz.nl");
		}
	}
}
