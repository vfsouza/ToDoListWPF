using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToDoList.MVVM.View
{
    public partial class CustomMessageBox : Window
    {
		public bool Result { get; private set; }

		public CustomMessageBox(string message) {
			InitializeComponent();
			MessageText.Text = message;
		}

		private void OkButton_Click(object sender, RoutedEventArgs e) {
			Result = true;
			Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e) {
			Result = false;
			Close();
		}

		public static bool Show(string message) {
			var messageBox = new CustomMessageBox(message);
			messageBox.ShowDialog();
			return messageBox.Result;
		}
	}
}
