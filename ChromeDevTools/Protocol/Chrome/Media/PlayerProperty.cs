using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MasterDevs.ChromeDevTools.Protocol.Chrome.Media
{
	/// <summary>
	/// Player Property type
	/// </summary>
	[SupportedBy("Chrome")]
	public class PlayerProperty
	{
		/// <summary>
	/// Gets or sets Name
		/// </summary>
		public string Name { get; set; }
		/// <summary>
	/// Gets or sets Value
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string Value { get; set; }
	}
}
