using System;

namespace Stacly
{
    static class ConfirmDeleteMode
    {
        public static void Mode(AppState AppState,Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,List<Todo> todoList)
        {
            var Key = Console.ReadKey(true);

            switch(Key.Key)
            {
                case ConsoleKey.Y: 
                    TodoManager.RemoveTodo(Navigation,todoList,AppState);
                    AppState.Mod = Mods.List;
                    break;
                case ConsoleKey.N: AppState.Mod = Mods.List;break;
            }
        }
    }
}
