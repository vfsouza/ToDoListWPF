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
	private DateTime _dueDate = DateTime.Now;
	private string _canvasLink;
	private ObservableCollection<string> _filePaths;
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
	public DateTime DueDate {
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
	public ObservableCollection<string> FilePaths {
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
	public RelayCommand<ToDoTask> DeleteTaskCommand { get; set; }
	public RelayCommand<string> OpenFileCommand { get; set; }
	public RelayCommand<object[]> AddNewFilePath { get; set; }
	public AsyncRelayCommand<ToDoTask> ChangeTaskCompletion { get; set; }
	public AsyncRelayCommand<byte> FilterTasksCommand { get; set; }
	public RelayCommand<string> DeletePath { get; set; }
	public RelayCommand<object[]> DeleteTaskPath { get; set; }
	public RelayCommand<string> OpenURLCommand { get; set; }
	public RelayCommand<ToDoTask> ChangeTaskDate { get; set; }
	public RelayCommand<ToDoTask> ToggleEditModeCommand { get; set; }
	public RelayCommand<ToDoTask> EditTaskCommand { get; set; }

	public ITaskService TaskService { get; }
	public ITaskDBService DbService { get; }

	public TasksViewModel(ITaskDBService dbService, INavigationService navigation, ITaskService taskService) {
		Navigation = navigation;
		TaskService = taskService;
		DbService = dbService;
		Tasks = new ObservableCollection<ToDoTask>();
		FilterTasksAsync(_lastFilterOperation);
		FilePaths = new ObservableCollection<string>();
		FileTitlesOC = new ObservableCollection<string>();
		FilePathStudy = new ObservableCollection<string>();

		CreateTaskCommand = new RelayCommand(CreateTask);

		AddNewFilePath = new RelayCommand<object[]>(AddFilePath);

		OpenFileCommand = new RelayCommand<string>(OpenFile);

		DeleteTaskCommand = new RelayCommand<ToDoTask>(DeleteTask);

		ChangeTaskCompletion = new AsyncRelayCommand<ToDoTask>(UpdateTaskCompletionAsync);

		FilterTasksCommand = new AsyncRelayCommand<byte>(FilterTasksAsync);

		DeletePath = new RelayCommand<string>(DeleteFilePath);

		DeleteTaskPath = new RelayCommand<object[]>(DeleteTaskFilePath);

		OpenURLCommand = new RelayCommand<string>(OpenURL, CanOpenURL);

		ChangeTaskDate = new RelayCommand<ToDoTask>(ChangeTaskDueDate);

		ToggleEditModeCommand = new RelayCommand<ToDoTask>(ToggleEditMode);

		EditTaskCommand = new RelayCommand<ToDoTask>(EditTask);
	}

	private void ToggleEditMode(ToDoTask task) {
		task.IsEditing = !task.IsEditing;
	}
	private void EditTask(ToDoTask task) {

		DbService.UpdateTaskAsync(task);

		task.IsEditing = !task.IsEditing;
	}
	private void ChangeTaskDueDate(ToDoTask task) {
		DbService.UpdateTaskAsync(task);
	}
	private bool CanOpenURL(string url) {
		return !string.IsNullOrEmpty(url);
	}
	private void OpenURL(string url) {
		OpenOnNavigator(url);
	}
	private async Task FilterTasksAsync(byte op) {

		if (op == 4) {
			op = _lastFilterOperation;
			_isNotCompletedSelected = !_isNotCompletedSelected;
		}

		Tasks.Clear();
		var tasks = await DbService.GetAllTasksAsync();
		DateTime today = DateTime.Now.Date;
		_lastFilterOperation = op;

		switch (op) {
			case 0:
				foreach (ToDoTask t in tasks) {
					if (t.DueDate.Date == today) {
						if (_isNotCompletedSelected && !t.IsCompleted) {
							Tasks.Add(t);
						} else if (!_isNotCompletedSelected) {
							Tasks.Add(t);
						}
					}
				}
				break;

			case 1:
				foreach (ToDoTask t in tasks) {
					if (t.DueDate.Date >= today && t.DueDate.Date <= today.AddDays(1)) {
						if (_isNotCompletedSelected && !t.IsCompleted) {
							Tasks.Add(t);
						} else if (!_isNotCompletedSelected) {
							Tasks.Add(t);
						}
					}
				}
				break;

			case 2:
				foreach (ToDoTask t in tasks) {
					if (t.DueDate.Date >= today.AddDays(-(int)today.DayOfWeek) && t.DueDate.Date < today.AddDays(7 - (int)today.DayOfWeek)) {
						if (_isNotCompletedSelected && !t.IsCompleted) {
							Tasks.Add(t);
						} else if (!_isNotCompletedSelected) {
							Tasks.Add(t);
						}
					}
				}
				break;

			case 3:
				foreach (ToDoTask t in tasks) {
					if (t.DueDate.Date >= new DateTime(today.Year, today.Month, 1) && t.DueDate.Date < new DateTime(today.Year, today.Month, 1).AddMonths(1)) {
						if (_isNotCompletedSelected && !t.IsCompleted) {
							Tasks.Add(t);
						} else if (!_isNotCompletedSelected) {
							Tasks.Add(t);
						}
					}
				}
				break;
		}
	}
	private void CreateTask() {
		if (string.IsNullOrEmpty(TaskTitle)) {
			MessageBox.Show("Please fill out all fields.");
			return;
		} else {
			DbService.AddTaskAsync(new ToDoTask {
				Title = TaskTitle,
				Description = TaskDescription,
				DueDate = DueDate,
				CanvasLink = CanvasLink,
				FilePaths = FilePaths
			});
		}
		TaskTitle = "";
		TaskDescription = "";
		DueDate = DateTime.Now;
		CanvasLink = "";
		FilePaths.Clear();
		FileTitlesOC.Clear();
		FilterTasksAsync(_lastFilterOperation);
	}
	private void AddFilePath(object[] obj) {
		if (obj == null) return;
		ToDoTask t = (ToDoTask)obj[0];
		byte op = byte.Parse((string)obj[1]);

		OpenFileDialog dlg = new OpenFileDialog();
		dlg.Filter = "pdf files (*.pdf) |*.pdf;";
		dlg.Multiselect = true;
		dlg.ShowDialog();

		if (op == 0) {
			if (dlg.FileNames != null) {
				foreach (string file in dlg.FileNames) {
					InsertInOrder(t.FilePaths, file);
				}
			}
		} else if (op == 1) {
			if (dlg.FileNames != null) {
				foreach (string file in dlg.FileNames) {
					InsertInOrder(t.FilePathsStudy, file);
				}
			}
		}
		DbService.UpdateTaskAsync(t);
	}
	private void OpenFile(string path) => OpenOnNavigator(path);
	private void DeleteTask(ToDoTask task) {
		if (MessageBox.Show(CanvasLink, "Are you sure you want to delete this task?", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
			DbService.DeleteTaskAsync(task.Id);
			FilterTasksAsync(_lastFilterOperation);
		}
	}
	private async Task UpdateTaskCompletionAsync(ToDoTask task) {
		await DbService.UpdateTaskAsync(task);
		await FilterTasksAsync(_lastFilterOperation);
	}
	private void DeleteFilePath(string path) {
		FilePaths.Remove(FilePaths.ToList().Find(a => a.Contains(path)));
		FileTitlesOC.Remove(path);
	}
	private void DeleteTaskFilePath(object[] obj) {
		ToDoTask task = (ToDoTask)obj[0];
		string path = (string)obj[1];
		byte op = (byte)obj[2];
		switch (op) {
			case 0:
				task.FilePaths.Remove(path);
				break;

			case 1:
				task.FilePathsStudy.Remove(path);
				break;

			default:
				throw new ArgumentException();
		}
		DbService.UpdateTaskAsync(task);
	}
	private Process? OpenOnNavigator(string path) => Process.Start(new ProcessStartInfo() { FileName = (string)path, UseShellExecute = true });
	private void InsertInOrder(ObservableCollection<string> collection, string filePath) {
		int index = collection.TakeWhile(f => string.Compare(f, filePath) < 0).Count();
		collection.Insert(index, filePath);
	}
}