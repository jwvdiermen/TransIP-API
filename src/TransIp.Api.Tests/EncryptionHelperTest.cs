using NUnit.Framework;
using System.Collections.Generic;
using TransIp.Api.Dto;

namespace TransIp.Api.Tests
{
	[TestFixture]
	public class EncryptionHelperTest
	{
		#region Data

		private static readonly string PrivateKey =
			@"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEAzmF5dRwIDondpKnOZMIC2ew4nLlaMLvCZ3283mysD9z5Brea
YIoRvQJt725AuA8CffDX5UrUeOuaCpmXH7+QL3/tcucAVBwJ5RzKrUpmSSPIXUJQ
qXhz1MFIS6QHPtWpRnL5V2Wo9d6vgWU7PFKGqQfAJUzmtDfVsUbUM7BrT7EvWGUX
+rnJD3LixK9dWcPVy3O1nmazKJNjGALyaaTJDEwCIRMp6VyMfvG+6/XXhlYR3S03
eq8pGXVqSqs7JbZPMVyctXf111Y+ejMqiNlZmCwfpYwlJbPu+KZsLttlEfqWDpQE
4dSDcsQlECTomu6jFS9zRrTmL2cmf8zkkVSPjQIDAQABAoIBAQCKd8Xo+ATD1GY8
a53J5o3JLv+Qz5+eoOs/SpKk3V7YSddfVWKjsR9TpESkZ2HO3Gs8mpIQCpPiCZlR
0Vke+QVBiWCEPk7vH9zXtuiZOhjEq9hsEelSuzlkHsZl0cj2tJ4dYVi/9bpWNLGm
bIhA4dHtqQCXRjBn7CpZBf+sKorlbPSmaIKaMFjUDRAoZWCnUZ1/L7PQCmTCk/Xu
XIecsdiy5WYRQnoAyxulrtC2MVnmXeIIpDqcMwZwF/tzhSHT/H3tCEi99zgaE3dj
+2YSXfcKaESqMb+DE7yO09LnhKez1cEog3JV/PzQIEjP7JvhIICZL4bGpkP+AW51
hHe08mcBAoGBAOr10kUQ+t4pDmB0Vnus0X9pgHfF2sXIZZDKP1P8EbtmJ8RtFBOe
/hvcwC1+EAjy+gJE5qsFz/2/2yMZIFEov5Sbmms1Qpslo6E+0w9OOWeW8xyb9dCV
7dp/gNZCxj0rVsdKOR1wgV4TicZUT3fyu8TlLscCzG4gVD6oYDYW41LNAoGBAODc
gM3+xnN/633IGl7H6CZkkyG3I343T17uyW9p9kyKOXNrRrg79XFwL4Q1GoBhuOt3
VUPn5kY5SDyiocfnxQ/MgHVQcd4xwWq3rT9wADdC56kOMiQSL37eCkmp1KQKfrov
QuUbAu/rW4zTT76Oo0fzvncTA7fWZ9odDOztva/BAoGAKegoVcs+g2tdNhTp6+sZ
/pipojM23vnsK5P3EZqu6vbAdwdhglJkTkHkQPjwETiNIOR7I9vIiiCzDCKKIg+b
g/zw4NhCBfwDoFndOSihknlY6Sxj/o0PPF5rc0u7oeNd+fOiFj8fw9DGTQpylhlE
Jk0eN76nCalYfUh4yIzyhK0CgYB/eIEMRgH6N+onw+gvEuRn31wJIOjeBDzadEN9
BXS6ryEibQ4KIvNg+1f0eqYrYTqTQXL0q+G+rXpl5UwRJzJvYl7wIkpqy4n6FWYB
MFzu9t6c149VI3oJUZZDbCM/WzO8GE6z0jw4BhRAIQpz3Ch0AZlXp0/UR5dX7mAF
cEC4AQKBgQCCvztBn38T1CiC4X5xS8HZe1wqPXhaBX53/wnr2APM764VSGXCsJO/
P6e3Hd2mi4E/OCEkq3Wn76B0ruyndSHgu2NITfTUdaZiaRhmom5tcaQhrps+lomP
6YNatcJ1NRUjBEAbx6GUTdd+7kpAmfSjCp0FpgjiXW+73AHZs3M1eA==
-----END RSA PRIVATE KEY-----";

		#endregion

		[Test]
		public void CanSign()
		{
			var domain = new Domain
			{
				Name = "example.com",
				DnsEntries = new[]
				{
					new DnsEntry {Name = "@", Expire = 86400, Type = DnsEntryType.A, Content = "80.69.67.46"},
					new DnsEntry {Name = "@", Expire = 86400, Type = DnsEntryType.MX, Content = "10 @"},
					new DnsEntry {Name = "@", Expire = 86400, Type = DnsEntryType.MX, Content = "20 relay.transip.nl."},
					new DnsEntry {Name = "ftp", Expire = 86400, Type = DnsEntryType.CNAME, Content = "@"},
					new DnsEntry {Name = "mail", Expire = 86400, Type = DnsEntryType.CNAME, Content = "@"},
					new DnsEntry {Name = "www", Expire = 86400, Type = DnsEntryType.CNAME, Content = "@"}
				}
			};

			const string timestamp = "1352842387";
			const string nonce = "50a2bc93d5e3e6.28404859";

			var signature = EncryptionHelper.Encode(EncryptionHelper.Sign(PrivateKey, new object[]
			{
				domain,
				new KeyValuePair<string, string>("__method", "register"),
				new KeyValuePair<string, string>("__service", "DomainService"),
				new KeyValuePair<string, string>("__hostname", "api.transip.nl"),
				new KeyValuePair<string, string>("__timestamp", timestamp),
				new KeyValuePair<string, string>("__nonce", nonce)
			}));

			// Expected signature taken from PHP example and replicated here in .NET
			const string expected =
				"wghhEAhMJNt4a4Rxun3oTODB4sJSvfJNYDkNqxO3PkWCkdpRrSh9MgiVCkUeAbl0zBrWf5SIXAsQSwBSrT0hoj3MyVs7XFNnod%2Finen3cLh65JCdVTS%2BRqNDqOlPzeI0AQ8tnuUXjgR%2Fr%2BxFaUJxrdirVsDt%2B4KaIurmztsY4U8%2BBLMBCS9HDoYKMJUFIGlWWHcYpNVIyg%2F8FzfXQRqDPfqOkzg%2FuXQA0%2BVF49zQewxdEYI6qLKPl8T%2BoWv%2FjgvlJZydmp378woawbngE5tQ%2FEQbOfgAHBM9i%2BwhbRFH%2FWpEy%2BJZPyhvV2sQxiDjjVFMX1A%2F9ue0rVNnjBKj86f2Rg%3D%3D";
			
			Assert.AreEqual(expected, signature);
		}
	}
}