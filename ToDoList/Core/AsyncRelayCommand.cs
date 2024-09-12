using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToDoList.Core {
	public class AsyncRelayCommand<T> : ICommand {
		private readonly Func<T, Task> _execute;
		private readonly Predicate<T> _canExecute;
		private bool _isExecuting;

		public AsyncRelayCommand(Func<T, Task> execute, Predicate<T> canExecute = null) {
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
			_canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object? parameter) => !_isExecuting && (_canExecute?.Invoke((T)parameter) ?? true);


		public async void Execute(object? parameter) {
			if (CanExecute(parameter)) {
				try {
					_isExecuting = true;
					await _execute((T)parameter);
				} finally {
					_isExecuting = false;
				}
			}

			RaiseCanExecuteChanged();
		}

		private void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
	}

	public class AsyncRelayCommand : AsyncRelayCommand<object> {
		public AsyncRelayCommand(Func<Task> execute, Predicate<object> canExecute = null)
			: base(
				  execute: _ => execute(),
				  canExecute: canExecute
			) { }
	}
}
