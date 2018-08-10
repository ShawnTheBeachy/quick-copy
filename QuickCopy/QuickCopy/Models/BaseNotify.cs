using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuickCopy.Models
{
    /// <summary>
    /// Base notifier for binding.
    /// </summary>
    public class BaseNotify : INotifyPropertyChanged
    {
        /// <summary>
        /// Event raised when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property for which to raise the PropertyChanged event.</param>
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets a field and raises the PropertyChanged event for the calling property.
        /// </summary>
        /// <typeparam name="T">The type of the field being set.</typeparam>
        /// <param name="storage">The field being set.</param>
        /// <param name="value">The value to be assigned.</param>
        /// <param name="propertyName">The name of the calling property. Automatically inferred.</param>
        /// <returns>Whether the operation was successful.</returns>
        public bool Set<T>(ref T storage, T value, [CallerMemberName()]string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }
    }
}
