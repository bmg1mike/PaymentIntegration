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

namespace PaymentIntegration.Application.CQRS.Command
{
    public class SingleTransferHandler : IRequestHandler<SingleTransferCommand, ResponseResult<SingleTransferResponse>>
    {
        private readonly ILogger<SingleTransferHandler> _logger;
        private readonly IHttpClientFactory _client;
        private readonly IConfiguration _config;

        public SingleTransferHandler(ILogger<SingleTransferHandler> logger, IHttpClientFactory client, IConfiguration config)
        {
            _logger = logger;
            _client = client;
            _config = config;
        }

        public async Task<ResponseResult<SingleTransferResponse>> Handle(SingleTransferCommand request, CancellationToken cancellationToken)
        {
            var monnifyRequest = new SingleTransferRequest
            {
                amount = request.Amount,
                currency = "NGN",
                destinationAccountNumber = request.DestinationAccountNumber,
                destinationBankCode = request.DestinationBankCode,
                narration = "Disbursement",
                sourceAccountNumber = _config["Monnify:WalletAccountNumber"]
            };

            try
            {
                var result = await SingleTransfer(monnifyRequest);
                if(result != null)
                {
                    return ResponseResult<SingleTransferResponse>.Success(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResponseResult<SingleTransferResponse>.Failure("There was a problem connecting to monnify. Please try again later");
            }

            return ResponseResult<SingleTransferResponse>.Failure("Something went wrong,Please try again later");
        }

        private async Task<SingleTransferResponse> SingleTransfer(SingleTransferRequest request)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _config["Monnify:SingleTransferUrl"]);
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

            if(response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SingleTransferResponse>(result);
            }
            else if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                var result = await response.Content.ReadAsStringAsync();
                return null;
            }
            else
            {
                return null;
            }
        }

        
    }
}
