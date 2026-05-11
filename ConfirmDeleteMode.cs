using System;

namespace Stacly
{
    static class ConfirmDeleteMode
    {
        public static void ProcessKey(AppCoordinator state,Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,List<Todo> todoList)
        {
            if(Console.KeyAvailable)
            {
            var Key = Console.ReadKey(true);

            switch(Key.Key)
            {
                case ConsoleKey.Y: 
                    TodoManager.RemoveTodo(Navigation,Navigation.Peek().Tasks,state);
                    state.IsDirty = true;
                    state.SetMessage("Удаление прошло успечно",false); 
                    state.Mod = Mods.List;
                    break;
                case ConsoleKey.N: state.Mod = Mods.List;break;
            }
            }
        }
    }
}
