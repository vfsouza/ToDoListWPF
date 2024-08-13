using System.Collections.ObjectModel;
using ToDoList.Core;
using ToDoList.Data;
using ToDoList.Services;

namespace ToDoList.MVVM.ViewModel;

public class TasksViewModel : Core.ViewModel {
	private ToDoTask _selectedTask;
	private INavigationService _navigation;
	private ITaskDBService _dbService;

	public ObservableCollection<ToDoTask> Tasks { get; }
	public ObservableCollection<string> FilePaths { get; }
	public RelayCommand NavigateToToDoTaskView { get; set; }
	public ToDoTask SelectedTask { 
		get => _selectedTask; 
		set {
			_selectedTask = value; 
			OnToDoTaskSelected();
			OnPropertyChanged();
		}
	}
	public INavigationService Navigation { 
		get => _navigation; 
		set { 
			_navigation = value;
			OnPropertyChanged();
		}
	}

	public ITaskDBService DbService { 
		get => _dbService;  
		set { 
			_dbService = value;
			OnPropertyChanged();
		}
	}

	public TasksViewModel(ITaskDBService dbService, INavigationService navigation) {
		Navigation = navigation;
		DbService = dbService;
		Tasks = DbService.GetAllTasks();

		NavigateToToDoTaskView = new RelayCommand((o) => Navigation.NavigateTo<ToDoTaskViewModel>());
	}

	public void OnToDoTaskSelected() {
		Navigation.NavigateTo<ToDoTaskViewModel>();
	}
}