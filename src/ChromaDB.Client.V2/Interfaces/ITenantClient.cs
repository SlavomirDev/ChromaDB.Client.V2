using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MirDev.ChromaDB.Client.V2
{
    /// <summary>
    /// Provides tenant-scoped operations for ChromaDB.
    /// All methods in this interface are bound to a specific tenant,
    /// eliminating the need to pass the tenant parameter with each call.
    /// </summary>
    /// <remarks>
    /// This interface is typically obtained by calling <see cref="ChromaClient.GetTenantClient(string)"/>.
    /// The underlying tenant context is preserved across all operations.
    /// </remarks>
    public interface ITenantClient : IDisposable
    {
        /// <summary>
        /// Gets the tenant name that this client is bound to.
        /// </summary>
        string Tenant { get; }

        /// <summary>
        /// Lists all databases within the current tenant.
        /// </summary>
        /// <param name="limit">The maximum number of databases to return. Use null for no limit.</param>
        /// <param name="offset">The number of databases to skip before starting to return results.</param>
        /// <returns>A list of databases belonging to the tenant.</returns>
        Task<List<Database>> ListDatabasesAsync(int? limit = null, int? offset = null);

        /// <summary>
        /// Creates a new database within the current tenant.
        /// </summary>
        /// <param name="name">The name of the database to create. Must be unique within the tenant.</param>
        /// <returns>A response indicating the result of the creation operation.</returns>
        Task<CreateDatabaseResponse> CreateDatabaseAsync(string name);

        /// <summary>
        /// Retrieves detailed information about a specific database.
        /// </summary>
        /// <param name="database">The name of the database to retrieve.</param>
        /// <returns>The database details including ID and configuration.</returns>
        Task<Database> GetDatabaseAsync(string database);

        /// <summary>
        /// Deletes a database from the current tenant.
        /// </summary>
        /// <param name="database">The name of the database to delete.</param>
        /// <returns>A response indicating the result of the deletion operation.</returns>
        /// <remarks>This operation is irreversible. All collections and records within the database will be permanently removed.</remarks>
        Task<DeleteDatabaseResponse> DeleteDatabaseAsync(string database);

        /// <summary>
        /// Lists all collections within a specified database.
        /// </summary>
        /// <param name="database">The name of the database containing the collections.</param>
        /// <param name="limit">The maximum number of collections to return. Use null for no limit.</param>
        /// <param name="offset">The number of collections to skip before starting to return results.</param>
        /// <returns>A list of collections in the specified database.</returns>
        Task<List<Collection>> ListCollectionsAsync(string database, int? limit = null, int? offset = null);

        /// <summary>
        /// Creates a new collection within the specified database.
        /// </summary>
        /// <param name="name">The name of the collection to create.</param>
        /// <param name="database">The name of the database where the collection will be created.</param>
        /// <param name="metadata">Optional metadata key-value pairs to associate with the collection.</param>
        /// <param name="configuration">Optional index configuration settings (HNSW, SPANN).</param>
        /// <param name="schema">Optional schema definition for typed fields.</param>
        /// <param name="getOrCreate">If true, returns an existing collection with the same name instead of throwing an error.</param>
        /// <returns>The created or existing collection details.</returns>
        Task<Collection> CreateCollectionAsync(string name, string database,
            Dictionary<string, object> metadata = null, CollectionConfiguration configuration = null, Schema schema = null, bool getOrCreate = false);

        /// <summary>
        /// Retrieves a collection by its unique identifier.
        /// </summary>
        /// <param name="collectionId">The GUID of the collection to retrieve.</param>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <returns>The collection details.</returns>
        Task<Collection> GetCollectionAsync(Guid collectionId, string database);

        /// <summary>
        /// Updates an existing collection's properties.
        /// </summary>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <param name="collectionId">The GUID of the collection to update.</param>
        /// <param name="newName">Optional new name for the collection.</param>
        /// <param name="newMetadata">Optional metadata to replace existing metadata.</param>
        /// <param name="newConfiguration">Optional configuration updates for the index.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        Task<UpdateCollectionResponse> UpdateCollectionAsync(string database, Guid collectionId, string newName = null,
            Dictionary<string, object> newMetadata = null, UpdateCollectionConfiguration newConfiguration = null);

        /// <summary>
        /// Deletes a collection and all its records.
        /// </summary>
        /// <param name="collectionId">The GUID of the collection to delete.</param>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <returns>A response indicating the result of the deletion operation.</returns>
        /// <remarks>This operation is irreversible. All records within the collection will be permanently removed.</remarks>
        Task<DeleteCollectionResponse> DeleteCollectionAsync(Guid collectionId, string database);

        /// <summary>
        /// Counts the total number of collections in a database.
        /// </summary>
        /// <param name="database">The name of the database to count collections for.</param>
        /// <returns>The total number of collections.</returns>
        Task<int> CountCollectionsAsync(string database);

        /// <summary>
        /// Creates a fork (copy) of an existing collection.
        /// </summary>
        /// <param name="collectionId">The GUID of the source collection to fork.</param>
        /// <param name="newName">The name for the new collection.</param>
        /// <param name="database">The name of the database containing the source collection.</param>
        /// <returns>The newly created collection details.</returns>
        Task<Collection> ForkCollectionAsync(Guid collectionId, string newName, string database);

        /// <summary>
        /// Adds new records to a collection.
        /// </summary>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="ids">List of unique identifiers for the records.</param>
        /// <param name="embeddings">Vector embeddings. Can be a single vector (List&lt;float&gt;) or multiple vectors (List&lt;List&lt;float&gt;&gt;).</param>
        /// <param name="documents">Optional list of text documents associated with each record.</param>
        /// <param name="metadatas">Optional list of metadata dictionaries for each record.</param>
        /// <param name="uris">Optional list of URIs associated with each record.</param>
        /// <returns>A response indicating the result of the add operation.</returns>
        Task<AddCollectionRecordsResponse> AddRecordsAsync(string database, Guid collectionId, List<string> ids, object embeddings,
            List<string> documents = null, List<Dictionary<string, object>> metadatas = null, List<string> uris = null);

        /// <summary>
        /// Upserts records to a collection (inserts new or updates existing records).
        /// </summary>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="ids">List of unique identifiers for the records.</param>
        /// <param name="embeddings">Vector embeddings for the records.</param>
        /// <param name="documents">Optional list of text documents.</param>
        /// <param name="metadatas">Optional list of metadata dictionaries.</param>
        /// <param name="uris">Optional list of URIs.</param>
        /// <returns>A response indicating the result of the upsert operation.</returns>
        Task<UpsertCollectionRecordsResponse> UpsertRecordsAsync(string database, Guid collectionId, List<string> ids, object embeddings,
            List<string> documents = null, List<Dictionary<string, object>> metadatas = null, List<string> uris = null);

        /// <summary>
        /// Updates existing records in a collection.
        /// </summary>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="ids">List of unique identifiers for the records to update.</param>
        /// <param name="embeddings">Optional new embeddings for the records.</param>
        /// <param name="documents">Optional new documents for the records.</param>
        /// <param name="metadatas">Optional new metadata for the records.</param>
        /// <param name="uris">Optional new URIs for the records.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        Task<UpdateCollectionRecordsResponse> UpdateRecordsAsync(string database, Guid collectionId, List<string> ids,
            object embeddings = null, List<string> documents = null, List<Dictionary<string, object>> metadatas = null, List<string> uris = null);

        /// <summary>
        /// Deletes records from a collection based on IDs or filter conditions.
        /// </summary>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="ids">Optional list of IDs to delete. If not provided, deletion is based on where conditions.</param>
        /// <param name="where">Optional metadata filter condition.</param>
        /// <param name="whereDocument">Optional document content filter condition.</param>
        /// <param name="limit">Maximum number of records to delete.</param>
        /// <returns>A response containing the number of deleted records.</returns>
        Task<DeleteCollectionRecordsResponse> DeleteRecordsAsync(string database, Guid collectionId, List<string> ids = null,
            object where = null, object whereDocument = null, int? limit = null);

        /// <summary>
        /// Retrieves records from a collection with optional filtering.
        /// </summary>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="ids">Optional list of IDs to retrieve.</param>
        /// <param name="where">Optional metadata filter condition.</param>
        /// <param name="whereDocument">Optional document content filter condition.</param>
        /// <param name="include">Optional list of fields to include in the response (documents, embeddings, metadata, etc.).</param>
        /// <param name="limit">Maximum number of records to return.</param>
        /// <param name="offset">Number of records to skip before returning results.</param>
        /// <returns>A response containing the retrieved records and their data.</returns>
        Task<GetResponse> GetRecordsAsync(string database, Guid collectionId, List<string> ids = null, object where = null,
            object whereDocument = null, List<Include> include = null, int? limit = null, int? offset = null);

        /// <summary>
        /// Counts the total number of records in a collection.
        /// </summary>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="readLevel">Optional read consistency level (index_and_wal or index_only).</param>
        /// <returns>The total number of records in the collection.</returns>
        Task<int> CountRecordsAsync(string database, Guid collectionId, ReadLevel? readLevel = null);

        /// <summary>
        /// Performs similarity search queries on a collection.
        /// </summary>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="queryEmbeddings">List of query vectors to search for similar records.</param>
        /// <param name="nResults">Number of results to return per query.</param>
        /// <param name="where">Optional metadata filter condition.</param>
        /// <param name="whereDocument">Optional document content filter condition.</param>
        /// <param name="include">Optional list of fields to include in the response.</param>
        /// <param name="ids">Optional list of IDs to filter by.</param>
        /// <param name="limit">Maximum number of results to return overall.</param>
        /// <param name="offset">Number of results to skip before returning.</param>
        /// <returns>A response containing the query results with distances and associated data.</returns>
        Task<QueryResponse> QueryRecordsAsync(string database, Guid collectionId, List<List<float>> queryEmbeddings,
            int? nResults = null, object where = null, object whereDocument = null, List<Include> include = null,
            List<string> ids = null, int? limit = null, int? offset = null);

        /// <summary>
        /// Performs an advanced search with grouping, ranking, and selection capabilities.
        /// </summary>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="searches">List of search configurations defining filters, groups, and ranking.</param>
        /// <param name="readLevel">Optional read consistency level.</param>
        /// <returns>A response containing the search results with scores and grouped data.</returns>
        Task<SearchResponse> SearchRecordsAsync(string database, Guid collectionId, List<SearchPayload> searches,
            ReadLevel? readLevel = null);

        /// <summary>
        /// Gets the indexing status and progress for a collection.
        /// </summary>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <returns>A response containing indexing progress information including indexed/unindexed operation counts.</returns>
        Task<IndexStatusResponse> GetIndexingStatusAsync(Guid collectionId, string database);

        /// <summary>
        /// Attaches a function to a collection for automated processing.
        /// </summary>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="functionId">The GUID of the function to attach.</param>
        /// <param name="name">The name for the attached function instance.</param>
        /// <param name="outputCollection">The name of the collection where function results will be stored.</param>
        /// <param name="params">Optional parameters for the function configuration.</param>
        /// <returns>A response containing the attached function details.</returns>
        Task<AttachFunctionResponse> AttachFunctionAsync(string database, Guid collectionId, Guid functionId, string name,
            string outputCollection, object @params = null);

        /// <summary>
        /// Retrieves detailed information about an attached function.
        /// </summary>
        /// <param name="collectionId">The GUID of the collection that has the attached function.</param>
        /// <param name="functionName">The name of the attached function.</param>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <returns>A response containing the attached function configuration and metadata.</returns>
        Task<GetAttachedFunctionResponse> GetAttachedFunctionAsync(Guid collectionId, string functionName, string database);

        /// <summary>
        /// Detaches a function from a collection, optionally deleting its output collection.
        /// </summary>
        /// <param name="database">The name of the database containing the collection.</param>
        /// <param name="collectionId">The GUID of the target collection.</param>
        /// <param name="name">The name of the attached function to detach.</param>
        /// <param name="deleteOutput">If true, deletes the associated output collection.</param>
        /// <returns>A response indicating whether the detachment was successful.</returns>
        Task<DetachFunctionResponse> DetachFunctionAsync(string database, Guid collectionId, string name, bool deleteOutput = false);
    }
}