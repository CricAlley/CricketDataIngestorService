using System;
using Nest;

namespace ElasticRepo
{
    public class ElasticClientProvider
    {
        public  ElasticClient GetElasticClient()
        {
            var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200/")).RequestTimeout(TimeSpan.FromMinutes(5));


            return new ElasticClient(connectionSettings);
        }
    }
}
