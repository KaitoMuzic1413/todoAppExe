using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace todoCS;

/// <summary>Stores tasks locally and provides basic task operations.</summary>
public sealed class TodoTaskRepository
{
    private readonly string dataFile = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "TodoCS", "tasks.json");

    public TodoTaskRepository() => Load();

    public List<TodoTask> Tasks { get; } = new();

    public void Add(string title)
    {
        Tasks.Insert(0, new TodoTask { Title = title.Trim(), CreatedAt = DateTime.Now });
        Save();
    }

    public void Update(TodoTask task, string title)
    {
        task.Title = title.Trim();
        Save();
    }

    public void Delete(TodoTask task)
    {
        Tasks.Remove(task);
        Save();
    }

    public void SetCompleted(TodoTask task, bool completed)
    {
        task.IsCompleted = completed;
        Save();
    }

    public IReadOnlyList<TodoTask> Search(string query)
    {
        var keyword = Normalize(query);
        return string.IsNullOrEmpty(keyword)
            ? Tasks
            : Tasks.Where(task => Normalize(task.Title).Contains(keyword, StringComparison.Ordinal)).ToList();
    }

    private static string Normalize(string text)
    {
        var decomposed = text.Trim().ToLowerInvariant().Replace('đ', 'd').Normalize(NormalizationForm.FormD);
        var result = new StringBuilder(decomposed.Length);
        foreach (var character in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark) continue;
            result.Append(char.IsLetterOrDigit(character) ? character : ' ');
        }
        return result.ToString().Normalize(NormalizationForm.FormC).Trim();
    }

    private void Load()
    {
        try
        {
            if (!File.Exists(dataFile)) return;
            var tasks = JsonSerializer.Deserialize<List<TodoTask>>(File.ReadAllText(dataFile));
            if (tasks is not null) Tasks.AddRange(tasks.Where(task => !string.IsNullOrWhiteSpace(task.Title)));
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
        catch (JsonException) { }
    }

    private void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataFile)!);
            File.WriteAllText(dataFile, JsonSerializer.Serialize(Tasks, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
    }
}
