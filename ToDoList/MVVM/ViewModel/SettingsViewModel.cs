using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoList.Core;
using ToDoList.Services;

namespace ToDoList.MVVM.ViewModel {
	class SettingsViewModel : Core.ViewModel {
		private readonly ISettingsManager _settingsManager;

		public string DefaultFolderPath {
			get => _settingsManager.CurrentSettings.DefaultFolderPath;
			set {
				_settingsManager.CurrentSettings.DefaultFolderPath = value;
				_settingsManager.SaveSettings();
				OnPropertyChanged();
			}
		}

		public string ThemeName {
			get => _settingsManager.CurrentSettings.ThemeName;
			set {
				_settingsManager.CurrentSettings.ThemeName = value;
				_settingsManager.SaveSettings();
				OnPropertyChanged();
			}
		}

		public ICommand ChooseFolderCommand { get; }
		public ICommand ExportSettingsCommand { get; }
		public ICommand ImportSettingsCommand { get; }

		public SettingsViewModel(ISettingsManager settingsManager) {
			_settingsManager = settingsManager;

			ChooseFolderCommand = new RelayCommand(ChooseFolder);
			ExportSettingsCommand = new RelayCommand(ExportSettings);
			ImportSettingsCommand = new RelayCommand(ImportSettings);
		}

		private void ChooseFolder() {
			var dialog = new OpenFolderDialog();
			dialog.Multiselect = false;

			if (dialog.ShowDialog() == true) {
				DefaultFolderPath = dialog.FolderName;
			}
		}

		private void ExportSettings() {
			SaveFileDialog saveFileDialog = new SaveFileDialog {
				Filter = "JSON files (*.json)|*.json",
				DefaultExt = "json",
				AddExtension = true
			};

			if (saveFileDialog.ShowDialog() == true) {
				_settingsManager.ExportSettings(saveFileDialog.FileName);
			}
		}

		private void ImportSettings() {
			OpenFileDialog openFileDialog = new OpenFileDialog {
				Filter = "JSON files (*.json)|*.json",
				DefaultExt = "json"
			};

			if (openFileDialog.ShowDialog() == true) {
				_settingsManager.ImportSettings(openFileDialog.FileName);
				OnPropertyChanged(nameof(DefaultFolderPath));
				OnPropertyChanged(nameof(ThemeName));
			}
		}
	}
}
