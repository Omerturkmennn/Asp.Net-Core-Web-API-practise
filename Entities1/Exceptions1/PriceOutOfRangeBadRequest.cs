namespace Entities1.Exceptions1
{
    public class PriceOutOfRangeBadRequest : BadRequest
    {
        public PriceOutOfRangeBadRequest() : base("Max price must be less than 1000 an greater than 10!")
        {
        }
    }
}
