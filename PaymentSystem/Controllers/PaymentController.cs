﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Model.Dto.Payments;
using PaymentSystem.Services.Interfaces;

namespace PaymentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _repository;
        private readonly INotifier _notifier;
        private readonly ICardValidator _validator;


        public PaymentController(
            IPaymentRepository repository,
            INotifier notifier,
            ICardValidator validator
        )
        {
            _repository = repository;
            _notifier = notifier;
            _validator = validator;
        }

        [HttpGet("history")]
        [Authorize]
        public IAsyncEnumerable<PaymentRecord> GetPaymentHistory(
            [FromQuery]DateTime periodStart, [FromQuery]DateTime periodEnd
        ) => _repository.GetPaymentHistoryAsync(periodStart, periodEnd);

        [HttpGet("session")]
        public async Task<IActionResult> GetPaymentSession([FromBody]PaymentRequest payment) =>
            Ok(await _repository.RecordPaymentAsync(payment));
        
        [HttpPost("initiate")]
        public async Task<IActionResult> InitiatePayment(
            [FromBody]Card cardDetails, [FromQuery]Guid sessionId, [FromQuery]string callback
        )
        {
            await _repository.SessionIsActiveAsync(sessionId);
            _validator.ValidateCard(cardDetails);
            if (!String.IsNullOrWhiteSpace(callback) && 
                    Uri.IsWellFormedUriString(callback, UriKind.Absolute))
                            await _notifier.SendAsyncNotification(new Uri(callback), sessionId.ToString());
            await _repository.MakePaymentAsync(sessionId, cardDetails);
            return Ok();
        }
    }
}
