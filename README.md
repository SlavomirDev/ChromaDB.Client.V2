# ChromaDB .NET Client V2

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A modern, feature-complete .NET client library for [ChromaDB](https://www.trychroma.com/) V2 API. This library provides a clean, idiomatic C# interface for working with ChromaDB's vector database capabilities.

## Features

- **Full V2 API Support** - Complete implementation of ChromaDB V2 REST API
- **Tenant and Database Scoping** - Convenient client interfaces for tenant-scoped operations

## Quick Start

``` Csharp
using ChromaDB.Client.V2;

// Create a client connected to your ChromaDB instance
var client = new ChromaClient("http://localhost:8000");

// Or with authentication
var client = new ChromaClient("https://your-chromadb-instance.com", "your-api-key");
```

## Usage Examples

### Tenant and Database Management

``` Csharp
// Create a tenant
await client.CreateTenantAsync("my-tenant");

// Create a database within the tenant
await client.CreateDatabaseAsync("my-database", "my-tenant");

// List databases for a tenant
var databases = await client.ListDatabasesAsync("my-tenant");
foreach (var db in databases)
{
    Console.WriteLine($"Database: {db.Name} (ID: {db.Id})");
}

// Get specific database
var database = await client.GetDatabaseAsync("my-database", "my-tenant");
Console.WriteLine($"Database ID: {database.Id}");

// Update a tenant
await client.UpdateTenantAsync("my-tenant", "updated-resource-name");

// Delete a database (use with caution!)
await client.DeleteDatabaseAsync("my-database", "my-tenant");
```

### Working with Collections

``` Csharp
// Create a collection
var collection = await client.CreateCollectionAsync(
    name: "my-collection",
    database: "my-database",
    tenant: "my-tenant",
    metadata: new Dictionary<string, object>
    {
        ["description"] = "My test collection",
        ["created_by"] = "ChromaDB .NET Client"
    }
);

Console.WriteLine($"Collection created: {collection.Name} (ID: {collection.Id})");

// List all collections
var collections = await client.ListCollectionsAsync("my-database", "my-tenant");
foreach (var col in collections)
{
    Console.WriteLine($"Collection: {col.Name}, Dimension: {col.Dimension}");
}

// Get collection by ID
var existingCollection = await client.GetCollectionAsync("my-database", "my-tenant", collection.Id);

// Get collection by CRN (Chroma Resource Name)
var collectionByCrn = await client.GetCollectionByCrnAsync(collection.Id.ToString());

// Update collection
await client.UpdateCollectionAsync(
    database: "my-database",
    tenant: "my-tenant",
    collectionId: collection.Id,
    newName: "renamed-collection",
    newMetadata: new Dictionary<string, object> { ["status"] = "updated" }
);

// Count collections
var collectionCount = await client.CountCollectionsAsync("my-database", "my-tenant");
Console.WriteLine($"Total collections: {collectionCount}");

// Fork a collection (create a copy)
var forkedCollection = await client.ForkCollectionAsync(
    collectionId: collection.Id,
    newName: "forked-collection",
    database: "my-database",
    tenant: "my-tenant"
);

// Delete collection
await client.DeleteCollectionAsync(collection.Id, "my-database", "my-tenant");
```

### Adding Records

``` Csharp
var collectionId = Guid.Parse("your-collection-id");

// Prepare your data
var ids = new List<string> { "doc1", "doc2", "doc3" };
var embeddings = new List<List<float>>
{
    new List<float> { 0.1f, 0.2f, 0.3f },
    new List<float> { 0.4f, 0.5f, 0.6f },
    new List<float> { 0.7f, 0.8f, 0.9f }
};
var documents = new List<string>
{
    "This is the first document about machine learning",
    "This is the second document about artificial intelligence",
    "This is the third document about deep learning"
};
var metadatas = new List<Dictionary<string, object>>
{
    new Dictionary<string, object> { ["source"] = "file1.txt", ["page"] = 1, ["category"] = "ML" },
    new Dictionary<string, object> { ["source"] = "file2.txt", ["page"] = 2, ["category"] = "AI" },
    new Dictionary<string, object> { ["source"] = "file3.txt", ["page"] = 3, ["category"] = "DL" }
};

// Add records to collection
await client.AddRecordsAsync(
    database: "my-database",
    tenant: "my-tenant",
    collectionId: collectionId,
    ids: ids,
    embeddings: embeddings,
    documents: documents,
    metadatas: metadatas
);

// Upsert records (update if exists, insert if new)
await client.UpsertRecordsAsync(
    database: "my-database",
    tenant: "my-tenant",
    collectionId: collectionId,
    ids: ids,
    embeddings: embeddings,
    documents: documents,
    metadatas: metadatas
);

// Update existing records
await client.UpdateRecordsAsync(
    database: "my-database",
    tenant: "my-tenant",
    collectionId: collectionId,
    ids: new List<string> { "doc1", "doc2" },
    documents: new List<string> { "Updated: Machine learning basics", "Updated: AI fundamentals" },
    metadatas: new List<Dictionary<string, object>>
    {
        new Dictionary<string, object> { ["updated"] = true, ["version"] = 2 },
        new Dictionary<string, object> { ["updated"] = true, ["version"] = 2 }
    }
);
```

### Querying Records

``` Csharp
// Simple similarity search
var queryEmbeddings = new List<List<float>>
{
    new List<float> { 0.15f, 0.25f, 0.35f } // Query vector
};

var results = await client.QueryRecordsAsync(
    database: "my-database",
    tenant: "my-tenant",
    collectionId: collectionId,
    queryEmbeddings: queryEmbeddings,
    nResults: 5,
    include: new List<Include> 
    { 
        Include.documents, 
        Include.metadatas, 
        Include.distances 
    }
);

// Process results
for (int i = 0; i < results.Ids.Count; i++)
{
    Console.WriteLine($"Query {i + 1} results:");
    for (int j = 0; j < results.Ids[i].Count; j++)
    {
        Console.WriteLine($"  ID: {results.Ids[i][j]}");
        Console.WriteLine($"  Distance: {results.Distances[i][j]}");
        Console.WriteLine($"  Document: {results.Documents[i][j]}");
        Console.WriteLine($"  Metadata: {JsonConvert.SerializeObject(results.Metadatas[i][j])}");
    }
}

// Query with metadata filtering
var filteredResults = await client.QueryRecordsAsync(
    database: "my-database",
    tenant: "my-tenant",
    collectionId: collectionId,
    queryEmbeddings: queryEmbeddings,
    where: new Dictionary<string, object> 
    { 
        ["category"] = new Dictionary<string, object> { ["$eq"] = "AI" } 
    },
    nResults: 10,
    include: new List<Include> { Include.documents, Include.metadatas }
);

// Query with document filtering
var documentFilteredResults = await client.QueryRecordsAsync(
    database: "my-database",
    tenant: "my-tenant",
    collectionId: collectionId,
    queryEmbeddings: queryEmbeddings,
    whereDocument: new Dictionary<string, object>
    {
        ["$contains"] = "machine learning"
    },
    nResults: 5
);

// Query with pagination
var paginatedResults = await client.QueryRecordsAsync(
    database: "my-database",
    tenant: "my-tenant",
    collectionId: collectionId,
    queryEmbeddings: queryEmbeddings,
    nResults: 10,
    limit: 5,
    offset: 10
);
```