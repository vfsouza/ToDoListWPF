using System;
using System.Globalization;
using System.Windows.Data;

namespace ToDoList.Services.Converter {
	public class ByteConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (parameter == null) {
				return null;
			} else {
				return byte.Parse((string)parameter);
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}