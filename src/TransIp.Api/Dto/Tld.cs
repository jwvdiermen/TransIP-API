using System.Linq;
using System.Runtime.Serialization;

namespace TransIp.Api.Dto
{
	/// <summary>
	/// This class models a Tld and holds information such as price,
	/// registration length and capabilities.
	/// </summary>
	[DataContract]
	public class Tld
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public Tld()
		{
			Capabilities = new TldCapabilities(this);
		}

		/// <summary>
		/// The name of this TLD, including the starting dot. E.g. .nl or .com.
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Price of the TLD in Euros.
		/// </summary>
		[DataMember(Name = "price")]
		public decimal Price { get; set; }

		/// <summary>
		/// Price for renewing the TLD in Euros.
		/// </summary>
		[DataMember(Name = "renewalPrice")]
		public decimal RenewalPrice { get; set; }

		/// <summary>
		/// A list of the capabilities that this Tld has (the things that can be
		/// done with a domain under this tld).
		/// </summary>
		[DataMember(Name = "capabilities")]
		public string[] CapabilityList { get; set; }

		/// <summary>
		/// The capabilities of the Tld.
		/// </summary>
		public TldCapabilities Capabilities { get; private set; }

		/// <summary>
		/// Length in months of each registration or renewal period.
		/// </summary>
		[DataMember(Name = "registrationPeriodLength")]
		public int RegistrationPeriodLength { get; set; }

		/// <summary>
		/// Number of days a domain needs to be canceled before the renewal date.
		/// E.g., If the renewal date is 10-Dec-2011 and the cancelTimeFrame is 4 days,
		/// the domain has to be canceled before 6-Dec-2011, otherwise it will be
		/// renewed already.
		/// </summary>
		[DataMember(Name = "cancelTimeFrame")]
		public int CancelTimeFrame { get; set; }
	}

	/// <summary>
	/// Utility class for easy querying for TLD capabilities.
	/// </summary>
	public class TldCapabilities
	{
		private readonly Tld _tld;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="tld">The Tld.</param>
		public TldCapabilities(Tld tld)
		{
			_tld = tld;
		}

		/// <summary>
		/// True if a authorization code is required to transfer the Tld.
		/// </summary>
		public bool RequiresAuthCode
		{
			get { return _tld.CapabilityList.Contains("requiresAuthCode"); }
		}

		/// <summary>
		/// True if new domains can be registered for the Tld.
		/// </summary>
		public bool CanRegister
		{
			get { return _tld.CapabilityList.Contains("canRegister"); }
		}

		/// <summary>
		/// True if domains of the Tld can be transfered with an owner change.
		/// </summary>
		public bool CanTransferWithOwnerChange
		{
			get { return _tld.CapabilityList.Contains("canTransferWithOwnerChange"); }
		}

		/// <summary>
		/// True if domains of the Tld can be transfered without an owner change.
		/// </summary>
		public bool CanTransferWithoutOwnerChange
		{
			get { return _tld.CapabilityList.Contains("canTransferWithoutOwnerChange"); }
		}

		/// <summary>
		/// True if the domains of the Tld can be locked.
		/// </summary>
		public bool CanSetLock
		{
			get { return _tld.CapabilityList.Contains("canSetLock"); }
		}

		/// <summary>
		/// True if the owner can be changed.
		/// </summary>
		public bool CanSetOwner
		{
			get { return _tld.CapabilityList.Contains("canSetOwner"); }
		}

		/// <summary>
		/// True if the WHOIS contacts can be changed.
		/// </summary>
		public bool CanSetContacts
		{
			get { return _tld.CapabilityList.Contains("canSetContacts"); }
		}

		/// <summary>
		/// True if the nameservices can be changed.
		/// </summary>
		public bool CanSetNameservers
		{
			get { return _tld.CapabilityList.Contains("canSetNameservers"); }
		}
	}
}
