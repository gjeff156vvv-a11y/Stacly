using System;
using Spectre.Console;

namespace TodoList
{
    static class ListMode
    {
         public static void Mode(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,List<Todo> todoList,ref AppState AppState,ref bool Running)
         {

            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.J: 
                    if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                        TodoManager.MoveDown(todoList,ref AppState); 
                    else 
                    {
                        if(AppState.SelectedIndex < todoList.Count - 1)
                            AppState.SelectedIndex++; 
                        else
                            AppState.SelectedIndex = 0;
                    }
                    break;
                case ConsoleKey.K: 
                    if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                        TodoManager.MoveUp(todoList,ref AppState);
                    else 
                    {
                        if(AppState.SelectedIndex > 0 )
                            AppState.SelectedIndex--; 
                        else
                            AppState.SelectedIndex = todoList.Count -1;
                    }
                    break;
                case ConsoleKey.L: TodoManager.PushNavigation(Navigation,ref AppState); break;
                case ConsoleKey.H: TodoManager.PopNavigation(Navigation,ref  AppState); break;
                case ConsoleKey.D: TodoManager.RemoveTodo(Navigation,todoList,AppState); break;
                case ConsoleKey.E: AppState.Mod = Mods.Edit; break;
                case ConsoleKey.Tab:AppState.IsExpanded = !AppState.IsExpanded; break;
                default:if (key.KeyChar == '/') {AppState.SearchBuffer = "";AppState.Mod = Mods.Search;}break;
                case ConsoleKey.N:
                        if(AppState.SearchBuffer == "" && AppState.FoundItems == null)
                            TodoManager.AddTodo(Navigation.Peek().Tasks,ref AppState);
                        else{
                           if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                               TodoManager.MoveUpSearch(AppState,todoList);
                           else TodoManager.MoveDownSearch(AppState,todoList);
                        }
                        break;
                case ConsoleKey.Spacebar: todoList[AppState.SelectedIndex].ChangeIsCompleted();TodoManager.Sort(todoList,AppState) ; break;
                case ConsoleKey.Escape:AppState.SearchBuffer = "";AppState.FoundItems = null;break;
                case ConsoleKey.Q: Running = false; return;
            }
         }
    }
}
