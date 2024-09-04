using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ToDoList.Data;

namespace ToDoList.Services.Converter {
	internal class DueDateTaskConverter : IMultiValueConverter {
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
			if (values[0] is ToDoTask task && values[1] != DependencyProperty.UnsetValue && values[1] is DateTime selectedDate) {
				if (task.DueDate != selectedDate) {
					return new { Task = task, SelectedDate = selectedDate };
				}
			}

			return null;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
