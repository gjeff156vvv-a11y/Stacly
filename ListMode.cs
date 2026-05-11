using System;
using Spectre.Console;

namespace Stacly
{
    static class ListMode
    {
         public static void ProcessKey(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,List<Todo> todoList,ref AppCoordinator AppCoordinator,ref bool Running)
         {

            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.J: 
                    if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                        TodoManager.MoveDown(todoList,ref AppCoordinator); 
                    else 
                    {
                        if(AppCoordinator.SelectedIndex < todoList.Count - 1)
                            AppCoordinator.SelectedIndex++; 
                        else
                            AppCoordinator.SelectedIndex = 0;
                    }
                    break;

                case ConsoleKey.K: 
                    if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                        TodoManager.MoveUp(todoList,ref AppCoordinator);
                    else 
                    {
                        if(AppCoordinator.SelectedIndex > 0 )
                            AppCoordinator.SelectedIndex--; 
                        else
                            AppCoordinator.SelectedIndex = todoList.Count -1;
                    }
                    break;

                case ConsoleKey.N:
                        if(AppCoordinator.SearchBuffer == "" && AppCoordinator.FoundItems == null)
                        {
                            TodoManager.AddTodo(Navigation.Peek().Tasks,ref AppCoordinator);
                            AppCoordinator.IsDirty = true;
                        }
                        else
                        {
                           if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                               TodoManager.MoveUpSearch(AppCoordinator,todoList);
                           else TodoManager.MoveDownSearch(AppCoordinator,todoList);
                        }
                        break;

                case ConsoleKey.L: TodoManager.PushNavigation(Navigation,ref AppCoordinator); break;
                case ConsoleKey.H: TodoManager.PopNavigation(Navigation,ref  AppCoordinator); break;
                case ConsoleKey.D: AppCoordinator.Mod = Mods.ConfirmDelete;break;
                case ConsoleKey.E: AppCoordinator.Mod = Mods.Edit; AppCoordinator.IsDirty = true;break;
                case ConsoleKey.P: TodoManager.CyclePriority(todoList[AppCoordinator.SelectedIndex]); AppCoordinator.IsDirty = true;break;
                case ConsoleKey.Tab:AppCoordinator.IsExpanded = !AppCoordinator.IsExpanded; break;
                default:if (key.KeyChar == '/') {AppCoordinator.SearchBuffer = "";AppCoordinator.Mod = Mods.Search;}break;
                case ConsoleKey.Spacebar: todoList[AppCoordinator.SelectedIndex].ChangeIsCompleted();AppCoordinator.IsDirty = true ; break;
                case ConsoleKey.Escape:AppCoordinator.SearchBuffer = "";AppCoordinator.FoundItems = null;break;
                case ConsoleKey.Q: Running = false; return;
            }
         }
    }
}
