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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoList.Data;
using ToDoList.MVVM.ViewModel;

namespace ToDoList.MVVM.View {
	/// <summary>
	/// Interação lógica para ToDoTaskView.xam
	/// </summary>
	public partial class ToDoTaskView : UserControl {
        public ToDoTaskView() {
			InitializeComponent();
		}
	}
}
