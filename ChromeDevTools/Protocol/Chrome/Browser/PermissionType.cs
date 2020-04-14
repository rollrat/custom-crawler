using MasterDevs.ChromeDevTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;


namespace MasterDevs.ChromeDevTools.Protocol.Chrome.Browser{
	/// <summary>

	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum PermissionType
	{
			AccessibilityEvents,
			AudioCapture,
			BackgroundSync,
			BackgroundFetch,
			ClipboardReadWrite,
			ClipboardSanitizedWrite,
			DurableStorage,
			Flash,
			Geolocation,
			Midi,
			MidiSysex,
			Nfc,
			Notifications,
			PaymentHandler,
			PeriodicBackgroundSync,
			ProtectedMediaIdentifier,
			Sensors,
			VideoCapture,
			IdleDetection,
			WakeLockScreen,
			WakeLockSystem,
	}
}
