namespace StocksApi.Persistence.Entities
{
	public class Stock
	{
		public Guid Id { get; set; }

		public string Symbol { get; set; }

		public double Price { get; set; }

		public DateTime DateTime { get; set; }

		public Stock()
		{
			Id = Guid.NewGuid();
		}
	}
}