using System;

namespace Stacly 
{
    static class InputMode
    {
        public static void ProcessKey(ref AppCoordinator AppCoordinator,List<Todo> todoList)
        { 
            // Читаем ТОЛЬКО ОДНУ клавишу
            var key = Console.ReadKey(true); 

            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    // Пользователь закончил ввод
                    switch(AppCoordinator.EditingField)
                    {
                        case EditingField.Name:        AppCoordinator.EditingTodo.ChangeName(AppCoordinator.InputBuffer);break;
                        case EditingField.Description: AppCoordinator.EditingTodo.ChangeDescription(AppCoordinator.InputBuffer);break;
                        case EditingField.Tags:        AppCoordinator.EditingTodo.ChangeTags(AppCoordinator.InputBuffer);break;
                    }

                    AppCoordinator.InputBuffer = ""; // Очищаем черновик
                    AppCoordinator.EditingTodo = null;
                    AppCoordinator.Mod = Mods.List; // Возвращаемся в обычный режим
                    break;

                case ConsoleKey.Backspace:
                    if (AppCoordinator.InputBuffer.Length > 0)
                    AppCoordinator.InputBuffer = AppCoordinator.InputBuffer[..^1];
                    break;
                
                case ConsoleKey.Escape:
                    AppCoordinator.InputBuffer = "";
                    AppCoordinator.EditingTodo = null;
                    AppCoordinator.Mod = Mods.List;
                    break;

                default:
                    // Если это обычная буква или цифра
                    if (!char.IsControl(key.KeyChar))
                    {
                        AppCoordinator.InputBuffer += key.KeyChar;
            }
                    break;


            }

        }
    }
}
