using System;
using MoneyBox.Web.ServiceModel.Actions;
using ServiceStack.FluentValidation;

namespace MoneyBox.Web.ServiceModel.DTO
{
    
    public class TransactionDto
    {
        public long TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CurrencyCode { get; set; }
        public string Merchant { get; set; }
    }

    public class TransactionValidator : AbstractValidator<TransactionDto>
    {
        public TransactionValidator()
        {
            RuleFor(r => r.TransactionId).NotEmpty();
            RuleFor(r => r.CreatedDate).NotEmpty();
            RuleFor(r => r.CurrencyCode).NotEmpty();
            RuleFor(r => r.ModifiedDate).NotEmpty();
            RuleFor(r => r.TransactionAmount).NotEmpty();
            RuleFor(r => r.TransactionDate).NotEmpty();
        }
    }

    public class TransactionIdValidator : AbstractValidator<TransactionIdRequest>
    {
        public TransactionIdValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
        }
    }
}