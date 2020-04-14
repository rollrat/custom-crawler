using MasterDevs.ChromeDevTools;
using MasterDevs.ChromeDevTools.Protocol.Chrome.CSS;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;


namespace MasterDevs.ChromeDevTools.Protocol.Chrome.WebAudio{
	/// <summary>
	/// Enum of AudioNode::ChannelCountMode from the spec

	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ChannelCountMode
	{
			[EnumMember(Value = "clamped-max")]
			Clamped_max,
			Explicit,
			Max,
	}
}
