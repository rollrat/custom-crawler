using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MasterDevs.ChromeDevTools.Protocol.Chrome.Network
{
	/// <summary>
	/// Fired when additional information about a requestWillBeSent event is available from the
	/// network stack. Not every requestWillBeSent event will have an additional
	/// requestWillBeSentExtraInfo fired for it, and there is no guarantee whether requestWillBeSent
	/// or requestWillBeSentExtraInfo will be fired first for the same request.
	/// </summary>
	[Event(ProtocolName.Network.RequestWillBeSentExtraInfo)]
	[SupportedBy("Chrome")]
	public class RequestWillBeSentExtraInfoEvent
	{
		/// <summary>
	/// Gets or sets Request identifier. Used to match this information to an existing requestWillBeSent event.
		/// </summary>
		public string RequestId { get; set; }
		/// <summary>
	/// Gets or sets A list of cookies which will not be sent with this request along with corresponding reasons
	/// for blocking.
		/// </summary>
		public BlockedCookieWithReason[] BlockedCookies { get; set; }
		/// <summary>
	/// Gets or sets Raw request headers as they will be sent over the wire.
		/// </summary>
		public Dictionary<string, string> Headers { get; set; }
	}
}
