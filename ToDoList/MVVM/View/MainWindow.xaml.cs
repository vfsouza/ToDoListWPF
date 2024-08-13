using System.Windows;
using System.Windows.Input;
using ToDoList.MVVM.ViewModel;

namespace ToDoList.MVVM.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
	public MainWindow(MainViewModel mainViewModel) {
		InitializeComponent();
		DataContext = mainViewModel;
	}

	private void DragMoveOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
		DragMove();
	}

	private void MinimizeButtonClick(object sender, RoutedEventArgs e) {
		WindowState = WindowState.Minimized;
	}

	private void MaximizeButtonClick(object sender, RoutedEventArgs e) {
		
		if (WindowState == WindowState.Normal) {
			WindowState = WindowState.Maximized;
			WindowPanel.Margin = new Thickness(14, 6, 16, 0);
			WindowPanelGridRow.Height = new GridLength(60);
		} else {
			WindowState = WindowState.Normal;
			WindowPanel.Margin = new Thickness(0, 0, 8, 0);
			WindowPanelGridRow.Height = new GridLength(50);
		}
	}

	private void CloseButtonClick(object sender, RoutedEventArgs e) {
		Application.Current.Shutdown();
	}
}