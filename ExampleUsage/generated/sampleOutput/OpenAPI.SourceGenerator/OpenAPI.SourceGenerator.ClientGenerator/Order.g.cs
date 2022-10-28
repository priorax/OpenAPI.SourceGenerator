
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace OpenAPI.SourceGenerator.Examples.Models
{
    public partial record Order
    {
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("petId")]
		public int Petid { get; set; }
		[JsonPropertyName("quantity")]
		public int Quantity { get; set; }
		[JsonPropertyName("shipDate")]
		public DateTime Shipdate { get; set; }
		/// <summary>
		/// Order Status
		/// </summary>
		[JsonPropertyName("status")]
		public OrderStatus Status { get; set; }
		[JsonPropertyName("complete")]
		public bool Complete { get; set; }
		public enum OrderStatus {
			placed,
			approved,
			delivered,
		}
		
	}
}