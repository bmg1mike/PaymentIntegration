using System;
namespace PaymentIntegration.Domain.DTOs
{
    public class BatchTransferResponse
    {
        
            public bool RequestSuccessful { get; set; }
            public string responseMessage { get; set; }
            public string ResponseCode { get; set; }
            public BulkTransferResponseBody ResponseBody { get; set; }   
    }

    public class BulkTransferResponseBody
    {
        public decimal totalAmount { get; set; }
        public decimal totalFee { get; set; }
        public string batchReference { get; set; }
        public string batchStatus { get; set; }
        public int totalTransactions { get; set; }
        public DateTime dateCreated { get; set; }
    }
}
