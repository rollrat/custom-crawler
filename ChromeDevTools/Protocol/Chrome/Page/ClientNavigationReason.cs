using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;


namespace MasterDevs.ChromeDevTools.Protocol.Chrome.Page{
	/// <summary>

	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ClientNavigationReason
	{
			FormSubmissionGet,
			FormSubmissionPost,
			HttpHeaderRefresh,
			ScriptInitiated,
			MetaTagRefresh,
			PageBlockInterstitial,
			Reload,
			AnchorClick,
	}
}
