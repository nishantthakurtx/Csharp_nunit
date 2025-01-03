using AutoMapper;
using CourseTech.Core.DTOs.Payment;
using CourseTech.Core.Services;
using CourseTech.Core.UnitOfWorks;

namespace CourseTech.Service.Services
{
    public class PaymentService(IUnitOfWork unitOfWork, IMapper mapper) : IPaymentService
    {
        public async Task<PaymentResponseDTO> ProcessPaymentAsync(PaymentRequestDTO paymentRequest)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentResponseDTO> ValidatePaymentAsync(string transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
