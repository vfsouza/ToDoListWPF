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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoList.MVVM.View
{
    /// <summary>
    /// Interação lógica para CreateTaskView.xam
    /// </summary>
    public partial class CreateTaskView : UserControl
    {
        public CreateTaskView()
        {
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

		private async void TextBox_KeyDown(object sender, KeyEventArgs e) {
			await Task.Delay(100);
			AddTaskInnerControl.Measure(new Size(AddTaskViewControl.MaxWidth, AddTaskViewControl.MaxHeight));
			DoubleAnimation heightAnimation = new DoubleAnimation(AddTaskInnerControl.DesiredSize.Height, new Duration(TimeSpan.FromSeconds(0.2)));
			AddTaskViewControl.BeginAnimation(HeightProperty, heightAnimation);
		}
	}
}
