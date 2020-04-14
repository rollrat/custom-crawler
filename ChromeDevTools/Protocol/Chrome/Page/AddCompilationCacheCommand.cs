using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MasterDevs.ChromeDevTools.Protocol.Chrome.Page
{
	/// <summary>
	/// Seeds compilation cache for given url. Compilation cache does not survive
	/// cross-process navigation.
	/// </summary>
	[Command(ProtocolName.Page.AddCompilationCache)]
	[SupportedBy("Chrome")]
	public class AddCompilationCacheCommand: ICommand<AddCompilationCacheCommandResponse>
	{
		/// <summary>
	/// Gets or sets Url
		/// </summary>
		public string Url { get; set; }
		/// <summary>
	/// Gets or sets Base64-encoded data
		/// </summary>
		public string Data { get; set; }
	}
}
