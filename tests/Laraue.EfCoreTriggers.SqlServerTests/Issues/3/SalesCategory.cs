namespace Laraue.EfCoreTriggers.SqlServerTests.Issues._3
{
	public class SalesCategory : DomainBase
	{
		public EntityStatus Status { get; set; }
	}

	public enum EntityStatus
	{
		New,
		Draft,
	}
}
