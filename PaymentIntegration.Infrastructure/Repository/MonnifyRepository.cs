using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentIntegration.Application.Contracts;
using PaymentIntegration.Domain.DTOs;

namespace PaymentIntegration.Infrastructure.Repository
{
    public class MonnifyRepository : IMonnifyRepository
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _client;
        private readonly ILogger<MonnifyRepository> _logger;

        public MonnifyRepository(IConfiguration config, IHttpClientFactory client, ILogger<MonnifyRepository> logger)
        {
            _config = config;
            _client = client;
            _logger = logger;
        }

        public async Task<SingleTransferResponse> SingleTransfer(SingleTransferRequest request)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://sandbox.monnify.com/api/v2/disbursements/single");

            requestMessage.Headers.Add("", "");

            if (request != null)
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            }

            var client = _client.CreateClient();

            HttpResponseMessage response;

            try
            {
                //response = await client.SendAsync(requestMessage);
                response = await client.SendAsync(requestMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

            if(response.StatusCode == HttpStatusCode.OK)
            {

                var result = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<SingleTransferResponse>(result);
            }
            else
            {
                return null;
            }
        }

        
    }
}
