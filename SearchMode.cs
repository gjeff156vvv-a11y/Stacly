using System;
using System.Linq;

namespace TodoList
{
    static class SearchMode
    {
        public static void Mode(List<Todo> todoList,ref AppState AppState)
        {
            var Key = Console.ReadKey(true);

            switch (Key.Key)
            {
                case ConsoleKey.Enter:
                    // Пользователь закончил ввод
                    AppState.SearchInput = ""; // Очищаем черновик
                    AppState.Mod = Mods.List; // Возвращаемся в обычный режим
                    break;

                case ConsoleKey.Backspace:
                    if (AppState.SearchInput.Length > 0)
                    AppState.SearchInput = AppState.SearchInput[..^1];
                    break;
                
                case ConsoleKey.Escape:
                    AppState.SearchInput = "";
                    AppState.Mod = Mods.List;
                    break;

                default:
                    // Если это обычная буква или цифра
                    if (!char.IsControl(Key.KeyChar))
                    {
                        AppState.SearchInput += Key.KeyChar;
                    }
                    break;


            }
        }
    }
}
