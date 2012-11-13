using System.Runtime.Serialization;

namespace TransIp.Api.Dto
{
	/// <summary>
	/// This class models a DomainAction, which holds information about the action being run.
	/// </summary>
	[DataContract]
	public class DomainAction
	{
		/// <summary>
		/// The name of this DomainAction.
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// If this action has failed, this field will be true.
		/// </summary>
		[DataMember(Name = "hasFailed")]
		public bool HasFailed { get; set; }

		/// <summary>
		/// If this action has failed, this field will contain an descriptive message.
		/// </summary>
		[DataMember(Name = "message")]
		public string Message { get; set; }
	}
}
