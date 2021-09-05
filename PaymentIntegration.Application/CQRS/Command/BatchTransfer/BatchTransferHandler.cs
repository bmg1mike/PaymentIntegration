using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentIntegration.Domain;
using PaymentIntegration.Domain.DTOs;

namespace PaymentIntegration.Application.CQRS.Command.BatchTransfer
{
    public class BatchTransferHandler : IRequestHandler<BatchTransferCommand,ResponseResult<BatchTransferResponse>>
    {
        private readonly ILogger<BatchTransferHandler> _logger;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _client;

        public BatchTransferHandler(ILogger<BatchTransferHandler> logger, IConfiguration config, IHttpClientFactory client)
        {
            _logger = logger;
            _config = config;
            _client = client;
        }

        public async Task<ResponseResult<BatchTransferResponse>> Handle(BatchTransferCommand request, CancellationToken cancellationToken)
        {
            var batchRequest = new BatchTransferRequest
            {
                transactionList = request.Transactions,
                sourceAccountNumber = _config["Monnify:WalletAccountNumber"],
                title = "Bulk Disbursement"
            };

            try
            {
                var result = await BatchTransfer(batchRequest);
                if (result != null)
                {
                    return ResponseResult<BatchTransferResponse>.Success(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResponseResult<BatchTransferResponse>.Failure("There was a problem connecting to monnify. Please try again later");
            }

            return ResponseResult<BatchTransferResponse>.Failure("Something went wrong,Please try again later");

        }

        private async Task<BatchTransferResponse> BatchTransfer(BatchTransferRequest request)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _config["Monnify:BatchTransferUrl"]);
            if (request != null)
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            }

            var authKey = Helper.GetAuthKey(_config["Monnify:ApiKey"], _config["Monnify:ClientSecret"]);

            requestMessage.Headers.Add("Authorization", $"Basic {authKey}");
            requestMessage.Headers.Add("Cache-Control", "no-cache");

            HttpResponseMessage response;
            var client = _client.CreateClient();
            try
            {
                response = await client.SendAsync(requestMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            string result = string.Empty;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = await response.Content.ReadAsStringAsync();
                var transfer = JsonConvert.DeserializeObject<BatchTransferResponse>(result);
                _logger.LogInformation("{@transfer}", transfer);
            }

            result = await response.Content.ReadAsStringAsync();
            _logger.LogError(result);
            return null;
            
        }

        
    }
}
