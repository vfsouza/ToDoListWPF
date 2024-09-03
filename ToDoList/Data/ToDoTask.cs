using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Data {
	public class ToDoTask {
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? CanvasLink { get; set; }
		public List<string>? FilePaths { get; set; }
		public List<string>? FilePathsStudy { get; set; }
		public DateTime DueDate { get; set; }
		public bool IsCompleted { get; set; } = false;
		public ToDoTask() { }

		public string GetCompleteDueDate() {
			return DueDate.ToString("dd/MM/yyyy");
		}
	}
}
