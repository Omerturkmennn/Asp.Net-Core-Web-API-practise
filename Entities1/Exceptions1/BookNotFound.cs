namespace Entities1.Exceptions1
{
    public sealed class BookNotFound : NotFoundException
    {
        public BookNotFound(int id) : base($"The book with id: {id} could not found")
        {
        }
    }
}