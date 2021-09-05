using System;
namespace PaymentIntegration.Domain.DTOs
{
    public class SingleTransferRequest
    {
        public decimal amount { get; set; }
        public string reference { get; set; } = Helper.ReferenceNumber();
        public string narration { get; set; }
        public string destinationBankCode { get; set; }
        public string destinationAccountNumber { get; set; }
        public string currency { get; set; } = "NGN";
        public string sourceAccountNumber { get; set; } 
    }
}