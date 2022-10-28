
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace OpenAPI.SourceGenerator.Examples.Models
{
    public partial record Customer
    {
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("username")]
		public string Username { get; set; }
		[JsonPropertyName("address")]
		public IEnumerable<Address> Address { get; set; }
	}
}