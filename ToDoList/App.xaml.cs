using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoList.Core;
using ToDoList.Data;
using ToDoList.MVVM.View;
using ToDoList.MVVM.ViewModel;
using ToDoList.Services;

namespace ToDoList;

public partial class App : Application {
	private IHost _host;

	public App() {
		_host = CreateHostBuilder().Build();
		_host.Start();

		using (var scope = _host.Services.CreateScope())
		using (var dbContext = scope.ServiceProvider.GetRequiredService<ToDoTaskContext>()) {
			dbContext.Database.EnsureCreated();
			PopulateDatabase(dbContext);
		}

	}

	private void PopulateDatabase(ToDoTaskContext context) {
		if (context.ToDoTasks.Any()) return;
		context.ToDoTasks.AddRange(new List<ToDoTask> {
			new ToDoTask {
				Title = "Task 1", 
				Description = "Description 1", 
				DueDate = DateTime.Now.AddDays(1),
				FilePaths = new List<string> {
					"abc",
					"def",
					"ghf"
				}
			},
			new ToDoTask { 
				Title = "Task 2",
				Description = "Description 2", 
				DueDate = DateTime.Now.AddDays(2),
				FilePaths = new List<string> {
					"abc",
					"def",
					"ghf"
				}
			},
			new ToDoTask { 
				Title = "Task 3", 
				Description = "Description 3",
				DueDate = DateTime.Now.AddDays(3),
				FilePaths = new List<string> {
					"abc",
					"def",
					"ghf"
				}
			},
			new ToDoTask {
				Title = "Task 4",
				Description = "Description 4",
				DueDate = DateTime.Now.AddDays(1),
				FilePaths = new List<string> {
					"abc",
					"def",
					"ghf"
				}
			},
			new ToDoTask {
				Title = "Task 5",
				Description = "Description 5",
				DueDate = DateTime.Now.AddDays(2),
				FilePaths = new List<string> {
					"abc",
					"def",
					"ghf"
				}
			},
			new ToDoTask {
				Title = "Task 6",
				Description = "Description 6",
				DueDate = DateTime.Now.AddDays(3),
				FilePaths = new List<string> {
					"abc",
					"def",
					"ghf"
				}
			},
			new ToDoTask {
				Title = "Task 7",
				Description = "Description 7",
				DueDate = DateTime.Now.AddDays(1),
				FilePaths = new List<string> {
					"abc",
					"def",
					"ghf"
				}
			},
			new ToDoTask {
				Title = "Task 8",
				Description = "Description 8",
				DueDate = DateTime.Now.AddDays(2),
				FilePaths = new List<string> {
					"abc",
					"def",
					"ghf"
				}
			},
			new ToDoTask {
				Title = "Task 9",
				Description = "Description 9",
				DueDate = DateTime.Now.AddDays(3),
				FilePaths = new List<string> {
					"abc",
					"def",
					"ghf"
				}
			}
		});
		context.SaveChanges();
	}

	public static IHostBuilder CreateHostBuilder() =>
		Host.CreateDefaultBuilder()
		.ConfigureServices((hostContext, services) => {
			services.AddSingleton<MainWindow>();

			services.AddSingleton<MainViewModel>();
			services.AddSingleton<ToDoTaskViewModel>();
			services.AddSingleton<TasksViewModel>();
			services.AddSingleton<CalendarViewModel>();

			services.AddSingleton<ITaskDBService, TaskDBService>();
			services.AddSingleton<INavigationService, NavigationService>();
			services.AddSingleton<ITaskService, TaskService>();

			services.AddSingleton<Func<Type, ViewModel>>(provider => viewModelType => (ViewModel) provider.GetRequiredService(viewModelType));

			string baseDir = AppDomain.CurrentDomain.BaseDirectory;
			string projectDir = baseDir.Substring(0, baseDir.IndexOf("bin"));
			services.AddDbContext<ToDoTaskContext>(options => {
				options.UseSqlite($"Data Source={projectDir}Data\\ToDoTasks.db");
				options.UseLazyLoadingProxies();
			});
		});

	protected override void OnStartup(StartupEventArgs e) {
		var mainWindow = _host.Services.GetRequiredService<MainWindow>();
		_host.Services.GetRequiredService<ToDoTaskViewModel>();
		mainWindow.Show();
		base.OnStartup(e);
	}
}