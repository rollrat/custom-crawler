using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MasterDevs.ChromeDevTools.Protocol.Chrome.DOMSnapshot
{
	/// <summary>
	/// Returns a document snapshot, including the full DOM tree of the root node (including iframes,
	/// template contents, and imported documents) in a flattened array, as well as layout and
	/// white-listed computed style information for the nodes. Shadow DOM in the returned DOM tree is
	/// flattened.
	/// </summary>
	[Command(ProtocolName.DOMSnapshot.CaptureSnapshot)]
	[SupportedBy("Chrome")]
	public class CaptureSnapshotCommand: ICommand<CaptureSnapshotCommandResponse>
	{
		/// <summary>
	/// Gets or sets Whitelist of computed styles to return.
		/// </summary>
		public string[] ComputedStyles { get; set; }
		/// <summary>
	/// Gets or sets Whether to include layout object paint orders into the snapshot.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? IncludePaintOrder { get; set; }
		/// <summary>
	/// Gets or sets Whether to include DOM rectangles (offsetRects, clientRects, scrollRects) into the snapshot
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public bool? IncludeDOMRects { get; set; }
	}
}
