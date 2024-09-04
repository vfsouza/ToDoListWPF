using System.Collections.Specialized;
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
	private TasksViewModel tasksViewModel;

	public TasksView() {
		InitializeComponent();

		DataContextChanged += TasksView_DataContextChanged;
	}

	private void TasksView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
		tasksViewModel = DataContext as TasksViewModel;

		if (tasksViewModel != null) {
			tasksViewModel.Tasks.CollectionChanged += Tasks_CollectionChanged;
		}
	}

	private void Tasks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
		if (e.OldItems != null) {
			foreach (ToDoTask oldTask in e.OldItems) {
				oldTask.FilePaths.CollectionChanged -= FilePaths_CollectionChanged;
				oldTask.FilePathsStudy.CollectionChanged -= FilePaths_CollectionChanged;
			}
		}

		if (e.NewItems != null) {
			foreach (ToDoTask newTask in e.NewItems) {
				newTask.FilePaths.CollectionChanged += FilePaths_CollectionChanged;
				newTask.FilePathsStudy.CollectionChanged += FilePaths_CollectionChanged;
			}
		}
	}

	private async void FilePaths_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
		await Task.Delay(80);
		Border selectedBorder = FindByName("TaskDetailPanel", previousLvi) as Border;
		StackPanel selectedStackPanel = FindByName("TaskDetailInnerPanel", previousLvi) as StackPanel;
		selectedStackPanel.Measure(new Size(selectedBorder.MaxWidth, selectedBorder.MaxHeight));
		DoubleAnimation heightAnimation = new DoubleAnimation(selectedStackPanel.DesiredSize.Height, new Duration(TimeSpan.FromSeconds(0.2)));
		selectedBorder.BeginAnimation(HeightProperty, heightAnimation);
	}

	private void TaskListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		ListView lvi = sender as ListView;

		if (lvi != null) {
			object o = lvi.SelectedItem;
			if (o == null) return;
			ListViewItem lviItem = lvi.ItemContainerGenerator.ContainerFromItem(o) as ListViewItem;
			Border selectedBorder = FindByName("TaskDetailPanel", lviItem) as Border;
			Border selectedTaskBorder = FindByName("TaskPanelBorder", lviItem) as Border;
			StackPanel selectedStackPanel = FindByName("TaskDetailInnerPanel", lviItem) as StackPanel;

			if (selectedBorder != null && selectedStackPanel != null && selectedTaskBorder != null) {
				CornerRadiusAnimation cornerRadiusAnimation = new CornerRadiusAnimation {
					From = new CornerRadius(25,0,0,25),
					To = new CornerRadius(25,0,0,0),
					Duration = new Duration(TimeSpan.FromSeconds(0.15))
				};

				cornerRadiusAnimation.Completed += async (s, args) => {
					await Task.Delay(100);
					selectedStackPanel.Measure(new Size(selectedBorder.MaxWidth, selectedBorder.MaxHeight));
					DoubleAnimation heightAnimation = new DoubleAnimation(selectedStackPanel.DesiredSize.Height, new Duration(TimeSpan.FromSeconds(0.2)));
					selectedBorder.BeginAnimation(HeightProperty, heightAnimation);
				};

				selectedTaskBorder.BeginAnimation(Border.CornerRadiusProperty, cornerRadiusAnimation);
			}

			if (previousLvi != null && previousLvi != lviItem) {
				Border previousBorder = FindByName("TaskDetailPanel", previousLvi) as Border;
				Border previousTaskBorder = FindByName("TaskPanelBorder", previousLvi) as Border;
				StackPanel previousStackPanel = FindByName("TaskDetailInnerPanel", previousLvi) as StackPanel;

				if (previousBorder != null && previousStackPanel != null && previousTaskBorder != null) {
					DoubleAnimation heightAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.2)));

					heightAnimation.Completed += async (s, args) => {
						await Task.Delay(5);
						CornerRadiusAnimation cornerRadiusAnimation = new CornerRadiusAnimation {
							From = new CornerRadius(25, 0, 0, 0),
							To = new CornerRadius(25, 0, 0, 25),
							Duration = new Duration(TimeSpan.FromSeconds(0.15))
						};

						previousTaskBorder.BeginAnimation(Border.CornerRadiusProperty, cornerRadiusAnimation);
					};

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

class CornerRadiusAnimation : AnimationTimeline {
	public CornerRadius From {
		get { return (CornerRadius)GetValue(FromProperty); }
		set { SetValue(FromProperty, value); }
	}

	public static readonly DependencyProperty FromProperty =
		DependencyProperty.Register("From", typeof(CornerRadius), typeof(CornerRadiusAnimation));

	public CornerRadius To {
		get { return (CornerRadius)GetValue(ToProperty); }
		set { SetValue(ToProperty, value); }
	}

	public static readonly DependencyProperty ToProperty =
		DependencyProperty.Register("To", typeof(CornerRadius), typeof(CornerRadiusAnimation));

	public override Type TargetPropertyType => typeof(CornerRadius);

	public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock) {
		if (animationClock.CurrentProgress == null) {
			return From;
		}

		double progress = animationClock.CurrentProgress.Value;

		CornerRadius fromVal = From;
		CornerRadius toVal = To;

		return new CornerRadius(
			fromVal.TopLeft + (toVal.TopLeft - fromVal.TopLeft) * progress,
			fromVal.TopRight + (toVal.TopRight - fromVal.TopRight) * progress,
			fromVal.BottomRight + (toVal.BottomRight - fromVal.BottomRight) * progress,
			fromVal.BottomLeft + (toVal.BottomLeft - fromVal.BottomLeft) * progress);
	}

	protected override Freezable CreateInstanceCore() {
		return new CornerRadiusAnimation();
	}
}
