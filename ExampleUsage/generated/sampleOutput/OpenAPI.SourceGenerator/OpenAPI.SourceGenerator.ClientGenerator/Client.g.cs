
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.Json;
using OpenAPI.SourceGenerator.Examples.Models;
namespace OpenAPI.SourceGenerator.Examples
{
	//<summary>
	// This is a sample Pet Store Server based on the OpenAPI 3.0 specification.  You can find out more about
	// Swagger at [https://swagger.io](https://swagger.io). In the third iteration of the pet store, we've switched to the design first approach!
	// You can now help us improve the API whether it's by making changes to the definition itself or to the code.
	// That way, with time, we can improve the API in general, and expose some of the new features in OAS3.
	// 
	// _If you're looking for the Swagger 2.0/OAS 2.0 version of Petstore, then click [here](https://editor.swagger.io/?url=https://petstore.swagger.io/v2/swagger.yaml). Alternatively, you can load via the `Edit > Load Petstore OAS 2.0` menu option!_
	// 
	// Some useful links:
	// - [The Pet Store repository](https://github.com/swagger-api/swagger-petstore)
	// - [The source API definition for the Pet Store](https://github.com/swagger-api/swagger-petstore/blob/master/src/main/resources/openapi.yaml)
	//</summary>
    public partial class Client
    {
		/// <summary>
		/// Update an existing pet
		/// </summary>
		public Task updatePet(){throw new NotImplementedException();}
		/// <summary>
		/// Add a new pet to the store
		/// </summary>
		public Task addPet(){throw new NotImplementedException();}
		/// <summary>
		/// Finds Pets by status
		/// </summary>
		public async Task<IEnumerable<Pet>> findPetsByStatus(PetFindbystatus status) {
			var queryParamStrings = new List<string>();
			queryParamStrings.Add($"status={status}");
			var path = $"/pet/findByStatus?{string.Join("&", queryParamStrings)}";
			var response = await _client.GetStreamAsync(path);
			var content = await JsonSerializer.DeserializeAsync<IEnumerable<Pet>>(response);
			return content!;
		}
		
		/// <summary>
		/// Finds Pets by tags
		/// </summary>
		public async Task<IEnumerable<Pet>> findPetsByTags(IEnumerable<string> tags) {
			var queryParamStrings = new List<string>();
			queryParamStrings.Add($"tags={string.Join(',', tags)}");
			var path = $"/pet/findByTags?{string.Join("&", queryParamStrings)}";
			var response = await _client.GetStreamAsync(path);
			var content = await JsonSerializer.DeserializeAsync<IEnumerable<Pet>>(response);
			return content!;
		}
		
		/// <summary>
		/// Find pet by ID
		/// </summary>
		public async Task<Pet> getPetById(int petId) {
			var path = "/pet/{petId}";
			var response = await _client.GetStreamAsync(path);
			var content = await JsonSerializer.DeserializeAsync<Pet>(response);
			return content!;
		}
		
		/// <summary>
		/// Updates a pet in the store with form data
		/// </summary>
		public Task updatePetWithForm(){throw new NotImplementedException();}
		/// <summary>
		/// Deletes a pet
		/// </summary>
		public Task<HttpResponseMessage> deletePet(int petId) {
			var requestUri = "/pet/{petId}";
			requestUri = requestUri.Replace("{petId}", petId.ToString());
			return _client.DeleteAsync(requestUri);
		}
		/// <summary>
		/// uploads an image
		/// </summary>
		public Task uploadFile(){throw new NotImplementedException();}
		/// <summary>
		/// Returns pet inventories by status
		/// </summary>
		public async Task<object> getInventory() {
			var path = "/store/inventory";
			var response = await _client.GetStreamAsync(path);
			var content = await JsonSerializer.DeserializeAsync<object>(response);
			return content!;
		}
		
		/// <summary>
		/// Place an order for a pet
		/// </summary>
		public Task placeOrder(){throw new NotImplementedException();}
		/// <summary>
		/// Find purchase order by ID
		/// </summary>
		public async Task<Order> getOrderById(int orderId) {
			var path = "/store/order/{orderId}";
			var response = await _client.GetStreamAsync(path);
			var content = await JsonSerializer.DeserializeAsync<Order>(response);
			return content!;
		}
		
		/// <summary>
		/// Delete purchase order by ID
		/// </summary>
		public Task<HttpResponseMessage> deleteOrder(int orderId) {
			var requestUri = "/store/order/{orderId}";
			requestUri = requestUri.Replace("{orderId}", orderId.ToString());
			return _client.DeleteAsync(requestUri);
		}
		/// <summary>
		/// Create user
		/// </summary>
		public Task createUser(){throw new NotImplementedException();}
		/// <summary>
		/// Creates list of users with given input array
		/// </summary>
		public Task createUsersWithListInput(){throw new NotImplementedException();}
		/// <summary>
		/// Logs user into the system
		/// </summary>
		public async Task<string> loginUser(string username,string password) {
			var queryParamStrings = new List<string>();
			queryParamStrings.Add($"username={username}");
			queryParamStrings.Add($"password={password}");
			var path = $"/user/login?{string.Join("&", queryParamStrings)}";
			var response = await _client.GetStreamAsync(path);
			var content = await JsonSerializer.DeserializeAsync<string>(response);
			return content!;
		}
		
		/// <summary>
		/// Get user by user name
		/// </summary>
		public async Task<User> getUserByName(string username) {
			var path = "/user/{username}";
			var response = await _client.GetStreamAsync(path);
			var content = await JsonSerializer.DeserializeAsync<User>(response);
			return content!;
		}
		
		/// <summary>
		/// Update user
		/// </summary>
		public Task updateUser(){throw new NotImplementedException();}
		/// <summary>
		/// Delete user
		/// </summary>
		public Task<HttpResponseMessage> deleteUser(string username) {
			var requestUri = "/user/{username}";
			requestUri = requestUri.Replace("{username}", username.ToString());
			return _client.DeleteAsync(requestUri);
		}	}
}