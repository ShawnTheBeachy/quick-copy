using QuickCopy.Models;
using System.Collections.ObjectModel;

namespace QuickCopy.ViewModels
{
	public class MainWindowViewModel : BaseNotify
	{
		#region Copies

		public ObservableCollection<BaseCopy> Copies { get; set; } = new ObservableCollection<BaseCopy>();

		#endregion Copies
	}
}
