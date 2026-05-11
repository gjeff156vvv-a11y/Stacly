using System;
using Spectre.Console;

namespace Stacly
{
    static class EditMode
    {
         public static void ProcessKey(List<Todo> todoList,ref AppCoordinator state)
         {
            Todo selectTodo = todoList[state.SelectedIndex];
           
            if(Console.KeyAvailable)
            {
            // Читаем нажатие клавиши БЕЗ ожидания Enter
            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.R: TodoManager.RenameTodo(selectTodo,state); break;
                case ConsoleKey.D: TodoManager.WriteDescription(selectTodo,state); break;               
                case ConsoleKey.P: TodoManager.CyclePriority(selectTodo,state); break;
                case ConsoleKey.T: TodoManager.WriteTags(selectTodo,state); break;
                case ConsoleKey.Spacebar: selectTodo.ChangeIsCompleted(); break;
                case ConsoleKey.Escape: state.Mod = Mods.List; break;
            }
            }
         }
    }
}
