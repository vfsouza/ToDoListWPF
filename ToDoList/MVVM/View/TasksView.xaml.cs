using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using ToDoList.Data;
using ToDoList.MVVM.ViewModel;

namespace ToDoList.MVVM.View;

public partial class TasksView : UserControl {
	public TasksView() {
		InitializeComponent();
		AddTaskViewControl.Height = 0;
	}

	private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
		AddTaskInnerControl.Measure(new Size(AddTaskViewControl.MaxWidth, AddTaskViewControl.MaxHeight));
		DoubleAnimation heightAnimation = new DoubleAnimation(AddTaskInnerControl.DesiredSize.Height, new Duration(TimeSpan.FromSeconds(0.2)));
		AddTaskViewControl.BeginAnimation(HeightProperty, heightAnimation);
	}

	private void StackPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
		DoubleAnimation heightAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.2)));
		AddTaskViewControl.BeginAnimation(HeightProperty, heightAnimation);
	}

	private void DeleteTask_Click(object sender, RoutedEventArgs e) {

		AddTaskInnerControl.Measure(new Size(AddTaskViewControl.MaxWidth, AddTaskViewControl.MaxHeight));
		DoubleAnimation heightAnimation = new DoubleAnimation(AddTaskInnerControl.DesiredSize.Height, new Duration(TimeSpan.FromSeconds(0.2)));
		AddTaskViewControl.BeginAnimation(HeightProperty, heightAnimation);
	}
}