using System;

namespace TodoList 
{
    static class InputMode
    {
        public static void Mode(ref AppState AppState,List<Todo> todoList)
        { 
            var inputIndex = todoList.FindIndex(t => t.Name == "" && !t.IsCompleted);

            if (inputIndex != -1) 
            {
                // Насильно ставим курсор на неё для отрисовки
                AppState.SelectedIndex = inputIndex; 
            }

            // Читаем ТОЛЬКО ОДНУ клавишу
            var key = Console.ReadKey(true); 

            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    // Пользователь закончил ввод
                    todoList[AppState.SelectedIndex].ChangeName(AppState.Buffer);
                    AppState.Buffer = ""; // Очищаем черновик
                    AppState.Mod = Mods.List; // Возвращаемся в обычный режим
                    break;

                case ConsoleKey.Backspace:
                    if (AppState.Buffer.Length > 0)
                    AppState.Buffer = AppState.Buffer[..^1];
                break;

                default:
                // Если это обычная буква или цифра
                if (!char.IsControl(key.KeyChar))
                {
                    AppState.Buffer += key.KeyChar;
                }
                break;
            }

        }
    }
}
