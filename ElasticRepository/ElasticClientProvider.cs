using System;
using Nest;

namespace ElasticRepository
{
    public class ElasticClientProvider
    {
        public  ElasticClient GetElasticClient(string uri)
        {
            var connectionSettings = new ConnectionSettings(new Uri(uri)).RequestTimeout(TimeSpan.FromMinutes(5));


            return new ElasticClient(connectionSettings);
        }
    }
}
