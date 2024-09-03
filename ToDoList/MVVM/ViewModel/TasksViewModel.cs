using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Data;
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
	private bool _isNotCompletedSelected = false;
	private byte _lastFilterOperation = 2;

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
	public ObservableCollection<string> FilePathStudy { get; set; }
	public ObservableCollection<string> FileTitlesOC { get; set; }
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
	public RelayCommand OpenFileCommand { get; set; }
	public RelayCommand AddNewFilePath { get; set; }
	public RelayCommand AddNewFilePathStudy { get; set; }
	public RelayCommand ChangeTaskCompletion { get; set; }
	public RelayCommand FilterTasksCommand { get; set; }
	public RelayCommand DeletePath { get; set; }
	public RelayCommand OpenURLCommand { get; set; }

	public ITaskService TaskService { get; }
	public ITaskDBService DbService { get; }

	public TasksViewModel(ITaskDBService dbService, INavigationService navigation, ITaskService taskService) {
		Navigation = navigation;
		TaskService = taskService;
		DbService = dbService;
		Tasks = new ObservableCollection<ToDoTask>();
		FilterTasks(_lastFilterOperation);
		FilePaths = new List<string>();
		FileTitlesOC = new ObservableCollection<string>();
		FilePathStudy = new ObservableCollection<string>();

		CreateTaskCommand = new RelayCommand(CreateTask);

		NavigateToToDoTaskView = new RelayCommand((o) => Navigation.NavigateTo<ToDoTaskViewModel>(), o => true);

		AddNewFilePath = new RelayCommand(AddFilePath);

		AddNewFilePathStudy = new RelayCommand(AddFilePathStudy);

		OpenFileCommand = new RelayCommand(OpenFile);

		NavigateToEditToDoTaskView = new RelayCommand((o) => {
			ToDoTask t = (ToDoTask)o;
			TaskService.CurrentTask = t;
			Navigation.NavigateTo<ToDoTaskViewModel>();
		});

		DeleteTaskCommand = new RelayCommand(DeleteTask);

		ChangeTaskCompletion = new RelayCommand(UpdateTaskCompletion);

		FilterTasksCommand = new RelayCommand(FilterTasks);

		DeletePath = new RelayCommand(DeleteFilePath);

		OpenURLCommand = new RelayCommand(OpenURL, CanOpenURL);
	}
	private bool CanOpenURL(object obj) {
		return obj != null && !string.IsNullOrEmpty((string)obj);
	}
	private void OpenURL(object obj) {
		if (obj == null) return;
		string url = (string)obj;
		OpenOnNavigator(url);
	}
	private void FilterTasks(object obj) {
		byte op;
		try {
			op = byte.Parse((string)obj);
		} catch (InvalidCastException) {
			op = (byte)obj;
		}
		Tasks.Clear();
		var tasks = DbService.GetAllTasks().OrderBy(t => t.DueDate);
		DateTime today = DateTime.Now.Date;
		_lastFilterOperation = op;

		switch (op) {
			case 0:
				foreach (ToDoTask t in tasks) {
					if (t.DueDate.Date == today) {
						Tasks.Add(t);
					}
				}
				break;

			case 1:
				foreach (ToDoTask t in tasks) {
					if (t.DueDate.Date >= today && t.DueDate.Date <= today.AddDays(1)) {
						Tasks.Add(t);
					}
				}
				break;

			case 2:
				foreach (ToDoTask t in tasks) {
					if (t.DueDate.Date >= today.AddDays(-(int)today.DayOfWeek) && t.DueDate.Date < today.AddDays(7 - (int)today.DayOfWeek)) {
						Tasks.Add(t);
					}
				}
				break;

			case 3:
				foreach (ToDoTask t in tasks) {
					if (t.DueDate.Date >= new DateTime(today.Year, today.Month, 1) && t.DueDate.Date < new DateTime(today.Year, today.Month, 1).AddMonths(1)) {
						Tasks.Add(t);
					}
				}
				break;

			case 4:
				_isNotCompletedSelected = !_isNotCompletedSelected;
				if (_isNotCompletedSelected) {
					foreach (ToDoTask t in tasks) {
						if (!t.IsCompleted) Tasks.Add(t);
					}
				} else {
					foreach (ToDoTask t in tasks) {
						Tasks.Add(t);
					}
				}
				break;
		}
	}
	private void CreateTask(object obj) {
		if (string.IsNullOrEmpty(TaskTitle)) {
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
		FilterTasks(_lastFilterOperation);
	}
	private void AddFilePath(object obj) {
		OpenFileDialog dlg = new OpenFileDialog();
		dlg.Filter = "pdf files (*.pdf) |*.pdf;";
		dlg.Multiselect = true;
		dlg.ShowDialog();
		if (dlg.FileNames != null) {
			foreach (string file in dlg.FileNames) {
				FilePaths.Add(file);
				string fileName = Path.GetFileName(file);
				int insertIndex = FileTitlesOC.ToList().BinarySearch(fileName);
				if (insertIndex < 0) {
					insertIndex = ~insertIndex;
				}
				FileTitlesOC.Insert(insertIndex, fileName);
			}
		}
		FilePaths = FilePaths.OrderBy(f => f).ToList();
	}
	private void AddFilePathStudy(object obj) {
		if (obj == null) return;
		ToDoTask t = (ToDoTask)obj;
		if (t.FilePathsStudy == null) t.FilePathsStudy = new List<string>();

		OpenFileDialog dlg = new OpenFileDialog();
		dlg.Filter = "pdf files (*.pdf) |*.pdf;";
		dlg.Multiselect = true;
		dlg.ShowDialog();

		if (dlg.FileNames != null) {
			foreach (string file in dlg.FileNames) {
				t.FilePathsStudy.Add(file);
			}
		}

		FilterTasks(_lastFilterOperation);
		DbService.UpdateTask(t);
	}
	private void OpenFile(object obj) {
		if (obj == null) return;
		string path = (string)obj;
		OpenOnNavigator(path);
	}
	private void DeleteTask(object obj) {
		if (MessageBox.Show(CanvasLink, "Are you sure you want to delete this task?", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
			DbService.DeleteTask(((ToDoTask)obj).Id);
			Tasks.Clear();
			foreach (ToDoTask t in DbService.GetAllTasks()) {
				Tasks.Add(t);
			}
		}
	}
	private void UpdateTaskCompletion(object obj) {
		ToDoTask t = (ToDoTask)obj;
		DbService.UpdateTask(t);
		_isNotCompletedSelected = !_isNotCompletedSelected;
		FilterTasks(_lastFilterOperation);
	}
	private void DeleteFilePath(object obj) {
		string path = (string)obj;
		FilePaths.Remove(FilePaths.Find(a => a.Contains(path)));
		FileTitlesOC.Remove(path);
	}
	private Process? OpenOnNavigator(string path) => Process.Start(new ProcessStartInfo() { FileName = (string)path, UseShellExecute = true });
}