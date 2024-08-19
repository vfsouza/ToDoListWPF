using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Services;

namespace ToDoList.MVVM.ViewModel {
	public class ToDoTaskViewModel : Core.ViewModel {
		private ToDoTask _selectedTask;
		private  ITaskService _taskService;

		public ToDoTask SelectedTask {
			get => _selectedTask;
			set {
				_selectedTask = value;
				OnPropertyChanged();
			}
		}

		public ITaskService TaskService {
			get => _taskService;
			set {
				_taskService = value;
				OnPropertyChanged();
			}
		}
		public ToDoTaskViewModel(ITaskService taskService) {
			TaskService = taskService;
			taskService.ToDoTaskChanged += (s, e) => {
				SelectedTask = TaskService.CurrentTask;
			};
		}

		public void OnNavigated() {
		}
	}
}
