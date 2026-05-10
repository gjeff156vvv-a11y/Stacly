using System;

namespace TodoList 
{
    static class InputMode
    {
        public static void Mode(ref AppState AppState,List<Todo> todoList)
        { 
            // Читаем ТОЛЬКО ОДНУ клавишу
            var key = Console.ReadKey(true); 

            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    // Пользователь закончил ввод
                    switch(AppState.EditingField)
                    {
                        case EditingField.Name:        AppState.EditingTodo.ChangeName(AppState.Buffer);break;
                        case EditingField.Description: AppState.EditingTodo.ChangeDescription(AppState.Buffer);break;
                        case EditingField.Tags:        AppState.EditingTodo.ChangeTags(AppState.Buffer);break;
                    }

                    AppState.Buffer = ""; // Очищаем черновик
                    AppState.EditingTodo = null;
                    AppState.Mod = Mods.List; // Возвращаемся в обычный режим
                    break;

                case ConsoleKey.Backspace:
                    if (AppState.Buffer.Length > 0)
                    AppState.Buffer = AppState.Buffer[..^1];
                    break;
                
                case ConsoleKey.Escape:
                    AppState.Buffer = "";
                    AppState.EditingTodo = null;
                    AppState.Mod = Mods.List;
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
