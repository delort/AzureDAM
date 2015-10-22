namespace Avanade.AzureDAM.DemoSetup.Search
{
    public class SearchConfiguration
    {
        public const string DatasourceName = "assetmetadata";
        public const string IndexName = "defaultassetindex";
        public const string IndexerName = "assetindexer";

        public static string BuildDataSourceDefinition(string connectionString)
        {
            // ""query"" : ""SELECT id, Name, Description, Author, Category, ContentType, Url, _ts FROM Assets WHERE _ts > @HighWaterMark""

            return $@"{{
            ""name"" : ""{DatasourceName}"",
            ""description"":"""",
            ""type"" : ""documentdb"",
            ""credentials"" : {{
                ""connectionString"" : ""{connectionString}""
            }},    
            ""container"" : {{
                    ""name"" : ""Assets""
                }},
            ""dataChangeDetectionPolicy"" : {{
                ""@odata.type"" : ""#Microsoft.Azure.Search.HighWaterMarkChangeDetectionPolicy"",
                ""highWaterMarkColumnName"": ""_ts""
                }}
            }}";
        }

        public static string BuildIndexDefinition()
        {
            //  ""suggesters"" : """",
            return $@"{{
                ""name"":""{IndexName}"",              
                ""fields"": [
                    {{""name"":""id"", ""type"":""Edm.String"", 
                      ""key"":""true"", ""retrievable"":""true"",
                      ""filterable"":""false"",""sortable"":""false"", 
                      ""facetable"":""false"", ""searchable"":""true""
                    }},
                    {{""name"":""Name"", ""type"":""Edm.String"", 
                      ""key"":""false"", ""retrievable"":""true"",
                      ""filterable"":""false"",""sortable"":""true"", 
                      ""facetable"":""false"", ""searchable"":""true""
                    }},
                    {{""name"":""Description"", ""type"":""Edm.String"", 
                      ""key"":""false"", ""retrievable"":""true"",
                      ""filterable"":""false"",""sortable"":""false"", 
                      ""facetable"":""false"", ""searchable"":""true""
                    }}, 
                    {{""name"":""Author"", ""type"":""Edm.String"", 
                      ""key"":""false"", ""retrievable"":""true"",
                      ""filterable"":""false"",""sortable"":""false"", 
                      ""facetable"":""true"", ""searchable"":""true""
                    }},
                    {{""name"":""Category"", ""type"":""Edm.String"", 
                      ""key"":""false"", ""retrievable"":""false"",
                      ""filterable"":""false"",""sortable"":""false"", 
                      ""facetable"":""true"", ""searchable"":""true""
                    }},
                    {{""name"":""ContentType"", ""type"":""Edm.String"", 
                      ""key"":""false"", ""retrievable"":""false"",
                      ""filterable"":""false"",""sortable"":""false"", 
                      ""facetable"":""false"", ""searchable"":""true""
                    }},
                    {{""name"":""Type"", ""type"":""Edm.String"", 
                      ""key"":""false"", ""retrievable"":""true"",
                      ""filterable"":""false"",""sortable"":""false"", 
                      ""facetable"":""true"", ""searchable"":""true""
                    }},
                    {{""name"":""Url"", ""type"":""Edm.String"", 
                      ""key"":""false"", ""retrievable"":""true"",
                      ""filterable"":""false"",""sortable"":""false"", 
                      ""facetable"":""true"", ""searchable"":""true""
                    }},
                    {{""name"":""Keywords"", ""type"":""Edm.String"", 
                      ""key"":""false"", ""retrievable"":""false"",
                      ""filterable"":""false"",""sortable"":""false"", 
                      ""facetable"":""false"", ""searchable"":""true""
                    }}
                ]
            }}";
        }

        public static string BuildIndexerDefinition()
        {
            return $@"{{
                ""name"":""{IndexerName}"", 
                ""description"":"""",
                ""dataSourceName"":""{DatasourceName}"", 
                ""targetIndexName"":""{IndexName}"",
                
                ""schedule"":{{
                    ""interval"":""PT10H"",
                    ""startTime"":""2015-01-01T00:00:00Z""
                }},
                ""parameters"":{{
                    ""maxFailedItems"":""10"",
                    ""maxFailedItemsPerBatch"":""5"",
                    ""base64EncodeKeys"":""false""
                }}
            }}";
        }
    }
}