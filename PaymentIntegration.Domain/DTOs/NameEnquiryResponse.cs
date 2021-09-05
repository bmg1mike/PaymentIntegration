using System;
namespace PaymentIntegration.Domain.DTOs
{
    public class NameEnquiryResponse
    {
        public NameEnquiryResponse()
        {
            ResponseBody = new NameEnquiryResponseBody();
        }
        public string ResponseMessage { get; set; }
        public string ResponseCode { get; set; }
        public bool RequestSuccessful { get; set; }

        public NameEnquiryResponseBody ResponseBody { get; set; }
    }

    public class NameEnquiryResponseBody
    {
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        public string AccountName { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
