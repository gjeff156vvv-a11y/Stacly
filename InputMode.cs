using System;

namespace Stacly 
{
    static class InputMode
    {
        public static void ProcessKey(ref AppCoordinator state,List<Todo> todoList)
        { 
            if(Console.KeyAvailable)
            {
            // Читаем ТОЛЬКО ОДНУ клавишу
            var key = Console.ReadKey(true); 

            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    // Пользователь закончил ввод
                    switch(state.EditingField)
                    {
                        case EditingField.Name:        state.EditingTodo.ChangeName(state.InputBuffer);break;
                        case EditingField.Description: state.EditingTodo.ChangeDescription(state.InputBuffer);break;
                        case EditingField.Tags:        state.EditingTodo.ChangeTags(state.InputBuffer);break;
                    }

                    state.InputBuffer = ""; // Очищаем черновик
                    state.EditingTodo = null;
                    state.EditingField = EditingField.Default;
                    state.SetMessage("Изменение сохранины",false);
                    state.Mod = Mods.List; // Возвращаемся в обычный режим
                    break;

                case ConsoleKey.Backspace:
                    if (state.InputBuffer.Length > 0)
                    state.InputBuffer = state.InputBuffer[..^1];
                    break;
                
                case ConsoleKey.Escape:
                    state.InputBuffer = "";
                    state.EditingTodo = null;
                    state.Mod = Mods.List;
                    break;

                default:
                    // Если это обычная буква или цифра
                    if (!char.IsControl(key.KeyChar))
                    {
                        state.InputBuffer += key.KeyChar;
                    }
                    else state.SetMessage("Нет такой комманды",false);
                    break;


            }
            }

        }
    }
}
