using System;
using System.Linq;

namespace Stacly
{
    static class SearchMode
    {
        public static void ProcessKey(AppCoordinator AppCoordinator)
        {
            if(Console.KeyAvailable)
            {
            var Key = Console.ReadKey(true);

            switch (Key.Key)
            {
                case ConsoleKey.Enter:
                    // Пользователь закончил ввод
                    AppCoordinator.Mod = Mods.List; // Возвращаемся в обычный режим
                    break;

                case ConsoleKey.Backspace:
                    if (AppCoordinator.SearchBuffer.Length > 0)
                    AppCoordinator.SearchBuffer = AppCoordinator.SearchBuffer[..^1];
                    break;
                
                case ConsoleKey.Escape:
                    AppCoordinator.SearchBuffer = "";
                    AppCoordinator.Mod = Mods.List;
                    break;
                
                case ConsoleKey.Tab:
                    if (!string.IsNullOrEmpty(AppCoordinator.TagSuggestion))
                    {
                        var words = AppCoordinator.SearchBuffer.Split(' ');
                        var lastWord = words.Last();
                        // Заменяем неполный тег на полный
                        AppCoordinator.SearchBuffer = string.Join(" ", words.SkipLast(1)) + " #" + AppCoordinator.TagSuggestion;
                        AppCoordinator.TagSuggestion = ""; 
                    }
                    break;

                default:
                    // Если это обычная буква или цифра
                    if (!char.IsControl(Key.KeyChar))
                    {
                        AppCoordinator.SearchBuffer += Key.KeyChar;
                    }
                    break;
            }
            }
            // 1. Получаем "плоский" список всех задач (включая подзадачи)
            var allTasks = GetAllTasks(AppCoordinator.RootList);

            var queryRaw = AppCoordinator.SearchBuffer.ToLower().Trim();
            var parts = queryRaw.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            AppCoordinator.FoundItems = allTasks.Where(t => 
            {
                if (parts.Length == 0) return true;

                // Задача должна содержать ВСЕ слова из поиска
                return parts.All(part => {
                if (part.StartsWith("#"))
                {
                    // Поиск по тегам (убираем #)
                    var tagPart = part.TrimStart('#').ToLower();
                    var allTags = GetAllUniqueTags(AppCoordinator.RootList);

                    AppCoordinator.TagSuggestion = allTags.FirstOrDefault(t => t.ToLower().StartsWith(tagPart)) ?? "";
                    return t.Tags != null && t.Tags.Any(tag => tag.ToLower().Contains(tagPart));
                }
                AppCoordinator.TagSuggestion = "";
                // Обычный поиск в имени
                return t.Name.ToLower().Contains(part);
                });
            }).OrderBy(t => t.IsCompleted).ToList();
        }

        public static IEnumerable<Todo> GetAllTasks(IEnumerable<Todo> tasks)
        {
            foreach (var task in tasks)
            {
                yield return task;
                // Рекурсивно добавляем все подзадачи
                if (task.SubTasks != null)
                {
                    foreach (var sub in GetAllTasks(task.SubTasks))
                        yield return sub;
                }
            }
        }

        public static List<string> GetAllUniqueTags(List<Todo> rootList)
        {
            return GetAllTasks(rootList) // Используем твой метод обхода дерева
                .SelectMany(t => t.Tags)
                .Distinct()
                .ToList();
        }


    }
}
