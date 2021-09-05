using System;
using System.Threading.Tasks;
using PaymentIntegration.Domain.DTOs;

namespace PaymentIntegration.Application.Contracts
{
    public interface IMonnifyRepository
    {
        Task<SingleTransferResponse> SingleTransfer(SingleTransferRequest request);
    }
}
