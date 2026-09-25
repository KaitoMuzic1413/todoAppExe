using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using System.Collections.ObjectModel;

namespace todoCS;

public partial class MainWindow : Window
{
    private ObservableCollection<string> tasks = new ObservableCollection<string>();

    public MainWindow()
    {
        Title = "Ứng dụng To-Do List (Pure C#)";
        Width = 400;
        Height = 500;

        // Ô nhập công việc
        var taskInput = new TextBox
        {
            PlaceholderText = "Nhập công việc...",
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        // Nút Thêm
        var addButton = new Button
        {
            Content = "Thêm",
            Margin = new Avalonia.Thickness(10, 0, 0, 0)
        };

        // Danh sách công việc
        var taskList = new ListBox
        {
            Height = 350,
            ItemsSource = tasks
        };

        // Bắt sự kiện Click cho nút Thêm bằng C#
        addButton.Click += (sender, e) =>
        {
            if (!string.IsNullOrWhiteSpace(taskInput.Text))
            {
                tasks.Add(taskInput.Text);
                taskInput.Text = string.Empty;
            }
        };

        // Khung chứa Ô nhập + Nút Thêm (Hàng ngang)
        var inputDock = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto")
        };
        Grid.SetColumn(taskInput, 0);
        Grid.SetColumn(addButton, 1);
        inputDock.Children.Add(taskInput);
        inputDock.Children.Add(addButton);

        // Khung tổng xếp theo hàng dọc
        var mainStack = new StackPanel
        {
            Margin = new Avalonia.Thickness(20),
            Spacing = 15
        };
        
        mainStack.Children.Add(new TextBlock 
        { 
            Text = "Danh Sách Công Việc", 
            FontSize = 20, 
            FontWeight = FontWeight.Bold 
        });
        mainStack.Children.Add(inputDock);
        mainStack.Children.Add(taskList);

        // Gán bố cục vào giao diện chính
        Content = mainStack;
    }
}