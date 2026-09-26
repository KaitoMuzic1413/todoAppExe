using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using System.Globalization;

namespace todoCS;

public partial class MainWindow : Window
{
    private readonly TodoTaskRepository repository = new();
    private readonly TextBox taskInput;
    private readonly TextBox searchInput;
    private readonly Button saveButton;
    private readonly TextBlock formLabel;
    private readonly TextBlock countLabel;
    private readonly TextBlock emptyLabel;
    private readonly ItemsControl taskItems;
    private TodoTask? editingTask;

    private static readonly IBrush Ink = Brush("#1F2B2A");
    private static readonly IBrush Muted = Brush("#5C6E69");
    private static readonly IBrush Primary = Brush("#1F8A7D");
    private static readonly IBrush PrimaryDark = Brush("#166E63");
    private static readonly IBrush Accent = Brush("#2A6B5F");
    private static readonly IBrush Surface = Brush("#FFFFFF");
    private static readonly IBrush SurfaceAlt = Brush("#F4F8F7");
    private static readonly IBrush Canvas = Brush("#EEF5F2");
    private static readonly IBrush BorderColor = Brush("#DDEAE5");
    private static readonly IBrush InputBackground = Brush("#F8FBFA");
    private static readonly IBrush EditBg = Brush("#EAF8F4");
    private static readonly IBrush EditHover = Brush("#D3EFEA");
    private static readonly IBrush DeleteBg = Brush("#FDECEC");
    private static readonly IBrush DeleteHover = Brush("#F7D3D2");
    private static readonly IBrush DeleteText = Brush("#B5534F");

    public MainWindow()
    {
        Title = "What you want to do ?";
        Width = 700;
        Height = 700;
        MinWidth = 520;
        MinHeight = 500;
        Background = Canvas;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        taskInput = new TextBox
        {
            PlaceholderText = "Add task ...",
            FontSize = 14,
            Padding = new Thickness(12, 10),
            VerticalContentAlignment = VerticalAlignment.Center,
            Background = InputBackground,
            BorderBrush = BorderColor,
            CornerRadius = new CornerRadius(8),
            Foreground = Ink
        };
        taskInput.PointerEntered += (_, _) => taskInput.PlaceholderForeground = Brush("#99000000");
        taskInput.PointerExited += (_, _) => taskInput.PlaceholderForeground = Muted;
        taskInput.KeyDown += (_, e) =>
        {
            if (e.Key == Key.Enter) { SaveTask(); e.Handled = true; }
            if (e.Key == Key.Escape && editingTask is not null) { CancelEdit(); e.Handled = true; }
        };

        searchInput = new TextBox
        {
            PlaceholderText = "Search tasks...",
            FontSize = 14,
            Padding = new Thickness(12, 9),
            VerticalContentAlignment = VerticalAlignment.Center,
            Background = InputBackground,
            BorderBrush = BorderColor,
            CornerRadius = new CornerRadius(8),
            Foreground = Ink
        };
        searchInput.PointerEntered += (_, _) => searchInput.PlaceholderForeground = Brush("#99000000");
        searchInput.PointerExited += (_, _) => searchInput.PlaceholderForeground = Muted;
        searchInput.TextChanged += (_, _) => RefreshTasks();

        saveButton = MakeButton("Add", Primary, Brushes.White, false, PrimaryDark);
        saveButton.Click += (_, _) => SaveTask();
        formLabel = Label("Add Task", 14, Ink, true);
        countLabel = Label("", 12, Muted);
        emptyLabel = Label("No tasks available.", 14, Muted);
        emptyLabel.HorizontalAlignment = HorizontalAlignment.Center;
        emptyLabel.Margin = new Thickness(8, 28);

        var header = new StackPanel { Spacing = 4 };
        header.Children.Add(Label("What do you want to do ?", 28, Ink, true));
        header.Children.Add(Label("Write down what you want to accomplish.", 14, Muted));

        var inputRow = new Grid { ColumnDefinitions = new ColumnDefinitions("*,Auto"), ColumnSpacing = 8 };
        Grid.SetColumn(taskInput, 0);
        Grid.SetColumn(saveButton, 1);
        inputRow.Children.Add(taskInput);
        inputRow.Children.Add(saveButton);

        taskItems = new ItemsControl
        {
            ItemTemplate = new FuncDataTemplate<TodoTask>((task, _) => task is null ? null : BuildTaskRow(task), true)
        };
        var listPanel = new StackPanel { Spacing = 8 };
        listPanel.Children.Add(emptyLabel);
        listPanel.Children.Add(taskItems);

        var listHeader = new Grid { ColumnDefinitions = new ColumnDefinitions("*,Auto") };
        var listTitle = Label("Task List", 17, Ink, true);
        Grid.SetColumn(listTitle, 0);
        Grid.SetColumn(countLabel, 1);
        listHeader.Children.Add(listTitle);
        listHeader.Children.Add(countLabel);

        var page = new Grid
        {
            RowDefinitions = new RowDefinitions("Auto,Auto,Auto,Auto,*"),
            RowSpacing = 16,
            Margin = new Thickness(28)
        };
        AddRow(page, header, 0);
        var form = new StackPanel { Spacing = 8 };
        form.Children.Add(formLabel);
        form.Children.Add(inputRow);
        AddRow(page, form, 1);
        AddRow(page, searchInput, 2);
        AddRow(page, listHeader, 3);
        AddRow(page, new ScrollViewer { Content = listPanel }, 4);
        Content = page;
        RefreshTasks();
    }

    private Control BuildTaskRow(TodoTask task)
    {
        var title = Label(task.Title, 14, task.IsCompleted ? Muted : Ink);
        title.TextWrapping = TextWrapping.Wrap;
        if (task.IsCompleted) title.TextDecorations = TextDecorations.Strikethrough;

        var date = Label(task.CreatedAt.ToString("dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("vi-VN")), 11, Muted);
        var check = new CheckBox { IsChecked = task.IsCompleted, VerticalAlignment = VerticalAlignment.Center };
        check.IsCheckedChanged += (_, _) =>
        {
            repository.SetCompleted(task, check.IsChecked == true);
            RefreshTasks();
        };
        var edit = MakeButton("Edit", EditBg, Accent, true, EditHover);
        edit.Click += (_, _) => BeginEdit(task);
        var delete = MakeButton("Delete", DeleteBg, DeleteText, true, DeleteHover);
        delete.Click += (_, _) => DeleteTask(task);

        var text = new StackPanel { Spacing = 4 };
        text.Children.Add(title);
        text.Children.Add(date);
        var actions = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
        actions.Children.Add(edit);
        actions.Children.Add(delete);
        var row = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto"), ColumnSpacing = 10 };
        Grid.SetColumn(check, 0);
        Grid.SetColumn(text, 1);
        Grid.SetColumn(actions, 2);
        row.Children.Add(check);
        row.Children.Add(text);
        row.Children.Add(actions);
        return new Border
        {
            Background = SurfaceAlt,
            BorderBrush = BorderColor,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(10),
            Padding = new Thickness(12),
            Child = row
        };
    }

    private void SaveTask()
    {
        var title = taskInput.Text?.Trim();
        if (string.IsNullOrWhiteSpace(title)) { taskInput.Focus(); return; }
        if (editingTask is null) repository.Add(title);
        else repository.Update(editingTask, title);
        CancelEdit();
        RefreshTasks();
        taskInput.Focus();
    }

    private void BeginEdit(TodoTask task)
    {
        editingTask = task;
        taskInput.Text = task.Title;
        formLabel.Text = "Edit Task (Esc to cancel)";
        saveButton.Content = "Save";
        taskInput.Focus();
        taskInput.SelectAll();
    }

    private void CancelEdit()
    {
        editingTask = null;
        taskInput.Clear();
        formLabel.Text = "Add Task";
        saveButton.Content = "Add";
    }

    private void DeleteTask(TodoTask task)
    {
        repository.Delete(task);
        if (editingTask?.Id == task.Id) CancelEdit();
        RefreshTasks();
    }

    private void RefreshTasks()
    {
        var matches = repository.Search(searchInput?.Text ?? "");
        taskItems.ItemsSource = matches;
        countLabel.Text = $"{matches.Count} tasks";
        emptyLabel.IsVisible = matches.Count == 0;
        emptyLabel.Text = repository.Tasks.Count == 0 ? "No tasks available." : "No tasks found.";
    }

    private static void AddRow(Grid grid, Control control, int row)
    {
        Grid.SetRow(control, row);
        grid.Children.Add(control);
    }

    private static TextBlock Label(string text, double size, IBrush color, bool bold = false) => new()
    {
        Text = text,
        FontSize = size,
        Foreground = color,
        FontWeight = bold ? FontWeight.SemiBold : FontWeight.Normal,
        VerticalAlignment = VerticalAlignment.Center
    };

    private static Button MakeButton(string text, IBrush background, IBrush foreground, bool compact = false, IBrush? hoverBackground = null)
    {
        var button = new Button
        {
            Content = text,
            Background = background,
            Foreground = foreground,
            BorderThickness = new Thickness(0),
            CornerRadius = new CornerRadius(8),
            Padding = compact ? new Thickness(10, 6) : new Thickness(16, 10),
            FontSize = 13,
            FontWeight = FontWeight.SemiBold,
            VerticalAlignment = VerticalAlignment.Center,
            Cursor = new Cursor(StandardCursorType.Hand)
        };

        if (hoverBackground is not null)
        {
            button.PointerEntered += (_, _) => button.Background = hoverBackground;
            button.PointerExited += (_, _) => button.Background = background;
        }

        return button;
    }

    private static SolidColorBrush Brush(string color) => new(Color.Parse(color));
}
