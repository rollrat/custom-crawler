using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MasterDevs.ChromeDevTools.Protocol.Chrome.WebAuthn
{
	[SupportedBy("Chrome")]
	public class VirtualAuthenticatorOptions
	{
		/// <summary>
	/// Gets or sets Protocol
		/// </summary>
		public AuthenticatorProtocol Protocol { get; set; }
		/// <summary>
	/// Gets or sets Transport
		/// </summary>
		public AuthenticatorTransport Transport { get; set; }
		/// <summary>
	/// Gets or sets Defaults to false.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? HasResidentKey { get; set; }
		/// <summary>
	/// Gets or sets Defaults to false.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? HasUserVerification { get; set; }
		/// <summary>
	/// Gets or sets If set to true, tests of user presence will succeed immediately.
	/// Otherwise, they will not be resolved. Defaults to true.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? AutomaticPresenceSimulation { get; set; }
		/// <summary>
	/// Gets or sets Sets whether User Verification succeeds or fails for an authenticator.
	/// Defaults to false.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? IsUserVerified { get; set; }
	}
}
