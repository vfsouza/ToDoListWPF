using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ToDoList.Data;
using ToDoList.MVVM.ViewModel;

namespace ToDoList.MVVM.View;

public partial class TasksView : UserControl {
	private bool selected = false;
	private ListViewItem previousLvi = null;

	public TasksView() {
		InitializeComponent();
	}

	private void TaskListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		ListView lvi = sender as ListView;

		if (lvi != null) {
			object o = lvi.SelectedItem;
			if (o == null) return;
			ListViewItem lviItem = lvi.ItemContainerGenerator.ContainerFromItem(o) as ListViewItem;
			Border selectedBorder = FindByName("TaskDetailPanel", lviItem) as Border;
			StackPanel selectedStackPanel = FindByName("TaskDetailInnerPanel", lviItem) as StackPanel;

			if (selectedBorder != null && selectedStackPanel != null) {
				selectedStackPanel.Measure(new Size(selectedBorder.MaxWidth, selectedBorder.MaxHeight));
				DoubleAnimation heightAnimation = new DoubleAnimation(selectedStackPanel.DesiredSize.Height, new Duration(TimeSpan.FromSeconds(0.2)));
				selectedBorder.BeginAnimation(HeightProperty, heightAnimation);
			}

			if (previousLvi != null && previousLvi != lviItem) {
				Border previousBorder = FindByName("TaskDetailPanel", previousLvi) as Border;
				StackPanel previousStackPanel = FindByName("TaskDetailInnerPanel", previousLvi) as StackPanel;

				if (previousBorder != null && previousStackPanel != null) {
					DoubleAnimation heightAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.2)));
					previousBorder.BeginAnimation(HeightProperty, heightAnimation);
				}
			}

			previousLvi = lviItem;
		}
	}

	private FrameworkElement FindByName(string name, FrameworkElement root) {
		Stack<FrameworkElement> tree = new Stack<FrameworkElement>();
		tree.Push(root);

		while (tree.Count > 0) {
			FrameworkElement current = tree.Pop();
			if (current.Name == name)
				return current;

			int count = VisualTreeHelper.GetChildrenCount(current);
			for (int i = 0; i < count; ++i) {
				DependencyObject child = VisualTreeHelper.GetChild(current, i);
				if (child is FrameworkElement)
					tree.Push((FrameworkElement)child);
			}
		}

		return null;
	}
}