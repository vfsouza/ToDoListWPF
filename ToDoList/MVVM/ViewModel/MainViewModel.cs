﻿using ToDoList.Core;
using ToDoList.Services;

namespace ToDoList.MVVM.ViewModel;

public class MainViewModel : Core.ViewModel {
	public RelayCommand NavigateToTasksCommand { get; set; }
	public RelayCommand NavigateToCalendarCommand { get; set; }
	private INavigationService _navigation;

	public MainViewModel() { }

	public MainViewModel(INavigationService navService) {
		Navigation = navService;
		Navigation.NavigateTo<TasksViewModel>();
		NavigateToTasksCommand = new RelayCommand(o => Navigation.NavigateTo<TasksViewModel>(), o => true);
		NavigateToCalendarCommand = new RelayCommand(o => Navigation.NavigateTo<CalendarViewModel>(), o => true);
	}

	public INavigationService Navigation {
		get => _navigation;
		set {
			_navigation = value;
			OnPropertyChanged();
		}
	}
}