using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using ToDoList.Core;
using ToDoList.Data;

namespace ToDoList.Services {
	public interface ITaskDBService {
		ToDoTask GetTask(int id);
		void AddTask(ToDoTask task);
		void UpdateTask(ToDoTask task);
		void DeleteTask(int id);
		ObservableCollection<ToDoTask> GetAllTasks();
		ObservableCollection<ToDoTask> GetCompletedTasks();
		ObservableCollection<ToDoTask> GetUncompletedTasks();
		ObservableCollection<ToDoTask> GetTaskByDueDate(DateTime dueDate);
		ObservableCollection<ToDoTask> GetTaskByDueDateLimit(DateTime dueDate);
	}

	public class TaskDBService : ObservableObject, ITaskDBService {
		private ToDoTaskContext dbContext;

		public ToDoTaskContext DbContext { 
			get => dbContext; 
			private set { 
				dbContext = value;
				OnPropertyChanged();
			} 
		}

		public TaskDBService(ToDoTaskContext dbContext) {
			DbContext = dbContext;
			DbContext.ToDoTasks.Load();
		}

		public void AddTask(ToDoTask task) {
			DbContext.ToDoTasks.Add(task);
			DbContext.SaveChanges();
		}

		public ToDoTask GetTask(int id) {
			return DbContext.ToDoTasks.Find(id) ?? throw new NullReferenceException();
		}

		public void DeleteTask(int id) {
			DbContext.ToDoTasks.Remove(GetTask(id));
			DbContext.SaveChanges();
		}

		public void UpdateTask(ToDoTask task) {
			DbContext.ToDoTasks.Update(task);
			DbContext.SaveChanges();
		}

		public ObservableCollection<ToDoTask> GetAllTasks() {
			return new ObservableCollection<ToDoTask>(DbContext.ToDoTasks.Local);
		}

		public ObservableCollection<ToDoTask> GetCompletedTasks() {
			return new ObservableCollection<ToDoTask>(DbContext.ToDoTasks.Local.Where(task => task.IsCompleted));
		}


		public ObservableCollection<ToDoTask> GetTaskByDueDate(DateTime dueDate) {
			return new ObservableCollection<ToDoTask>(DbContext.ToDoTasks.Local.Where(task => task.DueDate == dueDate));
		}

		public ObservableCollection<ToDoTask> GetTaskByDueDateLimit(DateTime dueDate) {
			return new ObservableCollection<ToDoTask>(DbContext.ToDoTasks.Local.Where(task => task.DueDate <= dueDate));
		}

		public ObservableCollection<ToDoTask> GetUncompletedTasks() {
			return new ObservableCollection<ToDoTask>(DbContext.ToDoTasks.Local.Where(task => !task.IsCompleted));
		}
	}
}
