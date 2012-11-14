using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TransIp.Api.Dto
{
	/// <summary>
	/// This class holds the data for one result item of a multi availability check.
	/// </summary>
	[DataContract(Namespace = "http://www.transip.nl/soap")]
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
	/// Array of <see cref="DomainCheckResult"/> objects.
	/// </summary>
	[CollectionDataContract(Namespace = "http://www.transip.nl/soap")]
	public class ArrayOfDomainCheckResult : List<DomainCheckResult>
	{
		public ArrayOfDomainCheckResult() { }
		public ArrayOfDomainCheckResult(IEnumerable<DomainCheckResult> collection) : base(collection) { }
		public ArrayOfDomainCheckResult(params DomainCheckResult[] args) : base(args) { }
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
