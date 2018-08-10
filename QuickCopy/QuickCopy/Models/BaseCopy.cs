using Newtonsoft.Json;
using System;

namespace QuickCopy.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	public abstract class BaseCopy : BaseNotify
	{
		#region Created

		private DateTime _created;
		[JsonProperty("created")]
		public DateTime Created { get => _created; set => Set(ref _created, value); }

		#endregion Created

		#region Id

		private Guid _id;
		[JsonProperty("id")]
		public Guid Id { get => _id; set => Set(ref _id, value); }

		#endregion Id
	}
}
