using Newtonsoft.Json;

namespace QuickCopy.Models
{
	public class TextCopy : BaseCopy
	{
		#region Text

		private string _text;
		[JsonProperty("text")]
		public string Text { get => _text; set => Set(ref _text, value); }

		#endregion Text
	}
}
