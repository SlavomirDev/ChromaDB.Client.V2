using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System;
using System.Collections.Generic;

namespace ChromaDB.Client.V2
{
    /// <summary>
    /// Specifies which fields to include in API responses.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Include : uint
    {
        /// <summary>Include distance scores.</summary>
        distances = 0,
        /// <summary>Include document text.</summary>
        documents = 1,
        /// <summary>Include embeddings vectors.</summary>
        embeddings = 2,
        /// <summary>Include metadata.</summary>
        metadatas = 3,
        /// <summary>Include URIs.</summary>
        uris = 4
    }

    /// <summary>
    /// Specifies the read consistency level for operations.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ReadLevel : uint
    {
        /// <summary>Read from both index and write-ahead log.</summary>
        index_and_wal = 0,
        /// <summary>Read from index only.</summary>
        index_only = 1
    }

    /// <summary>
    /// Specifies the distance metric space for vector similarity.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Space : uint
    {
        /// <summary>Euclidean distance (L2).</summary>
        L2 = 0,
        /// <summary>Cosine similarity.</summary>
        Cosine = 1,
        /// <summary>Inner product.</summary>
        IP = 2
    }

    /// <summary>
    /// Specifies quantization method for vector indices.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Quantization
    {
        /// <summary>No quantization.</summary>
        mone,
        /// <summary>4-bit quantization with RabitQ and U-search.</summary>
        four_bit_rabit_q_with_u_search
    }

    /// <summary>Response for create tenant operation.</summary>
    public class CreateTenantResponse { }

    /// <summary>Response for update tenant operation.</summary>
    public class UpdateTenantResponse { }

    /// <summary>Response for create database operation.</summary>
    public class CreateDatabaseResponse { }

    /// <summary>Response for delete database operation.</summary>
    public class DeleteDatabaseResponse { }

    /// <summary>Response for update collection operation.</summary>
    public class UpdateCollectionResponse { }

    /// <summary>Response for delete collection operation.</summary>
    public class DeleteCollectionResponse { }

    /// <summary>Response for add records operation.</summary>
    public class AddCollectionRecordsResponse { }

    /// <summary>Response for upsert records operation.</summary>
    public class UpsertCollectionRecordsResponse { }

    /// <summary>Response for update records operation.</summary>
    public class UpdateCollectionRecordsResponse { }

    /// <summary>Represents an error response from the ChromaDB API.</summary>
    public class ErrorResponse
    {
        /// <summary>Gets or sets the error code.</summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        /// <summary>Gets or sets the error message.</summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    /// <summary>Response containing user identity information.</summary>
    public class GetUserIdentityResponse
    {
        /// <summary>Gets or sets the user ID.</summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>Gets or sets the tenant name.</summary>
        [JsonProperty("tenant")]
        public string Tenant { get; set; }

        /// <summary>Gets or sets the list of accessible databases.</summary>
        [JsonProperty("databases")]
        public List<string> Databases { get; set; }
    }

    /// <summary>Response containing server heartbeat information.</summary>
    public class HeartbeatResponse
    {
        /// <summary>Gets or sets the heartbeat nanosecond timestamp.</summary>
        [JsonProperty("nanosecond heartbeat")]
        public long NanosecondHeartbeat { get; set; }
    }

    /// <summary>Response containing server capability information.</summary>
    public class ChecklistResponse
    {
        /// <summary>Gets or sets the maximum batch size supported by the server.</summary>
        [JsonProperty("max_batch_size")]
        public int MaxBatchSize { get; set; }

        /// <summary>Gets or sets whether base64 encoding is supported.</summary>
        [JsonProperty("supports_base64_encoding")]
        public bool SupportsBase64Encoding { get; set; }
    }

    /// <summary>Represents a collection in ChromaDB.</summary>
    public class Collection
    {
        /// <summary>Gets or sets the collection ID.</summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>Gets or sets the collection name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the collection configuration.</summary>
        [JsonProperty("configuration_json")]
        public CollectionConfiguration ConfigurationJson { get; set; }

        /// <summary>Gets or sets the tenant name.</summary>
        [JsonProperty("tenant")]
        public string Tenant { get; set; }

        /// <summary>Gets or sets the database name.</summary>
        [JsonProperty("database")]
        public string Database { get; set; }

        /// <summary>Gets or sets the log position.</summary>
        [JsonProperty("log_position")]
        public long LogPosition { get; set; }

        /// <summary>Gets or sets the collection version.</summary>
        [JsonProperty("version")]
        public int Version { get; set; }

        /// <summary>Gets or sets the embedding dimension.</summary>
        [JsonProperty("dimension")]
        public int? Dimension { get; set; }

        /// <summary>Gets or sets the collection metadata.</summary>
        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>Gets or sets the collection schema.</summary>
        [JsonProperty("schema")]
        public Schema Schema { get; set; }
    }

    /// <summary>Configuration for a collection.</summary>
    public class CollectionConfiguration
    {
        /// <summary>Gets or sets the embedding function configuration.</summary>
        [JsonProperty("embedding_function")]
        public EmbeddingFunctionConfiguration EmbeddingFunction { get; set; }

        /// <summary>Gets or sets the HNSW index configuration.</summary>
        [JsonProperty("hnsw")]
        public HnswConfiguration Hnsw { get; set; }

        /// <summary>Gets or sets the SPANN index configuration.</summary>
        [JsonProperty("spann")]
        public SpannConfiguration Spann { get; set; }
    }

    /// <summary>Configuration for an embedding function.</summary>
    public class EmbeddingFunctionConfiguration
    {
        /// <summary>Gets or sets the embedding function type.</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>Gets or sets the embedding function name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the embedding function configuration.</summary>
        [JsonProperty("config")]
        public object Config { get; set; }
    }

    /// <summary>Configuration for HNSW index.</summary>
    public class HnswConfiguration
    {
        /// <summary>Gets or sets the ef_construction parameter.</summary>
        [JsonProperty("ef_construction")]
        public int? EfConstruction { get; set; }

        /// <summary>Gets or sets the ef_search parameter.</summary>
        [JsonProperty("ef_search")]
        public int? EfSearch { get; set; }

        /// <summary>Gets or sets the max_neighbors parameter.</summary>
        [JsonProperty("max_neighbors")]
        public int? MaxNeighbors { get; set; }

        /// <summary>Gets or sets the resize_factor parameter.</summary>
        [JsonProperty("resize_factor")]
        public double? ResizeFactor { get; set; }

        /// <summary>Gets or sets the distance space.</summary>
        [JsonProperty("space")]
        public Space? Space { get; set; }

        /// <summary>Gets or sets the sync_threshold parameter.</summary>
        [JsonProperty("sync_threshold")]
        public int? SyncThreshold { get; set; }
    }

    /// <summary>Configuration for SPANN index.</summary>
    public class SpannConfiguration
    {
        /// <summary>Gets or sets the ef_construction parameter.</summary>
        [JsonProperty("ef_construction")]
        public int? EfConstruction { get; set; }

        /// <summary>Gets or sets the ef_search parameter.</summary>
        [JsonProperty("ef_search")]
        public int? EfSearch { get; set; }

        /// <summary>Gets or sets the max_neighbors parameter.</summary>
        [JsonProperty("max_neighbors")]
        public int? MaxNeighbors { get; set; }

        /// <summary>Gets or sets the merge_threshold parameter.</summary>
        [JsonProperty("merge_threshold")]
        public int? MergeThreshold { get; set; }

        /// <summary>Gets or sets the reassign_neighbor_count parameter.</summary>
        [JsonProperty("reassign_neighbor_count")]
        public int? ReassignNeighborCount { get; set; }

        /// <summary>Gets or sets the search_nprobe parameter.</summary>
        [JsonProperty("search_nprobe")]
        public int? SearchNprobe { get; set; }

        /// <summary>Gets or sets the distance space.</summary>
        [JsonProperty("space")]
        public Space? Space { get; set; }

        /// <summary>Gets or sets the split_threshold parameter.</summary>
        [JsonProperty("split_threshold")]
        public int? SplitThreshold { get; set; }

        /// <summary>Gets or sets the write_nprobe parameter.</summary>
        [JsonProperty("write_nprobe")]
        public int? WriteNprobe { get; set; }
    }

    /// <summary>Schema definition for a collection.</summary>
    public class Schema
    {
        /// <summary>Gets or sets the default value types.</summary>
        [JsonProperty("defaults")]
        public ValueTypes Defaults { get; set; }

        /// <summary>Gets or sets the key-specific value types.</summary>
        [JsonProperty("keys")]
        public Dictionary<string, ValueTypes> Keys { get; set; }

        /// <summary>Gets or sets the CMEK configuration.</summary>
        [JsonProperty("cmek")]
        public object Cmek { get; set; }

        /// <summary>Gets or sets the source attached function ID.</summary>
        [JsonProperty("source_attached_function_id")]
        public string SourceAttachedFunctionId { get; set; }
    }

    /// <summary>Value types for schema fields.</summary>
    public class ValueTypes
    {
        /// <summary>Gets or sets the boolean value type.</summary>
        [JsonProperty("bool")]
        public BoolValueType Bool { get; set; }

        /// <summary>Gets or sets the float value type.</summary>
        [JsonProperty("float")]
        public FloatValueType Float { get; set; }

        /// <summary>Gets or sets the float list value type.</summary>
        [JsonProperty("float_list")]
        public FloatListValueType FloatList { get; set; }

        /// <summary>Gets or sets the integer value type.</summary>
        [JsonProperty("int")]
        public IntValueType Int { get; set; }

        /// <summary>Gets or sets the sparse vector value type.</summary>
        [JsonProperty("sparse_vector")]
        public SparseVectorValueType SparseVector { get; set; }

        /// <summary>Gets or sets the string value type.</summary>
        [JsonProperty("string")]
        public StringValueType String { get; set; }
    }

    /// <summary>Boolean value type configuration.</summary>
    public class BoolValueType
    {
        /// <summary>Gets or sets the boolean inverted index configuration.</summary>
        [JsonProperty("bool_inverted_index")]
        public BoolInvertedIndexType BoolInvertedIndex { get; set; }
    }

    /// <summary>Boolean inverted index configuration.</summary>
    public class BoolInvertedIndexType
    {
        /// <summary>Gets or sets whether the index is enabled.</summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        /// <summary>Gets or sets the index configuration.</summary>
        [JsonProperty("config")]
        public BoolInvertedIndexConfig Config { get; set; }
    }

    /// <summary>Boolean inverted index config.</summary>
    public class BoolInvertedIndexConfig { }

    /// <summary>Float value type configuration.</summary>
    public class FloatValueType
    {
        /// <summary>Gets or sets the float inverted index configuration.</summary>
        [JsonProperty("float_inverted_index")]
        public FloatInvertedIndexType FloatInvertedIndex { get; set; }
    }

    /// <summary>Float inverted index configuration.</summary>
    public class FloatInvertedIndexType
    {
        /// <summary>Gets or sets whether the index is enabled.</summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        /// <summary>Gets or sets the index configuration.</summary>
        [JsonProperty("config")]
        public FloatInvertedIndexConfig Config { get; set; }
    }

    /// <summary>Float inverted index config.</summary>
    public class FloatInvertedIndexConfig { }

    /// <summary>Float list value type configuration.</summary>
    public class FloatListValueType
    {
        /// <summary>Gets or sets the vector index configuration.</summary>
        [JsonProperty("vector_index")]
        public VectorIndexType VectorIndex { get; set; }
    }

    /// <summary>Vector index configuration.</summary>
    public class VectorIndexType
    {
        /// <summary>Gets or sets whether the index is enabled.</summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        /// <summary>Gets or sets the index configuration.</summary>
        [JsonProperty("config")]
        public VectorIndexConfig Config { get; set; }
    }

    /// <summary>Vector index configuration details.</summary>
    public class VectorIndexConfig
    {
        /// <summary>Gets or sets the embedding function configuration.</summary>
        [JsonProperty("embedding_function")]
        public EmbeddingFunctionConfiguration EmbeddingFunction { get; set; }

        /// <summary>Gets or sets the HNSW index configuration.</summary>
        [JsonProperty("hnsw")]
        public HnswIndexConfig Hnsw { get; set; }

        /// <summary>Gets or sets the source key.</summary>
        [JsonProperty("source_key")]
        public string SourceKey { get; set; }

        /// <summary>Gets or sets the distance space.</summary>
        [JsonProperty("space")]
        public Space? Space { get; set; }

        /// <summary>Gets or sets the SPANN index configuration.</summary>
        [JsonProperty("spann")]
        public SpannIndexConfig Spann { get; set; }
    }

    /// <summary>HNSW index configuration details.</summary>
    public class HnswIndexConfig
    {
        /// <summary>Gets or sets the batch size.</summary>
        [JsonProperty("batch_size")]
        public int? BatchSize { get; set; }

        /// <summary>Gets or sets the ef_construction parameter.</summary>
        [JsonProperty("ef_construction")]
        public int? EfConstruction { get; set; }

        /// <summary>Gets or sets the ef_search parameter.</summary>
        [JsonProperty("ef_search")]
        public int? EfSearch { get; set; }

        /// <summary>Gets or sets the max_neighbors parameter.</summary>
        [JsonProperty("max_neighbors")]
        public int? MaxNeighbors { get; set; }

        /// <summary>Gets or sets the number of threads.</summary>
        [JsonProperty("num_threads")]
        public int? NumThreads { get; set; }

        /// <summary>Gets or sets the resize factor.</summary>
        [JsonProperty("resize_factor")]
        public double? ResizeFactor { get; set; }

        /// <summary>Gets or sets the sync threshold.</summary>
        [JsonProperty("sync_threshold")]
        public int? SyncThreshold { get; set; }
    }

    /// <summary>SPANN index configuration details.</summary>
    public class SpannIndexConfig
    {
        /// <summary>Gets or sets the center drift threshold.</summary>
        [JsonProperty("center_drift_threshold")]
        public float? CenterDriftThreshold { get; set; }

        /// <summary>Gets or sets the ef_construction parameter.</summary>
        [JsonProperty("ef_construction")]
        public int? EfConstruction { get; set; }

        /// <summary>Gets or sets the ef_search parameter.</summary>
        [JsonProperty("ef_search")]
        public int? EfSearch { get; set; }

        /// <summary>Gets or sets the initial lambda.</summary>
        [JsonProperty("initial_lambda")]
        public float? InitialLambda { get; set; }

        /// <summary>Gets or sets the max_neighbors parameter.</summary>
        [JsonProperty("max_neighbors")]
        public int? MaxNeighbors { get; set; }

        /// <summary>Gets or sets the merge threshold.</summary>
        [JsonProperty("merge_threshold")]
        public int? MergeThreshold { get; set; }

        /// <summary>Gets or sets the number of replicas.</summary>
        [JsonProperty("nreplica_count")]
        public int? NreplicaCount { get; set; }

        /// <summary>Gets or sets the number of centers to merge to.</summary>
        [JsonProperty("num_centers_to_merge_to")]
        public int? NumCentersToMergeTo { get; set; }

        /// <summary>Gets or sets the number of KMeans samples.</summary>
        [JsonProperty("num_samples_kmeans")]
        public int? NumSamplesKmeans { get; set; }

        /// <summary>Gets or sets the quantization method.</summary>
        [JsonProperty("quantize")]
        public Quantization? Quantize { get; set; }

        /// <summary>Gets or sets the reassign neighbor count.</summary>
        [JsonProperty("reassign_neighbor_count")]
        public int? ReassignNeighborCount { get; set; }

        /// <summary>Gets or sets the search nprobe.</summary>
        [JsonProperty("search_nprobe")]
        public int? SearchNprobe { get; set; }

        /// <summary>Gets or sets the search RNG epsilon.</summary>
        [JsonProperty("search_rng_epsilon")]
        public float? SearchRngEpsilon { get; set; }

        /// <summary>Gets or sets the search RNG factor.</summary>
        [JsonProperty("search_rng_factor")]
        public float? SearchRngFactor { get; set; }

        /// <summary>Gets or sets the split threshold.</summary>
        [JsonProperty("split_threshold")]
        public int? SplitThreshold { get; set; }

        /// <summary>Gets or sets the write nprobe.</summary>
        [JsonProperty("write_nprobe")]
        public int? WriteNprobe { get; set; }

        /// <summary>Gets or sets the write RNG epsilon.</summary>
        [JsonProperty("write_rng_epsilon")]
        public float? WriteRngEpsilon { get; set; }

        /// <summary>Gets or sets the write RNG factor.</summary>
        [JsonProperty("write_rng_factor")]
        public float? WriteRngFactor { get; set; }
    }

    /// <summary>Integer value type configuration.</summary>
    public class IntValueType
    {
        /// <summary>Gets or sets the integer inverted index configuration.</summary>
        [JsonProperty("int_inverted_index")]
        public IntInvertedIndexType IntInvertedIndex { get; set; }
    }

    /// <summary>Integer inverted index configuration.</summary>
    public class IntInvertedIndexType
    {
        /// <summary>Gets or sets whether the index is enabled.</summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        /// <summary>Gets or sets the index configuration.</summary>
        [JsonProperty("config")]
        public IntInvertedIndexConfig Config { get; set; }
    }

    /// <summary>Integer inverted index config.</summary>
    public class IntInvertedIndexConfig { }

    /// <summary>Sparse vector value type configuration.</summary>
    public class SparseVectorValueType
    {
        /// <summary>Gets or sets the sparse vector index configuration.</summary>
        [JsonProperty("sparse_vector_index")]
        public SparseVectorIndexType SparseVectorIndex { get; set; }
    }

    /// <summary>Sparse vector index configuration.</summary>
    public class SparseVectorIndexType
    {
        /// <summary>Gets or sets whether the index is enabled.</summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        /// <summary>Gets or sets the index configuration.</summary>
        [JsonProperty("config")]
        public SparseVectorIndexConfig Config { get; set; }
    }

    /// <summary>Sparse vector index configuration details.</summary>
    public class SparseVectorIndexConfig
    {
        /// <summary>Gets or sets whether BM25 is enabled.</summary>
        [JsonProperty("bm25")]
        public bool? Bm25 { get; set; }

        /// <summary>Gets or sets the embedding function configuration.</summary>
        [JsonProperty("embedding_function")]
        public EmbeddingFunctionConfiguration EmbeddingFunction { get; set; }

        /// <summary>Gets or sets the source key.</summary>
        [JsonProperty("source_key")]
        public string SourceKey { get; set; }
    }

    /// <summary>String value type configuration.</summary>
    public class StringValueType
    {
        /// <summary>Gets or sets the full-text search index configuration.</summary>
        [JsonProperty("fts_index")]
        public FtsIndexType FtsIndex { get; set; }

        /// <summary>Gets or sets the string inverted index configuration.</summary>
        [JsonProperty("string_inverted_index")]
        public StringInvertedIndexType StringInvertedIndex { get; set; }
    }

    /// <summary>Full-text search index configuration.</summary>
    public class FtsIndexType
    {
        /// <summary>Gets or sets whether the index is enabled.</summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        /// <summary>Gets or sets the index configuration.</summary>
        [JsonProperty("config")]
        public FtsIndexConfig Config { get; set; }
    }

    /// <summary>Full-text search index config.</summary>
    public class FtsIndexConfig { }

    /// <summary>String inverted index configuration.</summary>
    public class StringInvertedIndexType
    {
        /// <summary>Gets or sets whether the index is enabled.</summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        /// <summary>Gets or sets the index configuration.</summary>
        [JsonProperty("config")]
        public StringInvertedIndexConfig Config { get; set; }
    }

    /// <summary>String inverted index config.</summary>
    public class StringInvertedIndexConfig { }

    /// <summary>Represents a sparse vector.</summary>
    public class SparseVector
    {
        /// <summary>Gets or sets the indices.</summary>
        [JsonProperty("indices")]
        public List<int> Indices { get; set; }

        /// <summary>Gets or sets the values.</summary>
        [JsonProperty("values")]
        public List<float> Values { get; set; }

        /// <summary>Gets or sets the tokens.</summary>
        [JsonProperty("tokens")]
        public List<string> Tokens { get; set; }
    }

    /// <summary>Represents a database.</summary>
    public class Database
    {
        /// <summary>Gets or sets the database ID.</summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>Gets or sets the database name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the tenant name.</summary>
        [JsonProperty("tenant")]
        public string Tenant { get; set; }
    }

    /// <summary>Response containing tenant information.</summary>
    public class GetTenantResponse
    {
        /// <summary>Gets or sets the tenant name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the resource name.</summary>
        [JsonProperty("resource_name")]
        public string ResourceName { get; set; }
    }

    /// <summary>Payload for creating a tenant.</summary>
    public class CreateTenantPayload
    {
        /// <summary>Gets or sets the tenant name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>Payload for creating a database.</summary>
    public class CreateDatabasePayload
    {
        /// <summary>Gets or sets the database name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>Payload for updating a tenant.</summary>
    public class UpdateTenantPayload
    {
        /// <summary>Gets or sets the resource name.</summary>
        [JsonProperty("resource_name")]
        public string ResourceName { get; set; }
    }

    /// <summary>Payload for creating a collection.</summary>
    public class CreateCollectionPayload
    {
        /// <summary>Gets or sets the collection name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the collection configuration.</summary>
        [JsonProperty("configuration")]
        public CollectionConfiguration Configuration { get; set; }

        /// <summary>Gets or sets whether to return existing collection if it exists.</summary>
        [JsonProperty("get_or_create")]
        public bool GetOrCreate { get; set; }

        /// <summary>Gets or sets the collection metadata.</summary>
        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>Gets or sets the collection schema.</summary>
        [JsonProperty("schema")]
        public Schema Schema { get; set; }
    }

    /// <summary>Payload for updating a collection.</summary>
    public class UpdateCollectionPayload
    {
        /// <summary>Gets or sets the new collection name.</summary>
        [JsonProperty("new_name")]
        public string NewName { get; set; }

        /// <summary>Gets or sets the new metadata.</summary>
        [JsonProperty("new_metadata")]
        public Dictionary<string, object> NewMetadata { get; set; }

        /// <summary>Gets or sets the new configuration.</summary>
        [JsonProperty("new_configuration")]
        public UpdateCollectionConfiguration NewConfiguration { get; set; }
    }

    /// <summary>Configuration for updating a collection.</summary>
    public class UpdateCollectionConfiguration
    {
        /// <summary>Gets or sets the embedding function configuration.</summary>
        [JsonProperty("embedding_function")]
        public EmbeddingFunctionConfiguration EmbeddingFunction { get; set; }

        /// <summary>Gets or sets the HNSW configuration updates.</summary>
        [JsonProperty("hnsw")]
        public UpdateHnswConfiguration Hnsw { get; set; }

        /// <summary>Gets or sets the SPANN configuration updates.</summary>
        [JsonProperty("spann")]
        public UpdateSpannConfiguration Spann { get; set; }
    }

    /// <summary>HNSW configuration updates.</summary>
    public class UpdateHnswConfiguration
    {
        /// <summary>Gets or sets the batch size.</summary>
        [JsonProperty("batch_size")]
        public int? BatchSize { get; set; }

        /// <summary>Gets or sets the ef_search parameter.</summary>
        [JsonProperty("ef_search")]
        public int? EfSearch { get; set; }

        /// <summary>Gets or sets the max_neighbors parameter.</summary>
        [JsonProperty("max_neighbors")]
        public int? MaxNeighbors { get; set; }

        /// <summary>Gets or sets the number of threads.</summary>
        [JsonProperty("num_threads")]
        public int? NumThreads { get; set; }

        /// <summary>Gets or sets the resize factor.</summary>
        [JsonProperty("resize_factor")]
        public double? ResizeFactor { get; set; }

        /// <summary>Gets or sets the sync threshold.</summary>
        [JsonProperty("sync_threshold")]
        public int? SyncThreshold { get; set; }
    }

    /// <summary>SPANN configuration updates.</summary>
    public class UpdateSpannConfiguration
    {
        /// <summary>Gets or sets the ef_search parameter.</summary>
        [JsonProperty("ef_search")]
        public int? EfSearch { get; set; }

        /// <summary>Gets or sets the search nprobe.</summary>
        [JsonProperty("search_nprobe")]
        public int? SearchNprobe { get; set; }
    }

    /// <summary>Payload for adding records to a collection.</summary>
    public class AddCollectionRecordsPayload
    {
        /// <summary>Gets or sets the record IDs.</summary>
        [JsonProperty("ids")]
        public List<string> Ids { get; set; }

        /// <summary>Gets or sets the embeddings.</summary>
        [JsonProperty("embeddings")]
        public object Embeddings { get; set; }

        /// <summary>Gets or sets the documents.</summary>
        [JsonProperty("documents")]
        public List<string> Documents { get; set; }

        /// <summary>Gets or sets the metadata.</summary>
        [JsonProperty("metadatas")]
        public List<Dictionary<string, object>> Metadatas { get; set; }

        /// <summary>Gets or sets the URIs.</summary>
        [JsonProperty("uris")]
        public List<string> Uris { get; set; }
    }

    /// <summary>Payload for upserting records to a collection.</summary>
    public class UpsertCollectionRecordsPayload
    {
        /// <summary>Gets or sets the record IDs.</summary>
        [JsonProperty("ids")]
        public List<string> Ids { get; set; }

        /// <summary>Gets or sets the embeddings.</summary>
        [JsonProperty("embeddings")]
        public object Embeddings { get; set; }

        /// <summary>Gets or sets the documents.</summary>
        [JsonProperty("documents")]
        public List<string> Documents { get; set; }

        /// <summary>Gets or sets the metadata.</summary>
        [JsonProperty("metadatas")]
        public List<Dictionary<string, object>> Metadatas { get; set; }

        /// <summary>Gets or sets the URIs.</summary>
        [JsonProperty("uris")]
        public List<string> Uris { get; set; }
    }

    /// <summary>Payload for updating records in a collection.</summary>
    public class UpdateCollectionRecordsPayload
    {
        /// <summary>Gets or sets the record IDs to update.</summary>
        [JsonProperty("ids")]
        public List<string> Ids { get; set; }

        /// <summary>Gets or sets the new embeddings.</summary>
        [JsonProperty("embeddings")]
        public object Embeddings { get; set; }

        /// <summary>Gets or sets the new documents.</summary>
        [JsonProperty("documents")]
        public List<string> Documents { get; set; }

        /// <summary>Gets or sets the new metadata.</summary>
        [JsonProperty("metadatas")]
        public List<Dictionary<string, object>> Metadatas { get; set; }

        /// <summary>Gets or sets the new URIs.</summary>
        [JsonProperty("uris")]
        public List<string> Uris { get; set; }
    }

    /// <summary>Payload for deleting records from a collection.</summary>
    public class DeleteCollectionRecordsPayload
    {
        /// <summary>Gets or sets the record IDs to delete.</summary>
        [JsonProperty("ids")]
        public List<string> Ids { get; set; }

        /// <summary>Gets or sets the where filter condition.</summary>
        [JsonProperty("where")]
        public object Where { get; set; }

        /// <summary>Gets or sets the where document filter condition.</summary>
        [JsonProperty("where_document")]
        public object WhereDocument { get; set; }

        /// <summary>Gets or sets the maximum number of records to delete.</summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
    }

    /// <summary>Payload for getting records from a collection.</summary>
    public class GetRequestPayload
    {
        /// <summary>Gets or sets the record IDs to retrieve.</summary>
        [JsonProperty("ids")]
        public List<string> Ids { get; set; }

        /// <summary>Gets or sets the where filter condition.</summary>
        [JsonProperty("where")]
        public object Where { get; set; }

        /// <summary>Gets or sets the where document filter condition.</summary>
        [JsonProperty("where_document")]
        public object WhereDocument { get; set; }

        /// <summary>Gets or sets the fields to include in the response.</summary>
        [JsonProperty("include")]
        public List<Include> Include { get; set; }

        /// <summary>Gets or sets the maximum number of records to return.</summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>Gets or sets the number of records to skip.</summary>
        [JsonProperty("offset")]
        public int? Offset { get; set; }
    }

    /// <summary>Response for get records operation.</summary>
    public class GetResponse
    {
        /// <summary>Gets or sets the record IDs.</summary>
        [JsonProperty("ids")]
        public List<string> Ids { get; set; }

        /// <summary>Gets or sets the included fields.</summary>
        [JsonProperty("include")]
        public List<Include> Include { get; set; }

        /// <summary>Gets or sets the documents.</summary>
        [JsonProperty("documents")]
        public List<string> Documents { get; set; }

        /// <summary>Gets or sets the embeddings.</summary>
        [JsonProperty("embeddings")]
        public List<List<float>> Embeddings { get; set; }

        /// <summary>Gets or sets the metadata.</summary>
        [JsonProperty("metadatas")]
        public List<Dictionary<string, object>> Metadatas { get; set; }

        /// <summary>Gets or sets the URIs.</summary>
        [JsonProperty("uris")]
        public List<string> Uris { get; set; }
    }

    /// <summary>Payload for querying records.</summary>
    public class QueryRequestPayload
    {
        /// <summary>Gets or sets the query embeddings.</summary>
        [JsonProperty("query_embeddings")]
        public List<List<float>> QueryEmbeddings { get; set; }

        /// <summary>Gets or sets the number of results to return per query.</summary>
        [JsonProperty("n_results")]
        public int? NResults { get; set; }

        /// <summary>Gets or sets the where filter condition.</summary>
        [JsonProperty("where")]
        public object Where { get; set; }

        /// <summary>Gets or sets the where document filter condition.</summary>
        [JsonProperty("where_document")]
        public object WhereDocument { get; set; }

        /// <summary>Gets or sets the fields to include in the response.</summary>
        [JsonProperty("include")]
        public List<Include> Include { get; set; }

        /// <summary>Gets or sets the IDs to filter by.</summary>
        [JsonProperty("ids")]
        public List<string> Ids { get; set; }
    }

    /// <summary>Response for query records operation.</summary>
    public class QueryResponse
    {
        /// <summary>Gets or sets the record IDs grouped by query.</summary>
        [JsonProperty("ids")]
        public List<List<string>> Ids { get; set; }

        /// <summary>Gets or sets the included fields.</summary>
        [JsonProperty("include")]
        public List<Include> Include { get; set; }

        /// <summary>Gets or sets the distance scores.</summary>
        [JsonProperty("distances")]
        public List<List<float?>> Distances { get; set; }

        /// <summary>Gets or sets the documents grouped by query.</summary>
        [JsonProperty("documents")]
        public List<List<string>> Documents { get; set; }

        /// <summary>Gets or sets the embeddings grouped by query.</summary>
        [JsonProperty("embeddings")]
        public List<List<List<float>>> Embeddings { get; set; }

        /// <summary>Gets or sets the metadata grouped by query.</summary>
        [JsonProperty("metadatas")]
        public List<List<Dictionary<string, object>>> Metadatas { get; set; }

        /// <summary>Gets or sets the URIs grouped by query.</summary>
        [JsonProperty("uris")]
        public List<List<string>> Uris { get; set; }
    }

    /// <summary>Payload for search request.</summary>
    public class SearchRequestPayload
    {
        /// <summary>Gets or sets the list of search configurations.</summary>
        [JsonProperty("searches")]
        public List<SearchPayload> Searches { get; set; }

        /// <summary>Gets or sets the read consistency level.</summary>
        [JsonProperty("read_level")]
        public ReadLevel? ReadLevel { get; set; }
    }

    /// <summary>Search configuration payload.</summary>
    public class SearchPayload
    {
        /// <summary>Gets or sets the search filter.</summary>
        [JsonProperty("filter")]
        public SearchFilter Filter { get; set; }

        /// <summary>Gets or sets the group by configuration.</summary>
        [JsonProperty("group_by")]
        public SearchGroupBy GroupBy { get; set; }

        /// <summary>Gets or sets the limit configuration.</summary>
        [JsonProperty("limit")]
        public SearchLimit Limit { get; set; }

        /// <summary>Gets or sets the rank configuration.</summary>
        [JsonProperty("rank")]
        public object Rank { get; set; }

        /// <summary>Gets or sets the select configuration.</summary>
        [JsonProperty("select")]
        public SearchSelect Select { get; set; }
    }

    /// <summary>Search filter configuration.</summary>
    public class SearchFilter
    {
        /// <summary>Gets or sets the query IDs.</summary>
        [JsonProperty("query_ids")]
        public List<string> QueryIds { get; set; }

        /// <summary>Gets or sets the where clause.</summary>
        [JsonProperty("where_clause")]
        public object WhereClause { get; set; }
    }

    /// <summary>Group by configuration for search.</summary>
    public class SearchGroupBy
    {
        /// <summary>Gets or sets the aggregate configuration.</summary>
        [JsonProperty("aggregate")]
        public object Aggregate { get; set; }

        /// <summary>Gets or sets the group by keys.</summary>
        [JsonProperty("keys")]
        public List<string> Keys { get; set; }
    }

    /// <summary>Limit configuration for search.</summary>
    public class SearchLimit
    {
        /// <summary>Gets or sets the maximum number of results.</summary>
        [JsonProperty("limit")]
        public int Limit { get; set; }

        /// <summary>Gets or sets the offset.</summary>
        [JsonProperty("offset")]
        public int Offset { get; set; }
    }

    /// <summary>Select configuration for search.</summary>
    public class SearchSelect
    {
        /// <summary>Gets or sets the keys to select.</summary>
        [JsonProperty("keys")]
        public List<string> Keys { get; set; }
    }

    /// <summary>Response for search operation.</summary>
    public class SearchResponse
    {
        /// <summary>Gets or sets the record IDs grouped by search.</summary>
        [JsonProperty("ids")]
        public List<List<string>> Ids { get; set; }

        /// <summary>Gets or sets the documents grouped by search.</summary>
        [JsonProperty("documents")]
        public List<List<string>> Documents { get; set; }

        /// <summary>Gets or sets the embeddings grouped by search.</summary>
        [JsonProperty("embeddings")]
        public List<List<List<float>>> Embeddings { get; set; }

        /// <summary>Gets or sets the metadata grouped by search.</summary>
        [JsonProperty("metadatas")]
        public List<List<Dictionary<string, object>>> Metadatas { get; set; }

        /// <summary>Gets or sets the scores grouped by search.</summary>
        [JsonProperty("scores")]
        public List<List<float?>> Scores { get; set; }
    }

    /// <summary>Payload for forking a collection.</summary>
    public class ForkCollectionPayload
    {
        /// <summary>Gets or sets the new collection name.</summary>
        [JsonProperty("new_name")]
        public string NewName { get; set; }
    }

    /// <summary>Response containing indexing status.</summary>
    public class IndexStatusResponse
    {
        /// <summary>Gets or sets the indexing progress (0-1).</summary>
        [JsonProperty("op_indexing_progress")]
        public float OpIndexingProgress { get; set; }

        /// <summary>Gets or sets the number of unindexed operations.</summary>
        [JsonProperty("num_unindexed_ops")]
        public long NumUnindexedOps { get; set; }

        /// <summary>Gets or sets the number of indexed operations.</summary>
        [JsonProperty("num_indexed_ops")]
        public long NumIndexedOps { get; set; }

        /// <summary>Gets or sets the total number of operations.</summary>
        [JsonProperty("total_ops")]
        public long TotalOps { get; set; }
    }

    /// <summary>Request payload for attaching a function.</summary>
    public class AttachFunctionRequest
    {
        /// <summary>Gets or sets the function ID.</summary>
        [JsonProperty("function_id")]
        public Guid FunctionId { get; set; }

        /// <summary>Gets or sets the function name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the output collection name.</summary>
        [JsonProperty("output_collection")]
        public string OutputCollection { get; set; }

        /// <summary>Gets or sets the function parameters.</summary>
        [JsonProperty("params")]
        public object Params { get; set; }
    }

    /// <summary>Response for attach function operation.</summary>
    public class AttachFunctionResponse
    {
        /// <summary>Gets or sets the attached function information.</summary>
        [JsonProperty("attached_function")]
        public AttachedFunctionInfo AttachedFunction { get; set; }

        /// <summary>Gets or sets whether a new function was created.</summary>
        [JsonProperty("created")]
        public bool Created { get; set; }
    }

    /// <summary>Information about an attached function.</summary>
    public class AttachedFunctionInfo
    {
        /// <summary>Gets or sets the function ID.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>Gets or sets the function name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the function name.</summary>
        [JsonProperty("function_name")]
        public string FunctionName { get; set; }
    }

    /// <summary>Response for get attached function operation.</summary>
    public class GetAttachedFunctionResponse
    {
        /// <summary>Gets or sets the attached function details.</summary>
        [JsonProperty("attached_function")]
        public AttachedFunctionApiResponse AttachedFunction { get; set; }
    }

    /// <summary>Detailed attached function information.</summary>
    public class AttachedFunctionApiResponse
    {
        /// <summary>Gets or sets the function ID.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>Gets or sets the function name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the function name.</summary>
        [JsonProperty("function_name")]
        public string FunctionName { get; set; }

        /// <summary>Gets or sets the input collection ID.</summary>
        [JsonProperty("input_collection_id")]
        public Guid InputCollectionId { get; set; }

        /// <summary>Gets or sets the output collection name.</summary>
        [JsonProperty("output_collection")]
        public string OutputCollection { get; set; }

        /// <summary>Gets or sets the tenant ID.</summary>
        [JsonProperty("tenant_id")]
        public string TenantId { get; set; }

        /// <summary>Gets or sets the database ID.</summary>
        [JsonProperty("database_id")]
        public string DatabaseId { get; set; }

        /// <summary>Gets or sets the completion offset.</summary>
        [JsonProperty("completion_offset")]
        public long CompletionOffset { get; set; }

        /// <summary>Gets or sets the minimum records required for invocation.</summary>
        [JsonProperty("min_records_for_invocation")]
        public long MinRecordsForInvocation { get; set; }

        /// <summary>Gets or sets the output collection ID.</summary>
        [JsonProperty("output_collection_id")]
        public Guid? OutputCollectionId { get; set; }

        /// <summary>Gets or sets the function parameters as JSON string.</summary>
        [JsonProperty("params")]
        public string Params { get; set; }
    }

    /// <summary>Request payload for detaching a function.</summary>
    public class DetachFunctionRequest
    {
        /// <summary>Gets or sets whether to delete the output collection.</summary>
        [JsonProperty("delete_output")]
        public bool DeleteOutput { get; set; }
    }

    /// <summary>Response for detach function operation.</summary>
    public class DetachFunctionResponse
    {
        /// <summary>Gets or sets whether the operation was successful.</summary>
        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    /// <summary>Response for delete records operation.</summary>
    public class DeleteCollectionRecordsResponse
    {
        /// <summary>Gets or sets the number of deleted records.</summary>
        [JsonProperty("deleted")]
        public int Deleted { get; set; }
    }
}