using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppAzureClass.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<User> Users { get; set; } = new List<User>();
        [BindProperty]
        public string NewUserName { get; set; }

        private readonly string ConnectionString = "AccountEndpoint=https://cosmosdbwithfuncton.documents.azure.com:443/; AccountKey=ffutxQHVEYypHZP9keNhQYlEM6a1eZAT0BzJ4fkGo4l9oyPcSEO9O1dG2kuTY6jaqC1G26DoZ7JfHXOLQzL8jA==;";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

       

        public async Task<IActionResult> OnGetUsers()
        {
            CosmosClient cosmosClient = new CosmosClient(ConnectionString);
            Database database = cosmosClient.GetDatabase("UserDatabase");
            Container container = database.GetContainer("UserContainer");

            // Hämta och presetera data ifrån CosmosDb

            string sqlQueryText = "SELECT * FROM c";

            QueryDefinition definition = new QueryDefinition(sqlQueryText);

            var iterator = container.GetItemQueryIterator<User>(definition);
            
            while (iterator.HasMoreResults)
            {
                foreach (var item in await iterator.ReadNextAsync())
                {
                    var user = new User
                    {
                        Id = item.Id,
                        Name = item.Name
                    };
                    Users.Add(user);
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            CosmosClient cosmosClient = new CosmosClient(ConnectionString);
            Database database = cosmosClient.GetDatabase("UserDatabase");
            Container container = database.GetContainer("UserContainer");
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = NewUserName
            };
            try
            {
               await container.CreateItemAsync<User>(user, new PartitionKey(user.Name));
            }
            catch (Exception)
            {

                throw;
            }
            return Page();
        }
    }
}
public class User
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
    [JsonProperty(PropertyName = "Name")]
    public string Name { get; set; }

}




