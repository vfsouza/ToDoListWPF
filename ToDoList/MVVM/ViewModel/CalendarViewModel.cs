using System.Collections.ObjectModel;
using ToDoList.Data;
using ToDoList.Services;

namespace ToDoList.MVVM.ViewModel;

public class CalendarViewModel : Core.ViewModel {
	private readonly ITaskDBService _dbService;
	private DateTime _selectedDate;
	private ObservableCollection<ToDoTask> _tasksForSelectedDate;

	public DateTime SelectedDate {
		get => _selectedDate;
		set {
			SetProperty(ref _selectedDate, value);
			UpdateTasksForSelectedDate();
		}
	}

	public ObservableCollection<ToDoTask> TasksForSelectedDate {
		get => _tasksForSelectedDate;
		set {
			SetProperty(ref _tasksForSelectedDate, value);
		}
	}

	public CalendarViewModel(ITaskDBService dbService) {
		_dbService = dbService;
		SelectedDate = DateTime.Today;
		TasksForSelectedDate = new ObservableCollection<ToDoTask>();
	}

	private async void UpdateTasksForSelectedDate() {
		var tasks = await _dbService.GetTaskByDueDateAsync(SelectedDate);
		TasksForSelectedDate = new ObservableCollection<ToDoTask>(tasks);
	}

	public async Task<ObservableCollection<ToDoTask>> GetTasksForDate(DateTime date) {
		return new ObservableCollection<ToDoTask>(await _dbService.GetTaskByDueDateAsync(date));
	}
}