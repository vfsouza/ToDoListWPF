using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ToDoList.Extensions {
	public static class CalendarExtensions {
		public static CalendarDayButton GetCalendarDayButton(this Calendar calendar, DateTime date) {
			ArgumentNullException.ThrowIfNull(calendar);

			var month = calendar.FindName("PART_MonthView") as Grid;
			if (month == null)
				return null;

			foreach (var child in month.Children) {
				if (child is CalendarDayButton button && button.DataContext is DateTime buttonDate && buttonDate.Date == date.Date) {
					return button;
				}
			}

			return null;
		}
	}
}