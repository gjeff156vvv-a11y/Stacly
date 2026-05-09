using System;
using Spectre.Console;

namespace TodoList
{
    static class EditMode
    {
         public static void Mode(List<Todo> todoList,ref AppState AppState)
         {
            Todo selectTodo = todoList[AppState.SelectedIndex];
           
            // Читаем нажатие клавиши БЕЗ ожидания Enter
            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.R: TodoManager.RenameTodo(selectTodo,AppState); break;
                case ConsoleKey.D: TodoManager.WriteDescription(selectTodo,AppState); break;               
                case ConsoleKey.P: TodoManager.CyclePriority(selectTodo); break;
                case ConsoleKey.T: TodoManager.WriteTags(selectTodo,AppState); break;
                case ConsoleKey.Spacebar: selectTodo.ChangeIsCompleted(); break;
                case ConsoleKey.Escape: AppState.Mod = Mods.List; break;
            }
         }
    }
}
