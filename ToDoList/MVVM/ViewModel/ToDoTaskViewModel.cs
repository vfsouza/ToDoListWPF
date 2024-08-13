using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Data;

namespace ToDoList.MVVM.ViewModel {
	class ToDoTaskViewModel : Core.ViewModel {
		private ToDoTask _selectedTask;

		public ToDoTask SelectedTask {
			get => _selectedTask; 
			set { 
				_selectedTask = value;
				OnPropertyChanged();
			}
		}

		public ToDoTaskViewModel(TasksViewModel tasksViewModel) {
			SelectedTask = tasksViewModel.SelectedTask;
		}
	}
}
