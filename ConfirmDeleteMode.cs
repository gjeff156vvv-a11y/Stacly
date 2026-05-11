using System;

namespace Stacly
{
    static class ConfirmDeleteMode
    {
        public static void ProcessKey(AppCoordinator AppCoordinator,Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,List<Todo> todoList)
        {
            if(Console.KeyAvailable)
            {
            var Key = Console.ReadKey(true);

            switch(Key.Key)
            {
                case ConsoleKey.Y: 
                    TodoManager.RemoveTodo(Navigation,Navigation.Peek().Tasks,AppCoordinator);
                    AppCoordinator.IsDirty = true;
                    AppCoordinator.SetMessage("Удаление прошло успечно",false)
                    AppCoordinator.Mod = Mods.List;
                    break;
                case ConsoleKey.N: AppCoordinator.Mod = Mods.List;break;
            }
            }
        }
    }
}
