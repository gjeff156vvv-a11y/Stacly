using System;
using System.Linq;

namespace Stacly
{
    static class SearchMode
    {
        public static void ProcessKey(AppCoordinator state)
        {
            if(Console.KeyAvailable)
            {
            var Key = Console.ReadKey(true);

            switch (Key.Key)
            {
                case ConsoleKey.Enter:
                    // Пользователь закончил ввод
                    state.Mod = Mods.List; // Возвращаемся в обычный режим
                    break;

                case ConsoleKey.Backspace:
                    if (state.SearchBuffer.Length > 0)
                    state.SearchBuffer = state.SearchBuffer[..^1];
                    break;
                
                case ConsoleKey.Escape:
                    state.SearchBuffer = "";
                    state.Mod = Mods.List;
                    break;
                
                case ConsoleKey.Tab:
                    if (!string.IsNullOrEmpty(state.TagSuggestion))
                    {
                        var words = state.SearchBuffer.Split(' ');
                        var lastWord = words.Last();
                        // Заменяем неполный тег на полный
                        state.SearchBuffer = string.Join(" ", words.SkipLast(1)) + " #" + state.TagSuggestion;
                        state.TagSuggestion = ""; 
                    }
                    break;

                default:
                    // Если это обычная буква или цифра
                    if (!char.IsControl(Key.KeyChar))
                    {
                        state.SearchBuffer += Key.KeyChar;
                    }
                    break;
            }
            }
            // 1. Получаем "плоский" список всех задач (включая подзадачи)
            var allTasks = GetAllTasks(state.RootList);

            var queryRaw = state.SearchBuffer.ToLower().Trim();
            var parts = queryRaw.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            state.FoundItems = allTasks.Where(t => 
            {
                if (parts.Length == 0) return true;

                // Задача должна содержать ВСЕ слова из поиска
                return parts.All(part => {
                if (part.StartsWith("#"))
                {
                    // Поиск по тегам (убираем #)
                    var tagPart = part.TrimStart('#').ToLower();
                    var allTags = GetAllUniqueTags(state.RootList);

                    state.TagSuggestion = allTags.FirstOrDefault(t => t.ToLower().StartsWith(tagPart)) ?? "";
                    return t.Tags != null && t.Tags.Any(tag => tag.ToLower().Contains(tagPart));
                }
                state.TagSuggestion = "";
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
