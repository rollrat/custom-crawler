using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MasterDevs.ChromeDevTools.Protocol.Chrome.Media
{
	[SupportedBy("Chrome")]
	public class PlayerEvent
	{
		/// <summary>
	/// Gets or sets Type
		/// </summary>
		public PlayerEventType Type { get; set; }
		/// <summary>
	/// Gets or sets Events are timestamped relative to the start of the player creation
	/// not relative to the start of playback.
		/// </summary>
		public double Timestamp { get; set; }
		/// <summary>
	/// Gets or sets Name
		/// </summary>
		public string Name { get; set; }
		/// <summary>
	/// Gets or sets Value
		/// </summary>
		public string Value { get; set; }
	}
}
