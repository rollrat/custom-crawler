using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MasterDevs.ChromeDevTools.Protocol.Chrome.Audits
{
	/// <summary>
	/// An inspector issue reported from the back-end.
	/// </summary>
	[SupportedBy("Chrome")]
	public class InspectorIssue
	{
		/// <summary>
	/// Gets or sets Code
		/// </summary>
		public InspectorIssueCode Code { get; set; }
		/// <summary>
	/// Gets or sets Details
		/// </summary>
		public InspectorIssueDetails Details { get; set; }
	}
}
