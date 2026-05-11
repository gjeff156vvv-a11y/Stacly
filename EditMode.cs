using System;
using Spectre.Console;

namespace Stacly
{
    static class EditMode
    {
         public static void ProcessKey(List<Todo> todoList,ref AppCoordinator AppCoordinator)
         {
            Todo selectTodo = todoList[AppCoordinator.SelectedIndex];
           
            if(Console.KeyAvailable)
            {
            // Читаем нажатие клавиши БЕЗ ожидания Enter
            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.R: TodoManager.RenameTodo(selectTodo,AppCoordinator); break;
                case ConsoleKey.D: TodoManager.WriteDescription(selectTodo,AppCoordinator); break;               
                case ConsoleKey.P: TodoManager.CyclePriority(selectTodo,AppCoordinator); break;
                case ConsoleKey.T: TodoManager.WriteTags(selectTodo,AppCoordinator); break;
                case ConsoleKey.Spacebar: selectTodo.ChangeIsCompleted(); break;
                case ConsoleKey.Escape: AppCoordinator.Mod = Mods.List; break;
            }
            }
         }
    }
}
