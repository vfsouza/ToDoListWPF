using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace ToDoList.Services.Converter {
	internal class ItemsControlIdentifierConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is ItemsControl itemsControl) {
				if (itemsControl.Name == "FilePaths") return (byte)0;
				else if (itemsControl.Name == "FilePathsStudy") return (byte)1;
			}
			return (byte)255;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
