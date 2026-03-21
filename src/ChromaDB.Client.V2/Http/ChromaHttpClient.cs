using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChromaDB.Client.V2.Http
{
    /// <summary>
    /// HTTP client for communicating with ChromaDB V2 API.
    /// Handles all low-level HTTP operations, authentication, and error handling.
    /// </summary>
    public sealed class ChromaHttpClient : IDisposable
    {
        internal const string ChromaTokenHeaderName = "x-chroma-token";

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerSettings _jsonSettings;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the ChromaHttpClient class.
        /// </summary>
        /// <param name="baseUrl">The base URL of the ChromaDB server.</param>
        /// <param name="apiKey">Optional API key for authentication.</param>
        public ChromaHttpClient(string baseUrl, string apiKey = null)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/")
            };

            if (!string.IsNullOrEmpty(apiKey))
            {
                _httpClient.DefaultRequestHeaders.Add(ChromaTokenHeaderName, apiKey);
            }

            _jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
        }

        // Authentication
        /// <summary>
        /// Gets the identity of the current authenticated user.
        /// </summary>
        /// <returns>The user identity response.</returns>
        public Task<GetUserIdentityResponse> GetUserIdentityAsync() => GetAsync<GetUserIdentityResponse>("api/v2/auth/identity");

        // System
        /// <summary>
        /// Performs a health check on the server.
        /// </summary>
        /// <returns>A string indicating the health status.</returns>
        public async Task<string> HealthcheckAsync()
        {
            ThrowIfDisposed();
            using var response = await _httpClient.GetAsync("api/v2/healthcheck");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Gets the server heartbeat timestamp.
        /// </summary>
        /// <returns>The heartbeat response.</returns>
        public Task<HeartbeatResponse> HeartbeatAsync() => GetAsync<HeartbeatResponse>("api/v2/heartbeat");

        /// <summary>
        /// Performs pre-flight checks to verify server capabilities.
        /// </summary>
        /// <returns>The checklist response with server capabilities.</returns>
        public Task<ChecklistResponse> PreFlightChecksAsync() => GetAsync<ChecklistResponse>("api/v2/pre-flight-checks");

        /// <summary>
        /// Resets the server state by clearing all data.
        /// </summary>
        /// <returns>True if reset was successful, false otherwise.</returns>
        public async Task<bool> ResetAsync()
        {
            ThrowIfDisposed();
            using var response = await _httpClient.PostAsync("api/v2/reset", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        /// <summary>
        /// Gets the server version.
        /// </summary>
        /// <returns>The server version string.</returns>
        public async Task<string> VersionAsync()
        {
            ThrowIfDisposed();
            using var response = await _httpClient.GetAsync("api/v2/version");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Tenants
        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        /// <param name="payload">The tenant creation payload.</param>
        /// <returns>The create tenant response.</returns>
        public Task<CreateTenantResponse> CreateTenantAsync(CreateTenantPayload payload) => PostAsync<CreateTenantResponse>("api/v2/tenants", payload);

        /// <summary>
        /// Gets a tenant by name.
        /// </summary>
        /// <param name="tenantName">The name of the tenant.</param>
        /// <returns>The tenant details.</returns>
        public Task<GetTenantResponse> GetTenantAsync(string tenantName) => GetAsync<GetTenantResponse>($"api/v2/tenants/{tenantName}");

        /// <summary>
        /// Updates an existing tenant.
        /// </summary>
        /// <param name="tenantName">The name of the tenant to update.</param>
        /// <param name="payload">The update payload.</param>
        /// <returns>The update tenant response.</returns>
        public Task<UpdateTenantResponse> UpdateTenantAsync(string tenantName, UpdateTenantPayload payload) => PatchAsync<UpdateTenantResponse>($"api/v2/tenants/{tenantName}", payload);

        // Databases
        /// <summary>
        /// Lists all databases for a tenant.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="limit">Maximum number of databases to return.</param>
        /// <param name="offset">Number of databases to skip.</param>
        /// <returns>A list of databases.</returns>
        public Task<List<Database>> ListDatabasesAsync(string tenant, int? limit = null, int? offset = null)
        {
            var query = BuildQueryString(("limit", limit), ("offset", offset));
            return GetAsync<List<Database>>($"api/v2/tenants/{tenant}/databases{query}");
        }

        /// <summary>
        /// Creates a new database.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="payload">The database creation payload.</param>
        /// <returns>The create database response.</returns>
        public Task<CreateDatabaseResponse> CreateDatabaseAsync(string tenant, CreateDatabasePayload payload) => PostAsync<CreateDatabaseResponse>($"api/v2/tenants/{tenant}/databases", payload);

        /// <summary>
        /// Gets a database by name.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <returns>The database details.</returns>
        public Task<Database> GetDatabaseAsync(string tenant, string database) => GetAsync<Database>($"api/v2/tenants/{tenant}/databases/{database}");

        /// <summary>
        /// Deletes a database.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name to delete.</param>
        /// <returns>The delete database response.</returns>
        public Task<DeleteDatabaseResponse> DeleteDatabaseAsync(string tenant, string database) => DeleteAsync<DeleteDatabaseResponse>($"api/v2/tenants/{tenant}/databases/{database}");

        // Collections
        /// <summary>
        /// Gets a collection by its CRN.
        /// </summary>
        /// <param name="crn">The Chroma Resource Name.</param>
        /// <returns>The collection details.</returns>
        public Task<Collection> GetCollectionByCrnAsync(string crn) => GetAsync<Collection>($"api/v2/collections/{crn}");

        /// <summary>
        /// Lists all collections in a database.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="limit">Maximum number of collections to return.</param>
        /// <param name="offset">Number of collections to skip.</param>
        /// <returns>A list of collections.</returns>
        public Task<List<Collection>> ListCollectionsAsync(string tenant, string database, int? limit = null, int? offset = null)
        {
            var query = BuildQueryString(("limit", limit), ("offset", offset));
            return GetAsync<List<Collection>>($"api/v2/tenants/{tenant}/databases/{database}/collections{query}");
        }

        /// <summary>
        /// Creates a new collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="payload">The collection creation payload.</param>
        /// <returns>The created collection.</returns>
        public Task<Collection> CreateCollectionAsync(string tenant, string database, CreateCollectionPayload payload) => PostAsync<Collection>($"api/v2/tenants/{tenant}/databases/{database}/collections", payload);

        /// <summary>
        /// Gets a collection by ID.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The collection GUID.</param>
        /// <returns>The collection details.</returns>
        public Task<Collection> GetCollectionAsync(string tenant, string database, Guid collectionId) => GetAsync<Collection>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}");

        /// <summary>
        /// Updates an existing collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The collection GUID to update.</param>
        /// <param name="payload">The update payload.</param>
        /// <returns>The update collection response.</returns>
        public Task<UpdateCollectionResponse> UpdateCollectionAsync(string tenant, string database, Guid collectionId, UpdateCollectionPayload payload) => PutAsync<UpdateCollectionResponse>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}", payload);

        /// <summary>
        /// Deletes a collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The collection GUID to delete.</param>
        /// <returns>The delete collection response.</returns>
        public Task<DeleteCollectionResponse> DeleteCollectionAsync(string tenant, string database, Guid collectionId) => DeleteAsync<DeleteCollectionResponse>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}");

        /// <summary>
        /// Counts the total number of collections in a database.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <returns>The total number of collections.</returns>
        public Task<int> CountCollectionsAsync(string tenant, string database) => GetAsync<int>($"api/v2/tenants/{tenant}/databases/{database}/collections_count");

        /// <summary>
        /// Creates a fork of an existing collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The source collection GUID.</param>
        /// <param name="payload">The fork payload with new collection name.</param>
        /// <returns>The newly created collection.</returns>
        public Task<Collection> ForkCollectionAsync(string tenant, string database, Guid collectionId, ForkCollectionPayload payload) => PostAsync<Collection>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}/fork", payload);

        // Records
        /// <summary>
        /// Adds records to a collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The target collection GUID.</param>
        /// <param name="payload">The add records payload.</param>
        /// <returns>The add records response.</returns>
        public Task<AddCollectionRecordsResponse> AddRecordsAsync(string tenant, string database, Guid collectionId, AddCollectionRecordsPayload payload) => PostAsync<AddCollectionRecordsResponse>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}/add", payload);

        /// <summary>
        /// Upserts records to a collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The target collection GUID.</param>
        /// <param name="payload">The upsert records payload.</param>
        /// <returns>The upsert records response.</returns>
        public Task<UpsertCollectionRecordsResponse> UpsertRecordsAsync(string tenant, string database, Guid collectionId, UpsertCollectionRecordsPayload payload) => PostAsync<UpsertCollectionRecordsResponse>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}/upsert", payload);

        /// <summary>
        /// Updates records in a collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The target collection GUID.</param>
        /// <param name="payload">The update records payload.</param>
        /// <returns>The update records response.</returns>
        public Task<UpdateCollectionRecordsResponse> UpdateRecordsAsync(string tenant, string database, Guid collectionId, UpdateCollectionRecordsPayload payload) => PostAsync<UpdateCollectionRecordsResponse>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}/update", payload);

        /// <summary>
        /// Deletes records from a collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The target collection GUID.</param>
        /// <param name="payload">The delete records payload.</param>
        /// <returns>The delete records response.</returns>
        public Task<DeleteCollectionRecordsResponse> DeleteRecordsAsync(string tenant, string database, Guid collectionId, DeleteCollectionRecordsPayload payload) => PostAsync<DeleteCollectionRecordsResponse>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}/delete", payload);

        /// <summary>
        /// Gets records from a collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The target collection GUID.</param>
        /// <param name="payload">The get request payload.</param>
        /// <returns>The get response with retrieved records.</returns>
        public Task<GetResponse> GetRecordsAsync(string tenant, string database, Guid collectionId, GetRequestPayload payload) => PostAsync<GetResponse>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}/get", payload);

        /// <summary>
        /// Counts the total number of records in a collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The target collection GUID.</param>
        /// <param name="readLevel">Optional read consistency level.</param>
        /// <returns>The total number of records.</returns>
        public Task<int> CountRecordsAsync(string tenant, string database, Guid collectionId, ReadLevel? readLevel = null)
        {
            var query = BuildQueryString(("read_level", readLevel?.ToString()?.ToLower()));
            return GetAsync<int>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}/count{query}");
        }

        /// <summary>
        /// Performs a similarity search query on a collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The target collection GUID.</param>
        /// <param name="payload">The query request payload.</param>
        /// <param name="limit">Maximum number of results to return.</param>
        /// <param name="offset">Number of results to skip.</param>
        /// <returns>The query response with search results.</returns>
        public Task<QueryResponse> QueryRecordsAsync(string tenant, string database, Guid collectionId, QueryRequestPayload payload, int? limit = null, int? offset = null)
        {
            var query = BuildQueryString(("limit", limit), ("offset", offset));
            return PostAsync<QueryResponse>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}/query{query}", payload);
        }

        /// <summary>
        /// Performs an advanced search on a collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The target collection GUID.</param>
        /// <param name="payload">The search request payload.</param>
        /// <returns>The search response with results.</returns>
        public Task<SearchResponse> SearchRecordsAsync(string tenant, string database, Guid collectionId, SearchRequestPayload payload) => PostAsync<SearchResponse>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}/search", payload);

        /// <summary>
        /// Gets the indexing status of a collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The target collection GUID.</param>
        /// <returns>The indexing status response.</returns>
        public Task<IndexStatusResponse> GetIndexingStatusAsync(string tenant, string database, Guid collectionId) => GetAsync<IndexStatusResponse>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}/indexing_status");

        // Functions
        /// <summary>
        /// Attaches a function to a collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The target collection GUID.</param>
        /// <param name="payload">The attach function request payload.</param>
        /// <returns>The attach function response.</returns>
        public Task<AttachFunctionResponse> AttachFunctionAsync(string tenant, string database, Guid collectionId, AttachFunctionRequest payload) => PostAsync<AttachFunctionResponse>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}/functions/attach", payload);

        /// <summary>
        /// Gets an attached function by name.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The target collection GUID.</param>
        /// <param name="functionName">The name of the attached function.</param>
        /// <returns>The attached function details.</returns>
        public Task<GetAttachedFunctionResponse> GetAttachedFunctionAsync(string tenant, string database, Guid collectionId, string functionName) => GetAsync<GetAttachedFunctionResponse>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}/functions/{functionName}");

        /// <summary>
        /// Detaches a function from a collection.
        /// </summary>
        /// <param name="tenant">The tenant name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="collectionId">The target collection GUID.</param>
        /// <param name="name">The name of the attached function.</param>
        /// <param name="payload">The detach function request payload.</param>
        /// <returns>The detach function response.</returns>
        public Task<DetachFunctionResponse> DetachFunctionAsync(string tenant, string database, Guid collectionId, string name, DetachFunctionRequest payload) => PostAsync<DetachFunctionResponse>($"api/v2/tenants/{tenant}/databases/{database}/collections/{collectionId}/attached_functions/{name}/detach", payload);

        private async Task<T> GetAsync<T>(string url)
        {
            ThrowIfDisposed();
            using var response = await _httpClient.GetAsync(url);
            await EnsureSuccessWithErrorHandling(response);
            return await response.Content.ReadFromJsonAsync<T>(_jsonSettings);
        }

        private async Task<T> PostAsync<T>(string url, object payload = null)
        {
            ThrowIfDisposed();
            using var response = await _httpClient.PostAsJsonAsync(url, payload, _jsonSettings);
            await EnsureSuccessWithErrorHandling(response);
            return await response.Content.ReadFromJsonAsync<T>(_jsonSettings);
        }

        private async Task<T> PutAsync<T>(string url, object payload)
        {
            ThrowIfDisposed();
            using var response = await _httpClient.PutAsJsonAsync(url, payload, _jsonSettings);
            await EnsureSuccessWithErrorHandling(response);
            return await response.Content.ReadFromJsonAsync<T>(_jsonSettings);
        }

        private async Task<T> PatchAsync<T>(string url, object payload)
        {
            ThrowIfDisposed();
            using var response = await _httpClient.PatchAsJsonAsync(url, payload, _jsonSettings);
            await EnsureSuccessWithErrorHandling(response);
            return await response.Content.ReadFromJsonAsync<T>(_jsonSettings);
        }

        private async Task<T> DeleteAsync<T>(string url)
        {
            ThrowIfDisposed();
            using var response = await _httpClient.DeleteAsync(url);
            await EnsureSuccessWithErrorHandling(response);
            return await response.Content.ReadFromJsonAsync<T>(_jsonSettings);
        }

        private async Task EnsureSuccessWithErrorHandling(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            try
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new ChromaApiException(response.StatusCode, error?.Error, error?.Message);
            }
            catch (JsonException)
            {
                response.EnsureSuccessStatusCode();
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ChromaHttpClient));
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _httpClient.Dispose();

            _disposed = true;
        }

        /// <summary>
        /// Disposes the HTTP client and releases resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~ChromaHttpClient()
        {
            Dispose(disposing: false);
        }

        private static string BuildQueryString(params (string key, object value)[] parameters)
        {
            var queryParams = new List<string>();
            foreach (var (key, value) in parameters)
            {
                if (value != null)
                {
                    queryParams.Add($"{key}={Uri.EscapeDataString(value.ToString())}");
                }
            }
            return queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
        }
    }
}