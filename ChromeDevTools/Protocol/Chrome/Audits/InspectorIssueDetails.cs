using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MasterDevs.ChromeDevTools.Protocol.Chrome.Audits
{
	/// <summary>
	/// This struct holds a list of optional fields with additional information
	/// specific to the kind of issue. When adding a new issue code, please also
	/// add a new optional field to this type.
	/// </summary>
	[SupportedBy("Chrome")]
	public class InspectorIssueDetails
	{
		/// <summary>
	/// Gets or sets SameSiteCookieIssueDetails
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public SameSiteCookieIssueDetails SameSiteCookieIssueDetails { get; set; }
	}
}
