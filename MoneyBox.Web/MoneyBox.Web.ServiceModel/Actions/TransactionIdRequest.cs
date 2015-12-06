using ServiceStack;

namespace MoneyBox.Web.ServiceModel.Actions
{
    [Route("/transactions/{Id}", "GET, DELETE")]
    public class TransactionIdRequest : IReturnVoid
    {
        public long Id { get; set; }
    }
}