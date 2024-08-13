using Microsoft.EntityFrameworkCore;

namespace ToDoList.Data {
	public class ToDoTaskContext : DbContext {
		public DbSet<ToDoTask> ToDoTasks { get; set; }

		public ToDoTaskContext(DbContextOptions<ToDoTaskContext> options) : base(options) { }
	}
}
