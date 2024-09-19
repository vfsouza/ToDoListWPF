using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoList.Core;
using ToDoList.MVVM.Model;

namespace ToDoList.Services {
	public interface ISettingsManager {
		Settings CurrentSettings { get; }
		void LoadSettings();
		void SaveSettings();
		void UpdateSetting(string key, string value);
		string GetCustomSetting(string key);
		string GetDefaultPath();
		void ExportSettings(string filePath);
		void ImportSettings(string filePath);
	}

	public class SettingsManager : ObservableObject, ISettingsManager {
		private Settings _currentSettings;
		private readonly string _settingsFilePath;

		public Settings CurrentSettings {
			get => _currentSettings;
			private set {
				_currentSettings = value;
				OnPropertyChanged();
			}
		}

		public SettingsManager() {
			_settingsFilePath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				"ToDoList",
				"settings.json"
			);
			LoadSettings();
		}

		public void LoadSettings() {
			if (File.Exists(_settingsFilePath)) {
				string json = File.ReadAllText(_settingsFilePath);
				CurrentSettings = JsonSerializer.Deserialize<Settings>(json);
			} else {
				CurrentSettings = new Settings();
				SaveSettings();
			}
		}

		public void SaveSettings() {
			string json = JsonSerializer.Serialize(CurrentSettings);
			Directory.CreateDirectory(Path.GetDirectoryName(_settingsFilePath));
			File.WriteAllText(_settingsFilePath, json);
		}

		public void UpdateSetting(string key, string value) {
			if (CurrentSettings.CustomSettings.ContainsKey(key)) {
				CurrentSettings.CustomSettings[key] = value;
			} else {
				CurrentSettings.CustomSettings.Add(key, value);
			}
			SaveSettings();
		}

		public string GetCustomSetting(string key) {
			return CurrentSettings.CustomSettings.TryGetValue(key, out string value) ? value : null;
		}

		public string GetDefaultPath() {
			return CurrentSettings.DefaultFolderPath;
		}

		public void ExportSettings(string filePath) {
			string json = JsonSerializer.Serialize(CurrentSettings);
			File.WriteAllText(filePath, json);
		}

		public void ImportSettings(string filePath) {
			if (File.Exists(filePath)) {
				string json = File.ReadAllText(filePath);
				CurrentSettings = JsonSerializer.Deserialize<Settings>(json);
				SaveSettings();
			}
		}
	}

}
