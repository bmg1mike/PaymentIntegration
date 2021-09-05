using System;
using MediatR;
using PaymentIntegration.Domain.DTOs;

namespace PaymentIntegration.Application.CQRS.Command
{
    public class SingleTransferCommand : IRequest<ResponseResult<SingleTransferResponse>>
    {
        public decimal Amount { get; set; }
        public string DestinationBankCode { get; set; }
        public string DestinationAccountNumber { get; set; }
    }
}
