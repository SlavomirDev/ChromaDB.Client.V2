namespace ChromaDB.Client.V2
{
    public static class ChromaClientExtensions
    {
        /// <summary>
        /// Creates a tenant-scoped client that fixes the tenant for subsequent operations.
        /// </summary>
        /// <param name="tenant">The tenant name to bind to the returned client.</param>
        /// <returns>An ITenantClient scoped to the specified tenant.</returns>
        public static ITenantClient ForTenant(this ChromaClient chromaClient, string tenant) => new TenantClient(chromaClient, tenant);
    }
}