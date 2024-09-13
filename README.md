# ToDoList WPF Application

## The Development Journey of ToDoList

This project originated in February 2022, during my third semester at university, as a simple console application in C#. Now, in my seventh semester, with significantly broader knowledge in various areas of software development, I decided to enhance this initial concept, transforming it into an application with a responsive, intuitive, and aesthetically pleasing graphical interface.

The development process was a journey of self-directed learning. I utilized a variety of resources, including online tutorials, active participation in developer forums to solve specific challenges, and, primarily, Microsoft's official documentation for WPF. Additionally, in situations that demanded more complex or specific solutions, I made use of AI tools, such as Claude, to assist with intricate tasks that were difficult to resolve through conventional research.

The motivation behind this project stemmed from a personal need. After trying various task list applications available in the market, I realized that many lacked specific functionalities or didn't fully meet my user expectations. Thanks to my academic background and constant effort to acquire new knowledge and face daily challenges, I now possess the necessary skills to develop a personalized and efficient solution.

This project is a testament to the transformative power of education in information technology. The ability to create customized solutions for everyday problems is one of the greatest advantages of specializing in programming and IT. The vast field of possibilities that opens up before a developer is truly inspiring, and it was with this perspective that I conceived and implemented this application.

In essence, this ToDoList represents not just a productivity tool, but also a milestone in my professional development journey. It encapsulates technical learnings, creative problem-solving, and the satisfaction of creating a tailored solution for a real need.

## Technologies Used

- **WPF (Windows Presentation Foundation)**: For building the user interface.
- **.NET Core**: The application is built on the .NET Core framework.
- **C#**: The primary programming language used.
- **Entity Framework Core**: For database operations and ORM.
- **SQLite**: As the database engine.
- **Microsoft.Extensions.DependencyInjection**: For dependency injection.
- **Microsoft.Extensions.Hosting**: For application hosting and lifecycle management.

## Design Patterns and Architecture

- **MVVM (Model-View-ViewModel)**: The core architectural pattern used throughout the application.
- **Dependency Injection**: Used for loose coupling and better testability.
- **Repository Pattern**: Implemented through the `ITaskDBService` for data access abstraction.
- **Command Pattern**: Utilized through `RelayCommand` and `AsyncRelayCommand` for handling user actions.
- **Observer Pattern**: Implemented via data binding and `INotifyPropertyChanged`.

## Project Structure

### Views

1. **MainWindow**: The main application window.
2. **TasksView**: Displays the list of tasks and provides task management functionality.
3. **CalendarView**: A view for calendar-related functionality (appears to be a work in progress).
4. **CreateTaskView**: A view for creating new tasks.

### ViewModels

1. **MainViewModel**: Manages navigation between different views.
2. **TasksViewModel**: Handles task-related operations and data presentation.
3. **CalendarViewModel**: (Placeholder for future calendar functionality).

### Models

- **ToDoTask**: Represents a task in the application.

### Services

1. **ITaskDBService** and **TaskDBService**: Handle database operations for tasks.
2. **INavigationService** and **NavigationService**: Manage navigation between views.
3. **ITaskService** and **TaskService**: Manage the current task being worked on.

### Core

- **ObservableObject**: Base class for implementing `INotifyPropertyChanged`.
- **ViewModel**: Base class for ViewModels.
- **RelayCommand** and **AsyncRelayCommand**: Implementations of `ICommand` for handling user actions.

### Data

- **ToDoTaskContext**: DbContext for Entity Framework Core.

## Key Features

1. Task Creation and Management
2. Task Filtering (Today, Tomorrow, Week, Month)
3. Task Completion Tracking
4. File Attachment to Tasks
5. Due Date Management
6. Canvas Link Integration

## Styling and Themes

The application uses custom styles and themes defined in XAML resource dictionaries, providing a consistent and attractive user interface.

## Data Persistence

Tasks are stored in a SQLite database using Entity Framework Core, allowing for efficient data management and retrieval.

## Extensibility

The application is designed with extensibility in mind, using interfaces and dependency injection to allow for easy addition of new features or modification of existing ones.

## Getting Started

1. Clone the repository
2. Ensure you have .NET Core SDK installed
3. Open the solution in Visual Studio
4. Build and run the application

## Future Enhancements

- Implement full calendar functionality
- Add user authentication and multi-user support
- Integrate with external services (e.g., cloud storage for file attachments)
