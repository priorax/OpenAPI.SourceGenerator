
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace OpenAPI.SourceGenerator.Examples.Models
{
    public partial record ApiResponse
    {
		[JsonPropertyName("code")]
		public int Code { get; set; }
		[JsonPropertyName("type")]
		public string Type { get; set; }
		[JsonPropertyName("message")]
		public string Message { get; set; }
	}
}