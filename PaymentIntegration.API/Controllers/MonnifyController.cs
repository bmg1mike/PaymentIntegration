using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentIntegration.Application.CQRS.Command;
using PaymentIntegration.Application.CQRS.Command.BatchTransfer;
using PaymentIntegration.Application.CQRS.Command.NameEnquiry;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PaymentIntegration.API.Controllers
{
    public class MonnifyController : BaseAPIController
    {
        [HttpPost("SingleTransfer")]
        public async Task<IActionResult> SingleTransfer([FromBody] SingleTransferCommand command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpPost("BatchTransfer")]
        public async Task<IActionResult> BatchTransfer([FromBody] BatchTransferCommand command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpPost("NameEnquiry")]
        public async Task<IActionResult> NameEnquiry([FromBody] NameEnquiryCommand command)
        {
            return HandleResult(await Mediator.Send(command));
        }
    }
}
