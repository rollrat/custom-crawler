using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;


namespace MasterDevs.ChromeDevTools.Protocol.Chrome.Accessibility{
	/// <summary>
	/// Enum of possible native property sources (as a subtype of a particular AXValueSourceType).

	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum AXValueNativeSourceType
	{
			Figcaption,
			Label,
			Labelfor,
			Labelwrapped,
			Legend,
			Tablecaption,
			Title,
			Other,
	}
}
