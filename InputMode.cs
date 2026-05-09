using System;

namespace TodoList 
{
    static class InputMode
    {
        public static void Mode(ref AppState AppState,List<Todo> todoList)
        { 
            // Вместо FindIndex(t => t.Name == "")
            var inputIndex = todoList.IndexOf(AppState.EditingTodo);

            if (inputIndex != -1)
            {
                AppState.SelectedIndex = inputIndex;
            }

            // Читаем ТОЛЬКО ОДНУ клавишу
            var key = Console.ReadKey(true); 

            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    // Пользователь закончил ввод
                    switch(AppState.EditingField)
                    {
                        case "Name": todoList[AppState.SelectedIndex].ChangeName(AppState.Buffer);break;
                        case "Description": todoList[AppState.SelectedIndex].ChangeDescription(AppState.Buffer);break;
                        case "Tags": todoList[AppState.SelectedIndex].ChangeTags(AppState.Buffer);break;
                    }

                    AppState.Buffer = ""; // Очищаем черновик
                    AppState.EditingField = "";
                    AppState.EditingTodo = null;
                    AppState.Mod = Mods.List; // Возвращаемся в обычный режим
                    break;

                case ConsoleKey.Backspace:
                    if (AppState.Buffer.Length > 0)
                    AppState.Buffer = AppState.Buffer[..^1];
                    break;
                
                case ConsoleKey.Escape:
                    AppState.Buffer = "";
                    AppState.EditingField = "";
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
