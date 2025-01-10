using Microsoft.AspNetCore.Identity;

namespace StocksApi.Persistence.Entities
{
	public class AppUser : IdentityUser<Guid>
	{
		public AppUser() : base() { }

		public AppUser(Guid id, string email, string? name = null, string? lastName = null)
			: base(email)
		{
			Id = id;
			Email = email;
			Name = name ?? string.Empty;
			LastName = lastName ?? string.Empty;
		}

		public string? Name { get; set; }

		public string? LastName { get; set; }
	}
}