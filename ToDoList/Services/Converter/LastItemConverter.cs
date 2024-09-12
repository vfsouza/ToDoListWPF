using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ToDoList.Services.Converter {
	public class LastItemConverter : IMultiValueConverter {
		public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture) {
			if (value[0] is int alternationIndex && value[1] is int itemCount) {
				return alternationIndex == itemCount - 1;
			}
			return false;
		}

		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
