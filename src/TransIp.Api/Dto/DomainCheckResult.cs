using System.Runtime.Serialization;

namespace TransIp.Api.Dto
{
	/// <summary>
	/// This class holds the data for one result item of a multi availability check.
	/// </summary>
	[DataContract]
	public class DomainCheckResult
	{
		/// <summary>
		/// The name of the Domain for which we have a status in this object.
		/// </summary>
		[DataMember(Name = "domainName")]
		public string DomainName { get; set; }

		/// <summary>
		/// The status for this domain.
		/// </summary>
		[DataMember(Name = "status")]
		public AvailabilityStatus Status { get; set; }
	}

	/// <summary>
	/// The availability statusses.
	/// </summary>
	public enum AvailabilityStatus
	{
		InYourAccount,
		Unavailable,
		NotFree,
		Free
	}
}
