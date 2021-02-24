using System;

namespace Laraue.EfCoreTriggers.SqlServerTests.Issues._3
{
	public class SalesCategory : DomainBase
	{
		public EntityStatus Status { get; set; }
	}

	public class SalesArea : DomainBase
	{
		public string Name { get; set; }

		public EntityStatus Status { get; set; }
	}

	public enum EntityStatus
	{
		New,
		Draft,
	}

	public abstract class DomainBase
	{
		public long Id { get; set; }

		public DateTime CreatedDate { get; set; }

		public string CreatedBy { get; set; }

		public string LastModifiedBy { get; set; }

		public DateTime LastModifiedDate { get; set; }

		public byte[] RowVersion { get; set; }
	}
}
