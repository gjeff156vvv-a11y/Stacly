using System;
using Spectre.Console;

namespace Stacly
{
    static class ListMode
    {
         public static void ProcessKey(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,List<Todo> todoList,ref AppCoordinator state,ref bool Running)
         {

            if(Console.KeyAvailable)
            {
            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.J: 
                    if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                        TodoManager.MoveDown(todoList,ref state); 
                    else 
                    {
                        if(state.SelectedIndex < todoList.Count - 1)
                            state.SelectedIndex++; 
                        else
                            state.SelectedIndex = 0;
                    }
                    break;

                case ConsoleKey.K: 
                    if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                        TodoManager.MoveUp(todoList,ref state);
                    else 
                    {
                        if(state.SelectedIndex > 0 )
                            state.SelectedIndex--; 
                        else
                            state.SelectedIndex = todoList.Count -1;
                    }
                    break;

                case ConsoleKey.N:
                        if(state.SearchBuffer == "" && state.FoundItems == null)
                        {
                            TodoManager.AddTodo(Navigation.Peek().Tasks,ref state);
                            state.IsDirty = true;
                        }
                        else
                        {
                           if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                               TodoManager.MoveUpSearch(state,todoList);
                           else TodoManager.MoveDownSearch(state,todoList);
                        }
                        break;

                default:
                    if (key.KeyChar == '/') 
                    {
                        state.SearchBuffer = "";
                        state.Mod = Mods.Search;
                    }
                    else state.SetMessage("Нет такой комманды",true);
                    break;

                case ConsoleKey.L: TodoManager.PushNavigation(Navigation,ref state); break;
                case ConsoleKey.H: TodoManager.PopNavigation(Navigation,ref  state); break;
                case ConsoleKey.D: state.Mod = Mods.ConfirmDelete;break;
                case ConsoleKey.E: state.Mod = Mods.Edit; state.IsDirty = true;break;
                case ConsoleKey.P: TodoManager.CyclePriority(todoList[state.SelectedIndex],state); state.IsDirty = true;break;
                case ConsoleKey.Tab:state.IsExpanded = !state.IsExpanded; break;
                case ConsoleKey.Spacebar: todoList[state.SelectedIndex].ChangeIsCompleted();state.IsDirty = true ; break;
                case ConsoleKey.Escape:state.SearchBuffer = "";state.FoundItems = null;break;
                case ConsoleKey.Q: Running = false; return;
            }
            }
         }
    }
}
