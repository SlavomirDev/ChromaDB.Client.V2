using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MirDev.ChromaDB.Client.V2
{
    internal class TenantClient : ITenantClient, IDisposable
    {
        private readonly string _tenant;
        private ChromaClient _parent;
        private bool _disposed;

        public string Tenant => _tenant;

        /// <summary>
        /// Initializes a new instance of the TenantClient class for the specified tenant using the provided
        /// ChromaClient as the parent client.
        /// </summary>
        /// <param name="parent">The parent ChromaClient instance used to perform operations on behalf of the tenant.</param>
        /// <param name="tenant">The identifier of the tenant for which the client is being created. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if either parent or tenant is null.</exception>
        public TenantClient(ChromaClient parent, string tenant)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));
        }

        /// <inheritdoc/>
        public Task<List<Database>> ListDatabasesAsync(int? limit = null, int? offset = null)
        {
            ThrowIfDisposed();
            return _parent.ListDatabasesAsync(_tenant, limit, offset);
        }

        /// <inheritdoc/>
        public Task<CreateDatabaseResponse> CreateDatabaseAsync(string name)
        {
            ThrowIfDisposed();
            return _parent.CreateDatabaseAsync(name, _tenant);
        }

        /// <inheritdoc/>
        public Task<Database> GetDatabaseAsync(string database)
        {
            ThrowIfDisposed();
            return _parent.GetDatabaseAsync(database, _tenant);
        }

        /// <inheritdoc/>
        public Task<DeleteDatabaseResponse> DeleteDatabaseAsync(string database)
        {
            ThrowIfDisposed();
            return _parent.DeleteDatabaseAsync(database, _tenant);
        }

        /// <inheritdoc/>
        public Task<List<Collection>> ListCollectionsAsync(string database, int? limit = null, int? offset = null)
        {
            ThrowIfDisposed();
            return _parent.ListCollectionsAsync(database, _tenant, limit, offset);
        }

        /// <inheritdoc/>
        public Task<Collection> CreateCollectionAsync(string name, string database, Dictionary<string, object> metadata = null, CollectionConfiguration configuration = null, Schema schema = null, bool getOrCreate = false)
        {
            ThrowIfDisposed();
            return _parent.CreateCollectionAsync(name, database, _tenant, metadata, configuration, schema, getOrCreate);
        }

        /// <inheritdoc/>
        public Task<Collection> GetCollectionAsync(Guid collectionId, string database)
        {
            ThrowIfDisposed();
            return _parent.GetCollectionAsync(database, _tenant, collectionId);
        }

        /// <inheritdoc/>
        public Task<UpdateCollectionResponse> UpdateCollectionAsync(string database, Guid collectionId, string newName = null, Dictionary<string, object> newMetadata = null, UpdateCollectionConfiguration newConfiguration = null)
        {
            ThrowIfDisposed();
            return _parent.UpdateCollectionAsync(database, _tenant, collectionId, newName, newMetadata, newConfiguration);
        }

        /// <inheritdoc/>
        public Task<DeleteCollectionResponse> DeleteCollectionAsync(Guid collectionId, string database)
        {
            ThrowIfDisposed();
            return _parent.DeleteCollectionAsync(collectionId, database, _tenant);
        }

        /// <inheritdoc/>
        public Task<int> CountCollectionsAsync(string database)
        {
            ThrowIfDisposed();
            return _parent.CountCollectionsAsync(database, _tenant);
        }

        /// <inheritdoc/>
        public Task<Collection> ForkCollectionAsync(Guid collectionId, string newName, string database)
        {
            ThrowIfDisposed();
            return _parent.ForkCollectionAsync(collectionId, newName, database, _tenant);
        }

        /// <inheritdoc/>
        public Task<AddCollectionRecordsResponse> AddRecordsAsync(string database, Guid collectionId, List<string> ids, object embeddings, List<string> documents = null, List<Dictionary<string, object>> metadatas = null, List<string> uris = null)
        {
            ThrowIfDisposed();
            return _parent.AddRecordsAsync(database, _tenant, collectionId, ids, embeddings, documents, metadatas, uris);
        }
        
        /// <inheritdoc/>
        public Task<UpsertCollectionRecordsResponse> UpsertRecordsAsync(string database, Guid collectionId, List<string> ids, object embeddings, List<string> documents = null, List<Dictionary<string, object>> metadatas = null, List<string> uris = null)
        {
            ThrowIfDisposed();
            return _parent.UpsertRecordsAsync(database, _tenant, collectionId, ids, embeddings, documents, metadatas, uris);
        }

        /// <inheritdoc/>
        public Task<UpdateCollectionRecordsResponse> UpdateRecordsAsync(string database, Guid collectionId, List<string> ids, object embeddings = null, List<string> documents = null, List<Dictionary<string, object>> metadatas = null, List<string> uris = null)
        {
            ThrowIfDisposed();
            return _parent.UpdateRecordsAsync(database, _tenant, collectionId, ids, embeddings, documents, metadatas, uris);
        }

        /// <inheritdoc/>
        public Task<DeleteCollectionRecordsResponse> DeleteRecordsAsync(string database, Guid collectionId, List<string> ids = null, object where = null, object whereDocument = null, int? limit = null)
        {
            ThrowIfDisposed();
            return _parent.DeleteRecordsAsync(database, _tenant, collectionId, ids, where, whereDocument, limit);
        }

        /// <inheritdoc/>
        public Task<GetResponse> GetRecordsAsync(string database, Guid collectionId, List<string> ids = null, object where = null, object whereDocument = null, List<Include> include = null, int? limit = null, int? offset = null)
        {
            ThrowIfDisposed();
            return _parent.GetRecordsAsync(database, _tenant, collectionId, ids, where, whereDocument, include, limit, offset);
        }

        /// <inheritdoc/>
        public Task<int> CountRecordsAsync(string database, Guid collectionId, ReadLevel? readLevel = null)
        {
            ThrowIfDisposed();
            return _parent.CountRecordsAsync(database, _tenant, collectionId, readLevel);
        }

        /// <inheritdoc/>
        public Task<QueryResponse> QueryRecordsAsync(string database, Guid collectionId, List<List<float>> queryEmbeddings, int? nResults = null, object where = null, object whereDocument = null, List<Include> include = null, List<string> ids = null, int? limit = null, int? offset = null)
        {
            ThrowIfDisposed();
            return _parent.QueryRecordsAsync(database, _tenant, collectionId, queryEmbeddings, nResults, where, whereDocument, include, ids, limit, offset);
        }

        /// <inheritdoc/>
        public Task<SearchResponse> SearchRecordsAsync(string database, Guid collectionId, List<SearchPayload> searches, ReadLevel? readLevel = null)
        {
            ThrowIfDisposed();
            return _parent.SearchRecordsAsync(database, _tenant, collectionId, searches, readLevel);
        }

        /// <inheritdoc/>
        public Task<IndexStatusResponse> GetIndexingStatusAsync(Guid collectionId, string database)
        {
            ThrowIfDisposed();
            return _parent.GetIndexingStatusAsync(collectionId, database, _tenant);
        }

        /// <inheritdoc/>
        public Task<AttachFunctionResponse> AttachFunctionAsync(string database, Guid collectionId, Guid functionId, string name, string outputCollection, object @params = null)
        {
            ThrowIfDisposed();
            return _parent.AttachFunctionAsync(database, _tenant, collectionId, functionId, name, outputCollection, @params);
        }

        /// <inheritdoc/>
        public Task<GetAttachedFunctionResponse> GetAttachedFunctionAsync(Guid collectionId, string functionName, string database)
        {
            ThrowIfDisposed();
            return _parent.GetAttachedFunctionAsync(collectionId, functionName, database, _tenant);
        }

        /// <inheritdoc/>
        public Task<DetachFunctionResponse> DetachFunctionAsync(string database, Guid collectionId, string name, bool deleteOutput = false)
        {
            ThrowIfDisposed();
            return _parent.DetachFunctionAsync(database, _tenant, collectionId, name, deleteOutput);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TenantClient));
        }

        #region IDisposable
        void IDisposable.Dispose()
        {
            if (_disposed)
                return;

            _parent = null;
            _disposed = true;
        }
        #endregion
    }
}
