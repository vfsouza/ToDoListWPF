using System.Windows.Input;

namespace ToDoList.Core;

public class RelayCommand<T> : ICommand {
	private readonly Action<T> _execute;
	private readonly Predicate<T> _canExecute;

	public event EventHandler CanExecuteChanged {
		add { CommandManager.RequerySuggested += value; }
		remove { CommandManager.RequerySuggested -= value; }
	}
	public RelayCommand(Action<T> execute, Predicate<T> canExecute = null) {
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecute = canExecute;
	}

	public bool CanExecute(object? parameter) => _canExecute?.Invoke((T)parameter) ?? true;

	public void Execute(object? parameter) {
		_execute((T)parameter);
	}
}

public class RelayCommand : RelayCommand<object> {
	public RelayCommand(Action execute, Predicate<object> canExecute = null)
		: base(
			execute: _ => execute(),
			canExecute: canExecute
		) { }
}

