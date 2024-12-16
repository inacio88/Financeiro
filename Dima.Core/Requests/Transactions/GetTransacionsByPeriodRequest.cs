namespace Dima.Core.Requests.Transactions
{
    public class GetTransacionsByPeriodRequest : PagedRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}