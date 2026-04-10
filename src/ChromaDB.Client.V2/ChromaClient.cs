using MirDev.ChromaDB.Client.V2.Http;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MirDev.ChromaDB.Client.V2
{
    /// <summary>
    /// Main client for interacting with ChromaDB V2 API.
    /// Provides methods for managing tenants, databases, collections, and records.
    /// </summary>
    public class ChromaClient
    {
        private readonly ChromaHttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the ChromaClient class.
        /// </summary>
        /// <param name="baseUrl">The base URL of the ChromaDB server. Defaults to "http://localhost:8000".</param>
        /// <param name="apiKey">Optional API key for authentication.</param>
        public ChromaClient(string baseUrl = "http://localhost:8000", string apiKey = null)
        {
            _httpClient = new ChromaHttpClient(baseUrl, apiKey);
        }
        /// <summary>
        /// Performs a health check on the ChromaDB server.
        /// </summary>
        /// <returns>A string indicating the server health status.</returns>
        public Task<string> HealthcheckAsync() => _httpClient.HealthcheckAsync();

        /// <summary>
        /// Gets the server heartbeat timestamp.
        /// </summary>
        /// <returns>A response containing the current heartbeat nanosecond timestamp.</returns>
        public Task<HeartbeatResponse> HeartbeatAsync() => _httpClient.HeartbeatAsync();

        /// <summary>
        /// Performs pre-flight checks to verify server capabilities.
        /// </summary>
        /// <returns>A response containing server configuration information.</returns>
        public Task<ChecklistResponse> PreFlightChecksAsync() => _httpClient.PreFlightChecksAsync();

        /// <summary>
        /// Resets the server state by clearing all data.
        /// </summary>
        /// <returns>True if the reset was successful, false otherwise.</returns>
        public Task<bool> ResetAsync() => _httpClient.ResetAsync();

        /// <summary>
        /// Gets the server version.
        /// </summary>
        /// <returns>The server version string.</returns>
        public Task<string> VersionAsync() => _httpClient.VersionAsync();

        /// <summary>
        /// Gets the identity of the current authenticated user.
        /// </summary>
        /// <returns>A response containing user identity information.</returns>
        public Task<GetUserIdentityResponse> GetUserIdentityAsync() => _httpClient.GetUserIdentityAsync();

        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        /// <param name="name">The name of the tenant to create.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        public Task<CreateTenantResponse> CreateTenantAsync(string name)
        {
            var payload = new CreateTenantPayload { Name = name };
            return _httpClient.CreateTenantAsync(payload);
        }

        /// <summary>
        /// Retrieves information about a specific tenant.
        /// </summary>
        /// <param name="tenantName">The name of the tenant to retrieve.</param>
        /// <returns>A response containing tenant details.</returns>
        public Task<GetTenantResponse> GetTenantAsync(string tenantName)
        {
            return _httpClient.GetTenantAsync(tenantName);
        }

        /// <summary>
        /// Updates an existing tenant.
        /// </summary>
        /// <param name="tenantName">The name of the tenant to update.</param>
        /// <param name="resourceName">The new resource name for the tenant.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        public Task<UpdateTenantResponse> UpdateTenantAsync(string tenantName, string resourceName)
        {
            var payload = new UpdateTenantPayload { ResourceName = resourceName };
            return _httpClient.UpdateTenantAsync(tenantName, payload);
        }

        /// <summary>
        /// Lists all databases for a tenant.
        /// </summary>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="limit">Maximum number of databases to return.</param>
        /// <param name="offset">Number of databases to skip.</param>
        /// <returns>A list of databases.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant is not specified.</exception>
        public Task<List<Database>> ListDatabasesAsync(string tenant, int? limit = null, int? offset = null)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            return _httpClient.ListDatabasesAsync(tenant, limit, offset);
        }

        /// <summary>
        /// Creates a new database.
        /// </summary>
        /// <param name="name">The name of the database to create.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant is not specified.</exception>
        public Task<CreateDatabaseResponse> CreateDatabaseAsync(string name, string tenant)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            var payload = new CreateDatabasePayload { Name = name };
            return _httpClient.CreateDatabaseAsync(tenant, payload);
        }

        /// <summary>
        /// Retrieves information about a specific database.
        /// </summary>
        /// <param name="database">The name of the database to retrieve.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <returns>The database details.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant is not specified.</exception>
        public Task<Database> GetDatabaseAsync(string database, string tenant)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            return _httpClient.GetDatabaseAsync(tenant, database);
        }

        /// <summary>
        /// Deletes a database.
        /// </summary>
        /// <param name="database">The name of the database to delete.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant is not specified.</exception>
        public Task<DeleteDatabaseResponse> DeleteDatabaseAsync(string database, string tenant)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            return _httpClient.DeleteDatabaseAsync(tenant, database);
        }

        /// <summary>
        /// Retrieves a collection by its CRN (Chroma Resource Name).
        /// </summary>
        /// <param name="crn">The CRN of the collection.</param>
        /// <returns>The collection details.</returns>
        public Task<Collection> GetCollectionByCrnAsync(string crn) => _httpClient.GetCollectionByCrnAsync(crn);

        /// <summary>
        /// Lists all collections in a database.
        /// </summary>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="limit">Maximum number of collections to return.</param>
        /// <param name="offset">Number of collections to skip.</param>
        /// <returns>A list of collections.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<List<Collection>> ListCollectionsAsync(string database, string tenant, int? limit = null, int? offset = null)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");
            return _httpClient.ListCollectionsAsync(tenant, database, limit, offset);
        }

        /// <summary>
        /// Creates a new collection.
        /// </summary>
        /// <param name="name">The name of the collection.</param>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="metadata">Optional metadata for the collection.</param>
        /// <param name="configuration">Optional configuration for the collection.</param>
        /// <param name="schema">Optional schema definition.</param>
        /// <param name="getOrCreate">If true, returns existing collection if name already exists.</param>
        /// <returns>The created or existing collection.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<Collection> CreateCollectionAsync(string name, string database, string tenant,
            Dictionary<string, object> metadata = null, CollectionConfiguration configuration = null, Schema schema = null, bool getOrCreate = false)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");

            var payload = new CreateCollectionPayload
            {
                Name = name,
                Metadata = metadata,
                Configuration = configuration,
                Schema = schema,
                GetOrCreate = getOrCreate
            };

            return _httpClient.CreateCollectionAsync(tenant, database, payload);
        }

        /// <summary>
        /// Retrieves a collection by its ID.
        /// </summary>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="collectionId">The GUID of the collection.</param>
        /// <returns>The collection details.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<Collection> GetCollectionAsync(string database, string tenant, Guid collectionId)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");
            return _httpClient.GetCollectionAsync(tenant, database, collectionId);
        }

        /// <summary>
        /// Updates an existing collection.
        /// </summary>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="collectionId">The GUID of the collection to update.</param>
        /// <param name="newName">Optional new name for the collection.</param>
        /// <param name="newMetadata">Optional new metadata for the collection.</param>
        /// <param name="newConfiguration">Optional new configuration.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<UpdateCollectionResponse> UpdateCollectionAsync(string database, string tenant, Guid collectionId, string newName = null,
            Dictionary<string, object> newMetadata = null, UpdateCollectionConfiguration newConfiguration = null)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");

            var payload = new UpdateCollectionPayload
            {
                NewName = newName,
                NewMetadata = newMetadata,
                NewConfiguration = newConfiguration
            };

            return _httpClient.UpdateCollectionAsync(tenant, database, collectionId, payload);
        }

        /// <summary>
        /// Deletes a collection.
        /// </summary>
        /// <param name="collectionId">The GUID of the collection to delete.</param>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<DeleteCollectionResponse> DeleteCollectionAsync(Guid collectionId, string database, string tenant)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");
            return _httpClient.DeleteCollectionAsync(tenant, database, collectionId);
        }

        /// <summary>
        /// Counts the total number of collections in a database.
        /// </summary>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <returns>The total number of collections.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<int> CountCollectionsAsync(string database, string tenant)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");
            return _httpClient.CountCollectionsAsync(tenant, database);
        }

        /// <summary>
        /// Creates a fork (copy) of an existing collection.
        /// </summary>
        /// <param name="collectionId">The GUID of the source collection.</param>
        /// <param name="newName">The name for the new collection.</param>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <returns>The newly created collection.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<Collection> ForkCollectionAsync(Guid collectionId, string newName, string database, string tenant)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");

            var payload = new ForkCollectionPayload { NewName = newName };
            return _httpClient.ForkCollectionAsync(tenant, database, collectionId, payload);
        }

        /// <summary>
        /// Adds records to a collection.
        /// </summary>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="ids">List of unique identifiers for the records.</param>
        /// <param name="embeddings">The embeddings for the records. Can be List&lt;float&gt; or List&lt;List&lt;float&gt;&gt;.</param>
        /// <param name="documents">Optional list of text documents.</param>
        /// <param name="metadatas">Optional list of metadata dictionaries.</param>
        /// <param name="uris">Optional list of URIs.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<AddCollectionRecordsResponse> AddRecordsAsync(
            string database, string tenant, Guid collectionId, List<string> ids, object embeddings,
            List<string> documents = null, List<Dictionary<string, object>> metadatas = null, List<string> uris = null)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");

            var payload = new AddCollectionRecordsPayload
            {
                Ids = ids,
                Embeddings = embeddings,
                Documents = documents,
                Metadatas = metadatas,
                Uris = uris
            };

            return _httpClient.AddRecordsAsync(tenant, database, collectionId, payload);
        }

        /// <summary>
        /// Upserts records to a collection (updates existing or inserts new).
        /// </summary>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="ids">List of unique identifiers for the records.</param>
        /// <param name="embeddings">The embeddings for the records.</param>
        /// <param name="documents">Optional list of text documents.</param>
        /// <param name="metadatas">Optional list of metadata dictionaries.</param>
        /// <param name="uris">Optional list of URIs.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<UpsertCollectionRecordsResponse> UpsertRecordsAsync(string database, string tenant, Guid collectionId, List<string> ids, object embeddings,
            List<string> documents = null, List<Dictionary<string, object>> metadatas = null, List<string> uris = null)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");

            var payload = new UpsertCollectionRecordsPayload
            {
                Ids = ids,
                Embeddings = embeddings,
                Documents = documents,
                Metadatas = metadatas,
                Uris = uris
            };

            return _httpClient.UpsertRecordsAsync(tenant, database, collectionId, payload);
        }

        /// <summary>
        /// Updates existing records in a collection.
        /// </summary>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="ids">List of unique identifiers for the records to update.</param>
        /// <param name="embeddings">Optional new embeddings.</param>
        /// <param name="documents">Optional new documents.</param>
        /// <param name="metadatas">Optional new metadata.</param>
        /// <param name="uris">Optional new URIs.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<UpdateCollectionRecordsResponse> UpdateRecordsAsync(string database, string tenant, Guid collectionId, List<string> ids,
            object embeddings = null, List<string> documents = null, List<Dictionary<string, object>> metadatas = null, List<string> uris = null)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");

            var payload = new UpdateCollectionRecordsPayload
            {
                Ids = ids,
                Embeddings = embeddings,
                Documents = documents,
                Metadatas = metadatas,
                Uris = uris
            };

            return _httpClient.UpdateRecordsAsync(tenant, database, collectionId, payload);
        }

        /// <summary>
        /// Deletes records from a collection.
        /// </summary>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="ids">Optional list of IDs to delete.</param>
        /// <param name="where">Optional filter condition for deletion.</param>
        /// <param name="whereDocument">Optional document filter condition.</param>
        /// <param name="limit">Maximum number of records to delete.</param>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <returns>A response containing the number of deleted records.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<DeleteCollectionRecordsResponse> DeleteRecordsAsync(string database, string tenant, Guid collectionId, List<string> ids = null,
            object where = null, object whereDocument = null, int? limit = null)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");

            var payload = new DeleteCollectionRecordsPayload
            {
                Ids = ids,
                Where = where,
                WhereDocument = whereDocument,
                Limit = limit
            };

            return _httpClient.DeleteRecordsAsync(tenant, database, collectionId, payload);
        }

        /// <summary>
        /// Retrieves records from a collection with optional filtering.
        /// </summary>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="ids">Optional list of IDs to retrieve.</param>
        /// <param name="where">Optional filter condition.</param>
        /// <param name="whereDocument">Optional document filter condition.</param>
        /// <param name="include">Optional list of fields to include in the response.</param>
        /// <param name="limit">Maximum number of records to return.</param>
        /// <param name="offset">Number of records to skip.</param>
        /// <returns>A response containing the retrieved records.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<GetResponse> GetRecordsAsync(string database, string tenant, Guid collectionId, List<string> ids = null, object where = null,
            object whereDocument = null, List<Include> include = null, int? limit = null, int? offset = null)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");

            var payload = new GetRequestPayload
            {
                Ids = ids,
                Where = where,
                WhereDocument = whereDocument,
                Include = include,
                Limit = limit,
                Offset = offset
            };

            return _httpClient.GetRecordsAsync(tenant, database, collectionId, payload);
        }

        /// <summary>
        /// Counts the total number of records in a collection.
        /// </summary>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="readLevel">Optional read consistency level.</param>
        /// <returns>The total number of records.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<int> CountRecordsAsync(string database, string tenant, Guid collectionId, ReadLevel? readLevel = null)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");
            return _httpClient.CountRecordsAsync(tenant, database, collectionId, readLevel);
        }

        /// <summary>
        /// Performs a similarity search query on a collection.
        /// </summary>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="queryEmbeddings">List of query embeddings to search for.</param>
        /// <param name="nResults">Number of results to return per query.</param>
        /// <param name="where">Optional filter condition.</param>
        /// <param name="whereDocument">Optional document filter condition.</param>
        /// <param name="include">Optional list of fields to include in the response.</param>
        /// <param name="ids">Optional list of IDs to filter by.</param>
        /// <param name="limit">Maximum number of results to return.</param>
        /// <param name="offset">Number of results to skip.</param>
        /// <returns>A response containing the query results.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<QueryResponse> QueryRecordsAsync(string database, string tenant, Guid collectionId, List<List<float>> queryEmbeddings,
            int? nResults = null, object where = null, object whereDocument = null, List<Include> include = null,
            List<string> ids = null, int? limit = null, int? offset = null)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");

            var payload = new QueryRequestPayload
            {
                QueryEmbeddings = queryEmbeddings,
                NResults = nResults,
                Where = where,
                WhereDocument = whereDocument,
                Include = include,
                Ids = ids
            };

            return _httpClient.QueryRecordsAsync(tenant, database, collectionId, payload, limit, offset);
        }

        /// <summary>
        /// Performs a search query with advanced filtering and grouping options.
        /// </summary>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="searches">List of search configurations.</param>
        /// <param name="readLevel">Optional read consistency level.</param>
        /// <returns>A response containing the search results.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<SearchResponse> SearchRecordsAsync(string database, string tenant, Guid collectionId, List<SearchPayload> searches,
            ReadLevel? readLevel = null)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");

            var payload = new SearchRequestPayload
            {
                Searches = searches,
                ReadLevel = readLevel
            };

            return _httpClient.SearchRecordsAsync(tenant, database, collectionId, payload);
        }

        /// <summary>
        /// Gets the indexing status of a collection.
        /// </summary>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <returns>A response containing indexing progress information.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<IndexStatusResponse> GetIndexingStatusAsync(Guid collectionId, string database, string tenant)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");
            return _httpClient.GetIndexingStatusAsync(tenant, database, collectionId);
        }

        // Functions
        /// <summary>
        /// Attaches a function to a collection.
        /// </summary>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="functionId">The GUID of the function to attach.</param>
        /// <param name="name">The name for the attached function.</param>
        /// <param name="outputCollection">The output collection name.</param>
        /// <param name="params">Optional parameters for the function.</param>
        /// <returns>A response containing the attached function information.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<AttachFunctionResponse> AttachFunctionAsync(string database, string tenant, Guid collectionId, Guid functionId, string name,
            string outputCollection, object @params = null)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");

            var payload = new AttachFunctionRequest
            {
                FunctionId = functionId,
                Name = name,
                OutputCollection = outputCollection,
                Params = @params
            };

            return _httpClient.AttachFunctionAsync(tenant, database, collectionId, payload);
        }

        /// <summary>
        /// Gets an attached function by name.
        /// </summary>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="functionName">The name of the attached function.</param>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <returns>A response containing the attached function details.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<GetAttachedFunctionResponse> GetAttachedFunctionAsync(Guid collectionId, string functionName,
            string database, string tenant)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");
            return _httpClient.GetAttachedFunctionAsync(tenant, database, collectionId, functionName);
        }

        /// <summary>
        /// Detaches a function from a collection.
        /// </summary>
        /// <param name="database">The database name. If null, uses the current database.</param>
        /// <param name="tenant">The tenant name. If null, uses the current tenant.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="name">The name of the attached function.</param>
        /// <param name="deleteOutput">Whether to delete the output collection.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when tenant or database is not specified.</exception>
        public Task<DetachFunctionResponse> DetachFunctionAsync(string database, string tenant, Guid collectionId, string name, bool deleteOutput = false)
        {
            _ = tenant ?? throw new InvalidOperationException("Tenant not specified");
            _ = database ?? throw new InvalidOperationException("Database not specified");

            var payload = new DetachFunctionRequest { DeleteOutput = deleteOutput };
            return _httpClient.DetachFunctionAsync(tenant, database, collectionId, name, payload);
        }
    }
}