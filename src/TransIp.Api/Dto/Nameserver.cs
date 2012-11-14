using System.Runtime.Serialization;

namespace TransIp.Api.Dto
{
	/// <summary>
	/// Models A Nameserver.
	/// </summary>
	[DataContract]
	public class Nameserver
	{
		/// <summary>
		/// The hostname of this nameserver
		/// </summary>
		[DataMember(Name = "hostname")]
		public string HostName { get; set; }

		/// <summary>
		/// Optional ipv4 glue record for this nameserver, leave
		/// empty when no ipv4 glue record is needed for this nameserver.
		/// </summary>
		[DataMember(Name = "ipv4")]
		public string Ipv4 { get; set; }

		/// <summary>
		/// Optional ipv6 glue record for this nameserver, leave
		/// empty when no ipv6 glue record is needed for this nameserver.
		/// </summary>
		[DataMember(Name = "ipv6")]
		public string Ipv6 { get; set; }
	}
}
