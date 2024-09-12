using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using ToDoList.Core;
using ToDoList.Data;

namespace ToDoList.Services {
	public interface ITaskDBService {
		Task<ToDoTask> GetTaskAsync(int id);
		Task AddTaskAsync(ToDoTask task);
		Task UpdateTaskAsync(ToDoTask task);
		Task DeleteTaskAsync(int id);
		Task<ObservableCollection<ToDoTask>> GetAllTasksAsync();
		Task<ObservableCollection<ToDoTask>> GetCompletedTasksAsync();
		Task<ObservableCollection<ToDoTask>> GetUncompletedTasksAsync();
		Task<ObservableCollection<ToDoTask>> GetTaskByDueDateAsync(DateTime dueDate);
		Task<ObservableCollection<ToDoTask>> GetTaskByDueDateLimitAsync(DateTime dueDate);
	}

	public class TaskDBService : ObservableObject, ITaskDBService {
		private ToDoTaskContext dbContext;

        public ToDoTaskContext DbContext
        {
            get => dbContext;
            private set
            {
                dbContext = value;
                OnPropertyChanged();
            }
        }

        public TaskDBService(ToDoTaskContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task AddTaskAsync(ToDoTask task)
        {
            await DbContext.ToDoTasks.AddAsync(task);
            await DbContext.SaveChangesAsync();
        }

        public async Task<ToDoTask> GetTaskAsync(int id)
        {
            return await DbContext.ToDoTasks.FindAsync(id) ?? throw new NullReferenceException();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await GetTaskAsync(id);
            DbContext.ToDoTasks.Remove(task);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(ToDoTask task)
        {
            DbContext.ToDoTasks.Update(task);
            await DbContext.SaveChangesAsync();
        }

        public async Task<ObservableCollection<ToDoTask>> GetAllTasksAsync()
        {
            var tasks = await DbContext.ToDoTasks.OrderBy(t => t.DueDate).ToListAsync();
            return new ObservableCollection<ToDoTask>(tasks);
        }

        public async Task<ObservableCollection<ToDoTask>> GetCompletedTasksAsync()
        {
            var tasks = await DbContext.ToDoTasks.Where(task => task.IsCompleted).OrderBy(t => t.DueDate).ToListAsync();
            return new ObservableCollection<ToDoTask>(tasks);
        }

        public async Task<ObservableCollection<ToDoTask>> GetTaskByDueDateAsync(DateTime dueDate)
        {
            var tasks = await DbContext.ToDoTasks.Where(task => task.DueDate == dueDate).OrderBy(t => t.DueDate).ToListAsync();
            return new ObservableCollection<ToDoTask>(tasks);
        }

        public async Task<ObservableCollection<ToDoTask>> GetTaskByDueDateLimitAsync(DateTime dueDate)
        {
            var tasks = await DbContext.ToDoTasks.Where(task => task.DueDate <= dueDate).OrderBy(t => t.DueDate).ToListAsync();
            return new ObservableCollection<ToDoTask>(tasks);
        }

        public async Task<ObservableCollection<ToDoTask>> GetUncompletedTasksAsync()
        {
            var tasks = await DbContext.ToDoTasks.Where(task => !task.IsCompleted).OrderBy(t => t.DueDate).ToListAsync();
            return new ObservableCollection<ToDoTask>(tasks);
        }
	}
}
