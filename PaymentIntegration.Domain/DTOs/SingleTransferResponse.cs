using System;
namespace PaymentIntegration.Domain.DTOs
{
    public class SingleTransferResponse
    {
        public bool RequestSuccessful { get; set; }
        public string responseMessage { get; set; }
        public string ResponseCode { get; set; }
        public SingleTransferResponseBody ResponseBody { get; set; }
    }

    public class SingleTransferResponseBody
    {
        public string Reference { get; set; }
        public string status { get; set; }
        public DateTime DateCreated { get; set; }
    }
}