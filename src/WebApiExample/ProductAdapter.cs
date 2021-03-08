using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApiExample
{
    public interface IProductAdapter
    {
        Task<Product> GetProduct(int productId);
    }

    public class ProductAdapter : IProductAdapter
    {
        private readonly HttpClient _client;

        public ProductAdapter(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<Product> GetProduct(int productId)
        {
            var response = await _client.GetAsync($"api/product/{productId}").ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var product = JsonConvert.DeserializeObject<Product>(content);

            return product;
        }
    }
}
