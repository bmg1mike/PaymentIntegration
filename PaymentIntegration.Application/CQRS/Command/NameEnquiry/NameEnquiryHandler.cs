using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentIntegration.Domain;
using PaymentIntegration.Domain.DTOs;

namespace PaymentIntegration.Application.CQRS.Command.NameEnquiry
{
    public class NameEnquiryHandler : IRequestHandler<NameEnquiryCommand, ResponseResult<NameEnquiryResponse>>
    {
        private readonly IHttpClientFactory _client;
        private readonly ILogger<NameEnquiryHandler> _logger;
        private readonly IConfiguration _config;

        public NameEnquiryHandler(IHttpClientFactory client, ILogger<NameEnquiryHandler> logger, IConfiguration config)
        {
            _client = client;
            _logger = logger;
            _config = config;
        }

        public async Task<ResponseResult<NameEnquiryResponse>> Handle(NameEnquiryCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.AccountNumber))
            {
                return ResponseResult<NameEnquiryResponse>.Failure("Account Number can't be empty");
            }
            if (string.IsNullOrEmpty(request.BankCode))
            {
                return ResponseResult<NameEnquiryResponse>.Failure("Bank code can't be empty");
            }

            var response = await NameEnquiry(request.AccountNumber, request.BankCode);

            if (response != null)
            {
                return ResponseResult<NameEnquiryResponse>.Success(response);
            }

            return ResponseResult<NameEnquiryResponse>.Failure("There was a problem connecting to monnify, please try again later");
        }

        private async Task<NameEnquiryResponse> NameEnquiry(string accountNumber,string bankCode)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, _config["Monnify:AccountEnquire"] + "accountNumber=" + accountNumber + "&" + "bankCode=" + bankCode);

            var authKey = Helper.GetAuthKey(_config["Monnify:ApiKey"], _config["Monnify:ClientSecret"]);
            requestMessage.Headers.Add("Authorization", $"Basic {authKey}");
            requestMessage.Headers.Add("Cache-Control", "no-cache");
            requestMessage.Headers.Add("Content-Type", "application/json");

            var client = _client.CreateClient();
            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(requestMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError("{@ex}", ex);
                return null;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<NameEnquiryResponse>(result);
            }
            else if (!response.IsSuccessStatusCode)
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
