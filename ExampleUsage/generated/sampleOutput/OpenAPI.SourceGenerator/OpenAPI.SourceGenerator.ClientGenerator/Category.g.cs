
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace OpenAPI.SourceGenerator.Examples.Models
{
    public partial record Category
    {
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}