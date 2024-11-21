namespace Infrastructure;

public class Constants
{
    public static class Service
    {
        public const string ServiceName = "SalesPlatform";
        public const string BoundedContext = "Backend";
    }
    
    public static class CosmosDbProperties
    {
        public const string PartitionKeyPath = "/PartitionKey";
        public const string CollectionName = "Usages";
        public const string DatabaseName = "Usage";
        
        public const string TestDatabaseName = "TestUsage";
        public const string TestCollectionName = "TestUsages";
    }
}