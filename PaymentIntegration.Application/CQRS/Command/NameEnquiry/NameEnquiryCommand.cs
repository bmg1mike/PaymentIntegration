using System;
using MediatR;
using PaymentIntegration.Domain.DTOs;

namespace PaymentIntegration.Application.CQRS.Command.NameEnquiry
{
    public class NameEnquiryCommand : IRequest<ResponseResult<NameEnquiryResponse>>
    {
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
    }
}
