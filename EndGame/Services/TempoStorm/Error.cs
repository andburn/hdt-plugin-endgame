using Newtonsoft.Json;

namespace HDT.Plugins.EndGame.Services.TempoStorm
{
	internal class Error
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("status")]
		public int Status { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("statusCode")]
		public int StatusCode { get; set; }
	}
}