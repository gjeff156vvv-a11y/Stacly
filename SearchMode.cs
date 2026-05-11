using System;
using System.Linq;

namespace Stacly
{
    static class SearchMode
    {
        public static void ProcessKey(List<Todo> todoList,AppCoordinator AppCoordinator)
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

                default:
                    // Если это обычная буква или цифра
                    if (!char.IsControl(Key.KeyChar))
                    {
                        AppCoordinator.SearchBuffer += Key.KeyChar;
                    }
                    break;
            }
            // 1. Получаем "плоский" список всех задач (включая подзадачи)
            var allTasks = GetAllTasks(todoList);

            var queryRaw = AppCoordinator.SearchBuffer.ToLower().Trim();
            var parts = queryRaw.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            AppCoordinator.FoundItems = allTasks.Where(t => {
                if (parts.Length == 0) return true;

                // Задача должна содержать ВСЕ слова из поиска
                return parts.All(part => {
                if (part.StartsWith("#")) {
                // Поиск по тегам (убираем #)
                var tagPart = part.TrimStart('#');
                return t.Tags != null && t.Tags.Any(tag => tag.ToLower().Contains(tagPart));
                }
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

    }
}
