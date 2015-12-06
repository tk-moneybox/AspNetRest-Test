using MoneyBox.Web.ServiceModel.DTO;
using ServiceStack;

namespace MoneyBox.Web.ServiceModel.Actions
{
    [Route("/transactions/", "POST PUT")]
    public class UpsertTransaction
    {
        public TransactionDto TransactionDto { get; set; }
    }
}