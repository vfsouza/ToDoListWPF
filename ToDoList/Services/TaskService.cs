using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core;
using ToDoList.Data;

namespace ToDoList.Services {
	public interface ITaskService {
		ToDoTask CurrentTask { get; set; }
		public event EventHandler ToDoTaskChanged;
	}

	public class TaskService : ObservableObject, ITaskService {
		public event EventHandler ToDoTaskChanged;
		private ToDoTask currentTask;
		public ToDoTask CurrentTask {
			get => currentTask;
			set {
				currentTask = value;
				ToDoTaskChanged?.Invoke(this, EventArgs.Empty);
				OnPropertyChanged();
			}
		}
	}
}
