using System;
using Spectre.Console;

namespace TodoList
{
    static class ListMode
    {
         public static void Mode(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,List<Todo> todoList,ref AppState AppState)
         {

            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.J: 
                    if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                        TodoManager.MoveDown(todoList,ref AppState); 
                    else if(AppState.SelectedIndex < todoList.Count - 1)
                        AppState.SelectedIndex++; 
                    break;
                case ConsoleKey.K: 
                    if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                        TodoManager.MoveUp(todoList,ref AppState);
                    else if (AppState.SelectedIndex > 0) 
                        AppState.SelectedIndex--; 
                    break;
                case ConsoleKey.L: TodoManager.PushNavigation(Navigation,ref AppState); break;
                case ConsoleKey.H: TodoManager.PopNavigation(Navigation,ref  AppState); break;
                case ConsoleKey.D: TodoManager.RemoveTodo(Navigation,todoList,AppState); break;
                case ConsoleKey.E: AppState.Mod = Mods.Edit; break;
                //case ConsoleKey.: Mods = Mods.Search; break;
                case ConsoleKey.Tab:AppState.IsExpanded = !AppState.IsExpanded; break;
                case ConsoleKey.N: TodoManager.AddTodo(Navigation.Peek().Tasks,ref AppState); break;
                case ConsoleKey.Spacebar: todoList[AppState.SelectedIndex].ChangeIsCompleted(); break;
                case ConsoleKey.Q: AppState.Running = false; return;
            }
         }
    }
}
