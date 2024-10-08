﻿using System.Windows.Input;

namespace ToDoList.Core;

public class RelayCommand : ICommand {
	private Action<object> _execute;
	private Predicate<object> _canExecute;

	public event EventHandler CanExecuteChanged {
		add => CommandManager.RequerySuggested += value;
		remove => CommandManager.RequerySuggested -= value;
	}

	public RelayCommand(Action<object> execute, Predicate<object> canExecute = null) {
		_execute = execute;
		_canExecute = canExecute;
	}

	public bool CanExecute(object parameter) {
		return _canExecute(parameter);
	}

	public void Execute(object parameter) {
		_execute(parameter);
	}
}