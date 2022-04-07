using System;
using System.Data.SqlTypes;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

public class Program
{
    private static readonly string _endpointUri = "https://netsdkcoresql2021.documents.azure.com:443/";
    private static readonly string _primaryKey = "R69RuwXTv7j2y7ZPf6UgHqnDDwjdxges01I6NTxtnDyh1ZmlYqCqFWHVTN7Grksu5RQUDE3Wttf3qk7XwRHyyg==";
    private static readonly string _databaseId = "NutritionDatabase";
    private static readonly string _containerId = "FoodCollection";

    private static CosmosClient _client = new CosmosClient(_endpointUri, _primaryKey, new CosmosClientOptions() { ConnectionMode = ConnectionMode.Direct });

    public static async Task Main(string[] args)
    {
        Database database = _client.GetDatabase(_databaseId);
        Container container = database.GetContainer(_containerId);

        /* Please note: 
         * The indexing metrics are only supported in the .NET SDK (version 3.21.0 or later) and Java SDK (version 4.19.0 or later)
         * So ensure you upgrade your Nuget to minimum 3.21.0 or later for using Indexing Metrics.
        */

        /*
        // Point-Read:
        ItemResponse<Food> candyResponse = await container.ReadItemAsync<Food>("19293", new PartitionKey("Sweets"));
        Food candy = candyResponse.Resource;
        Console.Out.WriteLine($"Read {candy.Description}");
        Console.Out.WriteLine("This is a point-read: looking for item: 19293 with partitionKey = Sweets");
        await Console.Out.WriteLineAsync($"{candyResponse.RequestCharge} RU/s");                        // print out the RU for the read operation.
        Console.Out.WriteLine(candyResponse.Diagnostics.ToString());                                    // print out the Diagnostics for the operation
        Console.Out.WriteLine();

        // In-partition Query: Simple
        string sqlA = "SELECT * FROM c WHERE c.foodGroup = 'Sweets' AND c.id = 19293";                                        // please note: /foodGroup is partitionKey for collection.
        FeedIterator<Food> query2 = container.GetItemQueryIterator<Food>(sqlA);
        FeedResponse<Food> queryResponse2 = await query2.ReadNextAsync();
        await Console.Out.WriteLineAsync($"Query is: {sqlA}");
        await Console.Out.WriteLineAsync($"{queryResponse2.RequestCharge} RUs");
        Console.Out.WriteLine(queryResponse2.Diagnostics.ToString());                                                         // print out the Diagnostics for the operation
        Console.Out.WriteLine();
        */

        // In-partition Query: Complex
        string sqlQueryText = "SELECT * FROM c WHERE c.foodGroup = 'Baby Foods' and IS_DEFINED(c.description) and IS_DEFINED(c.manufacturerName) ORDER BY c.tags.name ASC, c.version ASC";
        QueryDefinition query = new QueryDefinition(sqlQueryText);
        FeedIterator<Food> resultSetIterator = container.GetItemQueryIterator<Food>(query, requestOptions: new QueryRequestOptions { PopulateIndexMetrics = true });
        FeedResponse<Food> response = null;
        while (resultSetIterator.HasMoreResults)
        {
            response = await resultSetIterator.ReadNextAsync();
            Console.WriteLine(response.IndexMetrics);
        }

        await Console.Out.WriteLineAsync($"Query is: {sqlQueryText}");
        await Console.Out.WriteLineAsync($"{response.RequestCharge} RUs");
        Console.Out.WriteLine();

        // Cross-partition Query:
        /*
        string sqlQueryText = "SELECT * FROM c WHERE c.version = 1";
        QueryDefinition query = new QueryDefinition(sqlQueryText);
        FeedIterator<Food> resultSetIterator = container.GetItemQueryIterator<Food>(query, requestOptions: new QueryRequestOptions { PopulateIndexMetrics = true });
        FeedResponse<Food> response = null;
        while (resultSetIterator.HasMoreResults)
        {
            response = await resultSetIterator.ReadNextAsync();
            Console.WriteLine(response.IndexMetrics);
        }

        await Console.Out.WriteLineAsync($"Query is: {sqlQueryText}");
        await Console.Out.WriteLineAsync($"{response.RequestCharge} RUs");
        Console.Out.WriteLine();
        */
    }
}