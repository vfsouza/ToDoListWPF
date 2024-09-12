using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core;

namespace ToDoList.Data {
	public class ToDoTask : ObservableObject {
		[NotMapped]
		private bool _isEditing = false;
		[NotMapped]
		private string? _title;
		[NotMapped]
		private string? _description;
		[NotMapped]
		private string? _canvasLink;
		[NotMapped]
		private DateTime _dueDate;
		[NotMapped]
		private bool _isCompleted;

		public int Id { get; set; }
		public string? Title {
			get => _title;
			set {
				_title = value;
				OnPropertyChanged();
			}
		}
		public string? Description {
			get => _description;
			set {
				_description = value;
				OnPropertyChanged();
			}
		}
		public string? CanvasLink {
			get => _canvasLink;
			set {
				_canvasLink = value;
				OnPropertyChanged();
			}
		}
		public ObservableCollection<string>? FilePaths { get; set; } = new ObservableCollection<string>();
		public ObservableCollection<string>? FilePathsStudy { get; set; } = new ObservableCollection<string>();
		public DateTime DueDate {
			get => _dueDate;
			set {
				_dueDate = value;
				OnPropertyChanged();
			}
		}
		public bool IsCompleted {
			get => _isCompleted;
			set {
				_isCompleted = value;
				OnPropertyChanged();
			}
		}
		[NotMapped]
		public bool IsEditing {
			get => _isEditing;
			set {
				_isEditing = value;
				OnPropertyChanged();
			}
		}
		public ToDoTask() { }

		public string GetCompleteDueDate() {
			return DueDate.ToString("dd/MM/yyyy");
		}
	}
}
