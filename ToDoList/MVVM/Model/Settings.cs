using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.MVVM.Model {
	public class Settings {
		public string DefaultFolderPath { get; set; }
		public string ThemeName { get; set; }
		public Dictionary<string, string> CustomSettings { get; set; }

		public Settings() {
			DefaultFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ToDoList");
			ThemeName = "Default";
			CustomSettings = new Dictionary<string, string>();
		}
	}
}
