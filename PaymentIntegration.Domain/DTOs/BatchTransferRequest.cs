using System;
using System.Collections.Generic;

namespace PaymentIntegration.Domain.DTOs
{
    public class BatchTransferRequest
    {
        public string title { get; set; }
        public string batchReference { get; set; } = Helper.ReferenceNumber();
        public string narration { get; set; } = "Batch Disbursement";
        public string sourceAccountNumber { get; set; }
        public string onValidationFailure { get; set; } = "CONTINUE";
        public int notificationInterval { get; set; } = 25;
        public List<TransactionList> transactionList { get; set; }
    }

    public class TransactionList
    {
        public decimal amount { get; set; }
        public string reference { get; set; } = Helper.ReferenceNumber();
        public string narration { get; set; }
        public string destinationBankCode { get; set; }
        public string destinationAccountNumber { get; set; }
        public string currency { get; set; } = "NGN";
    }
}
