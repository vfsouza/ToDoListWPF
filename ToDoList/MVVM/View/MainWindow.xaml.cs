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

	private void CloseButtonClick(object sender, RoutedEventArgs e) {
		Application.Current.Shutdown();
	}
}