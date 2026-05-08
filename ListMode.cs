using System;
using Spectre.Console;

namespace TodoList
{
    static class ListMode
    {
         public static void Mode(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,List<Todo> todoList,ref AppState AppState,ref Mods Mods)
         {

            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.J: 
                    if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                        TodoManager.MoveDown(todoList,ref AppState.SelectedIndex); 
                    else if(AppState.SelectedIndex < todoList.Count - 1)
                        AppState.SelectedIndex++; 
                    break;
                case ConsoleKey.K: 
                    if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                        TodoManager.MoveUp(todoList,ref AppState.SelectedIndex);
                    else if (AppState.SelectedIndex > 0) 
                        AppState.SelectedIndex--; 
                    break;
                case ConsoleKey.L: TodoManager.PushNavigation(Navigation,ref AppState.SelectedIndex); break;
                case ConsoleKey.H: TodoManager.PopNavigation(Navigation,ref  AppState.SelectedIndex); break;
                case ConsoleKey.D: TodoManager.RemoveTodo(todoList,AppState.SelectedIndex); break;
                case ConsoleKey.E: Mods = Mods.Edit; break;
                case ConsoleKey.F: Mods = Mods.Search; break;
                case ConsoleKey.Tab:AppState.IsExpanded = !AppState.IsExpanded; break;
                //case ConsoleKey.N: TodoManager.AddTodo(todoList); break;
                case ConsoleKey.Spacebar: todoList[AppState.SelectedIndex].ChangeIsCompleted(); break;
                case ConsoleKey.Q: AppState.Running = false; return;
            }
         }
    }
}
