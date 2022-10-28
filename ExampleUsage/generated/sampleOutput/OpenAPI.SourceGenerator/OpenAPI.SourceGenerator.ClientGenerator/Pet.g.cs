
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace OpenAPI.SourceGenerator.Examples.Models
{
    public partial record Pet
    {
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("name")]
		public string? Name { get; set; }
		[JsonPropertyName("category")]
		public Category Category { get; set; }
		[JsonPropertyName("photoUrls")]
		public IEnumerable<string>? Photourls { get; set; }
		[JsonPropertyName("tags")]
		public IEnumerable<Tag> Tags { get; set; }
		/// <summary>
		/// pet status in the store
		/// </summary>
		[JsonPropertyName("status")]
		public petstatusinthestore Status { get; set; }
		public enum petstatusinthestore {
			available,
			pending,
			sold,
		}
		
	}
}