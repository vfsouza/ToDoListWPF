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
	public RelayCommand NavigateToEditToDoTaskView { get; set; }
	public ToDoTask SelectedTask { 
		get => _selectedTask; 
		set {
			_selectedTask = value; 
			OnToDoTaskSelected();
		}
	}
	public INavigationService Navigation { 
		get => _navigation; 
		set { 
			_navigation = value;
			OnPropertyChanged();
		}
	}

	public ITaskService TaskService { get; }

	public ITaskDBService DbService { get; }

	public TasksViewModel(ITaskDBService dbService, INavigationService navigation, ITaskService taskService) {
		Navigation = navigation;
		TaskService = taskService;
		DbService = dbService;
		Tasks = DbService.GetAllTasks();

		NavigateToToDoTaskView = new RelayCommand((o) => Navigation.NavigateTo<ToDoTaskViewModel>(), o => true);
		NavigateToEditToDoTaskView = new RelayCommand((o) => { 
			ToDoTask t = (ToDoTask) o;
			TaskService.CurrentTask = t;
			Navigation.NavigateTo<ToDoTaskViewModel>();
		}, o => true);
	}

	public void OnToDoTaskSelected() {
		TaskService.CurrentTask = SelectedTask;
		Navigation.NavigateTo<ToDoTaskViewModel>();
	}
}