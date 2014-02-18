using System.Runtime.Serialization;

namespace TransIp.Api.Dto
{
	/// <summary>
	/// Models branding for a Domain.
	/// </summary>
	[DataContract]
	public class DomainBranding
	{
		/// <summary>
		/// The company name displayed in transfer-branded e-mails
		/// </summary>
		[DataMember(Name = "companyName")]
		public string CompanyName { get; set; }

		/// <summary>
		/// The support email used for transfer-branded e-mails
		/// </summary>
		[DataMember(Name = "supportEmail")]
		public string SupportEmail { get; set; }

		/// <summary>
		/// The company url displayed in transfer-branded e-mails
		/// </summary>
		[DataMember(Name = "companyUrl")]
		public string CompanyUrl { get; set; }

		/// <summary>
		/// The terms of usage url as displayed in transfer-branded e-mails
		/// </summary>
		[DataMember(Name = "termsOfUsageUrl")]
		public string TermsOfUsageUrl { get; set; }

		/// <summary>
		/// The first generic bannerLine displayed in whois-branded whois output.
		/// </summary>
		[DataMember(Name = "bannerLine1")]
		public string BannerLine1 { get; set; }

		/// <summary>
		/// The second generic bannerLine displayed in whois-branded whois output.
		/// </summary>
		[DataMember(Name = "bannerLine2")]
		public string BannerLine2 { get; set; }

		/// <summary>
		/// The third generic bannerLine displayed in whois-branded whois output.
		/// </summary>
		[DataMember(Name = "bannerLine3")]
		public string BannerLine3 { get; set; }
	}
}