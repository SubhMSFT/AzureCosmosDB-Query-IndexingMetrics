# Indexing Metrics - Understanding Indexing in Azure Cosmos DB Querying Better
How to do an Effective Study of Indexing Metrics for a Query in Azure Cosmos DB using .NET SDK

**Summary:**
This document provides guidance on getting your hands dirty using Azure Cosmos DB Core (SQL) API .NET SDK

![Image0](images/header.png)

# Contents

[Introduction](#Introduction)

[Points to Note](#points-to-note)

[How to Enable Indexing Metrics](#enabling-indexing-metrics)

[Feedback](#feedback)

[License/Terms of Use](#license--terms-of-use)

## Introduction
Azure Cosmos DB Core (SQL) API provides **Indexing Metrics** to show both utilized indexed paths and recommended indexed paths. As an Azure Cosmos DB developer, you can leverage these Core (SQL) API indexing metrics to optimize query performance.

## Points to Note
3 important points to note:
1. Indexing Metrics is especially useful in cases where you are not sure how to modify the indexing policy for your specific use-case and Query Patterns.
2. Indexing Metrics is available for usage in Azure Cosmos DB .NET SDK and Java SDK, as of now.
3. Indexing Metrics require a minimum version: 1) If you're on .NET SDK, you need to ensure you're using .NET SDK ver 3.21.0 or later, 2) If you're on Java SDK, you need to ensure you're using 4.19.0 or later.

## How to Enable Indexing Metrics
You can enable indexing metrics for a query by setting the PopulateIndexMetrics property to true. When not specified, PopulateIndexMetrics defaults to false. 

## An Example in .NET SDK
I have an Azure Cosmos DB database named 'NutritionDatabase' and container named 'FoodCollection'.
You can see a sample schema of the items in the container.
![Image1](image/image1.jpg)

