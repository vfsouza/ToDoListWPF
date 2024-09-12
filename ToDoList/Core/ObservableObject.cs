using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoList.Core;

public class ObservableObject : INotifyPropertyChanged {
	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string name = null) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}

	protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null) {
		if (EqualityComparer<T>.Default.Equals(field, value)) return false;
		field = value;
		OnPropertyChanged(name);
		return true;
	}

	protected bool SetProperty<T>(ref T field, T value, Action onChanged, [CallerMemberName] string name = null) {
		if (EqualityComparer<T>.Default.Equals(field, value)) return false;
		field = value;
		onChanged?.Invoke();
		OnPropertyChanged(name);
		return true;
	}
}