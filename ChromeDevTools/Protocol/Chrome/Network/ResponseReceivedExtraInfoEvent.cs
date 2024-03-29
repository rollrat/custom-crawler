using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MasterDevs.ChromeDevTools.Protocol.Chrome.Network
{
	/// <summary>
	/// Fired when additional information about a responseReceived event is available from the network
	/// stack. Not every responseReceived event will have an additional responseReceivedExtraInfo for
	/// it, and responseReceivedExtraInfo may be fired before or after responseReceived.
	/// </summary>
	[Event(ProtocolName.Network.ResponseReceivedExtraInfo)]
	[SupportedBy("Chrome")]
	public class ResponseReceivedExtraInfoEvent
	{
		/// <summary>
	/// Gets or sets Request identifier. Used to match this information to another responseReceived event.
		/// </summary>
		public string RequestId { get; set; }
		/// <summary>
	/// Gets or sets A list of cookies which were not stored from the response along with the corresponding
	/// reasons for blocking. The cookies here may not be valid due to syntax errors, which
	/// are represented by the invalid cookie line string instead of a proper cookie.
		/// </summary>
		public BlockedSetCookieWithReason[] BlockedCookies { get; set; }
		/// <summary>
	/// Gets or sets Raw response headers as they were received over the wire.
		/// </summary>
		public Dictionary<string, string> Headers { get; set; }
		/// <summary>
	/// Gets or sets Raw response header text as it was received over the wire. The raw text may not always be
	/// available, such as in the case of HTTP/2 or QUIC.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string HeadersText { get; set; }
	}
}
