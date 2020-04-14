using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;


namespace MasterDevs.ChromeDevTools.Protocol.Chrome.Media{
	/// <summary>
	/// Break out events into different types

	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum PlayerEventType
	{
			ErrorEvent,
			TriggeredEvent,
			MessageEvent,
	}
}
