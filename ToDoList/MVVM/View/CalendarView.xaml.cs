using System;
using System.Windows.Controls;
using ToDoList.MVVM.ViewModel;
using ToDoList.Extensions;

namespace ToDoList.MVVM.View {
	public partial class CalendarView : UserControl {
		private CalendarViewModel _viewModel;

		public CalendarView() {
			InitializeComponent();
			Loaded += CalendarView_Loaded;
		}

		private void CalendarView_Loaded(object sender, System.Windows.RoutedEventArgs e) {
			_viewModel = DataContext as CalendarViewModel;
			if (_viewModel != null) {
				MainCalendar.DisplayDateChanged += MainCalendar_DisplayDateChanged;
				UpdateCalendarDays(MainCalendar.DisplayDate);
			}
		}

		private void MainCalendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e) {
			if (e.AddedDate.HasValue) {
				UpdateCalendarDays(e.AddedDate.Value);
			}
		}

		private void UpdateCalendarDays(DateTime displayDate) {
			var firstDayOfMonth = new DateTime(displayDate.Year, displayDate.Month, 1);
			var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

			for (var date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1)) {
				var calendarButton = MainCalendar.GetCalendarDayButton(date);
				if (calendarButton != null) {
					var tasks = _viewModel.GetTasksForDate(date);
					calendarButton.DataContext = new { Date = date, DayTasks = tasks };
				}
			}
		}
	}
}