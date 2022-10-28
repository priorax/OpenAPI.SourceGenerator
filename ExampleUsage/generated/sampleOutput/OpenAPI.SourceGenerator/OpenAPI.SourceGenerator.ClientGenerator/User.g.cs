
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace OpenAPI.SourceGenerator.Examples.Models
{
    public partial record User
    {
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("username")]
		public string Username { get; set; }
		[JsonPropertyName("firstName")]
		public string Firstname { get; set; }
		[JsonPropertyName("lastName")]
		public string Lastname { get; set; }
		[JsonPropertyName("email")]
		public string Email { get; set; }
		[JsonPropertyName("password")]
		public string Password { get; set; }
		[JsonPropertyName("phone")]
		public string Phone { get; set; }
		/// <summary>
		/// User Status
		/// </summary>
		[JsonPropertyName("userStatus")]
		public int Userstatus { get; set; }
	}
}