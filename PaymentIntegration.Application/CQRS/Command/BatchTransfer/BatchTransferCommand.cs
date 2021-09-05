using System;
using System.Collections.Generic;
using MediatR;
using PaymentIntegration.Domain.DTOs;

namespace PaymentIntegration.Application.CQRS.Command.BatchTransfer
{
    public class BatchTransferCommand : IRequest<ResponseResult<BatchTransferResponse>>
    {
        public List<TransactionList> Transactions { get; set; }
    }
}
