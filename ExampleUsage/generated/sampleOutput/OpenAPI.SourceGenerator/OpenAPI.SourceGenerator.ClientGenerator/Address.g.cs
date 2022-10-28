
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace OpenAPI.SourceGenerator.Examples.Models
{
    public partial record Address
    {
		[JsonPropertyName("street")]
		public string Street { get; set; }
		[JsonPropertyName("city")]
		public string City { get; set; }
		[JsonPropertyName("state")]
		public string State { get; set; }
		[JsonPropertyName("zip")]
		public string Zip { get; set; }
	}
}