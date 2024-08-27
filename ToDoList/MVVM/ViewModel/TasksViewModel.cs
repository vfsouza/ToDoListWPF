using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Windows;
using ToDoList.Core;
using ToDoList.Data;
using ToDoList.Services;

namespace ToDoList.MVVM.ViewModel;

public class TasksViewModel : Core.ViewModel {
	private INavigationService _navigation;
	private ITaskDBService _dbService;
	private string _taskTitle;
	private string _taskDescription;
	private DateTime? _dueDate;
	private string _canvasLink;
	private List<string> _filePaths;

	public string TaskTitle {
		get => _taskTitle;
		set {
			_taskTitle = value;
			OnPropertyChanged();
		}
	}
	public string TaskDescription {
		get => _taskDescription;
		set {
			_taskDescription = value;
			OnPropertyChanged();
		}
	}
	public DateTime? DueDate {
		get => _dueDate;
		set {
			_dueDate = value;
			OnPropertyChanged();
		}
	}
	public string CanvasLink {
		get => _canvasLink;
		set {
			_canvasLink = value;
			OnPropertyChanged();
		}
	}
	public List<string> FilePaths {
		get => _filePaths;
		set {
			_filePaths = value;
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

	public ObservableCollection<ToDoTask> Tasks { get; }
	public RelayCommand CreateTaskCommand { get; set; }
	public RelayCommand NavigateToToDoTaskView { get; set; }
	public RelayCommand NavigateToEditToDoTaskView { get; set; }
	public RelayCommand DeleteTaskCommand { get; set; }

	public ITaskService TaskService { get; }
	public ITaskDBService DbService { get; }

	public TasksViewModel(ITaskDBService dbService, INavigationService navigation, ITaskService taskService) {
		Navigation = navigation;
		TaskService = taskService;
		DbService = dbService;
		Tasks = DbService.GetAllTasks();

		CreateTaskCommand = new RelayCommand(CreateTask);

		NavigateToToDoTaskView = new RelayCommand((o) => Navigation.NavigateTo<ToDoTaskViewModel>(), o => true);
		NavigateToEditToDoTaskView = new RelayCommand((o) => {
			ToDoTask t = (ToDoTask) o;
			TaskService.CurrentTask = t;
			Navigation.NavigateTo<ToDoTaskViewModel>();
		}, o => true);
		DeleteTaskCommand = new RelayCommand(o => {
			if (MessageBox.Show(CanvasLink, "Are you sure you want to delete this task?", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
				DbService.DeleteTask(((ToDoTask)o).Id);
				Tasks.Clear();
				foreach (ToDoTask t in DbService.GetAllTasks()) {
					Tasks.Add(t);
				}
			}
		});
	}

	private void CreateTask(object obj) {
		if (string.IsNullOrEmpty(TaskTitle) || string.IsNullOrEmpty(TaskDescription)) {
			MessageBox.Show("Please fill out all fields.");
			return;
		} else {
			DbService.AddTask(new ToDoTask {
				Title = TaskTitle,
				Description = TaskDescription,
				DueDate = DueDate ?? DateTime.Now,
				CanvasLink = CanvasLink,
				FilePaths = FilePaths
			});
			Tasks.Clear();
			foreach (ToDoTask t in DbService.GetAllTasks()) {
				Tasks.Add(t);
			}
		}
	}
}