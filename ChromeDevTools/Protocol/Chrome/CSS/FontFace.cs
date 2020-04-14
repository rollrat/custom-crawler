using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MasterDevs.ChromeDevTools.Protocol.Chrome.CSS
{
	/// <summary>
	/// Properties of a web font: https://www.w3.org/TR/2008/REC-CSS2-20080411/fonts.html#font-descriptions
	/// </summary>
	[SupportedBy("Chrome")]
	public class FontFace
	{
		/// <summary>
	/// Gets or sets The font-family.
		/// </summary>
		public string FontFamily { get; set; }
		/// <summary>
	/// Gets or sets The font-style.
		/// </summary>
		public string FontStyle { get; set; }
		/// <summary>
	/// Gets or sets The font-variant.
		/// </summary>
		public string FontVariant { get; set; }
		/// <summary>
	/// Gets or sets The font-weight.
		/// </summary>
		public string FontWeight { get; set; }
		/// <summary>
	/// Gets or sets The font-stretch.
		/// </summary>
		public string FontStretch { get; set; }
		/// <summary>
	/// Gets or sets The unicode-range.
		/// </summary>
		public string UnicodeRange { get; set; }
		/// <summary>
	/// Gets or sets The src.
		/// </summary>
		public string Src { get; set; }
		/// <summary>
	/// Gets or sets The resolved platform font family
		/// </summary>
		public string PlatformFontFamily { get; set; }
	}
}
