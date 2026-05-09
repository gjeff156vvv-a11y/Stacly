using System;
using Spectre.Console;
using System.Collections.Generic;

namespace TodoList
{
    class Program
    {

        public static void Main(string[] str)
        {
            var AppState = new AppState();

            //главный список для сохранения
            List<Todo>  RootList = Storage.ReadAll();

            var Navigation = new Stack<(List<Todo> Tasks,Todo? Parent)>();
            Navigation.Push((RootList,null));

            var Window = RenderWindow.CreatWindow();

            Console.Clear();

            AnsiConsole.Live(Window)
                .Start(ctx => 
                    {
                        var todo = Sort(Navigation.Peek().Tasks,AppState);
                        ctx.UpdateTarget(RenderWindow.Render(Window,todo,Navigation,AppState));
                        ctx.Refresh();

                        while(AppState.Running)
                        {
                            todo = Sort(Navigation.Peek().Tasks,AppState);
                            ctx.UpdateTarget(RenderWindow.Render(Window,todo,Navigation,AppState));
                            ctx.Refresh();
                            switch(AppState.Mod)
                            {
                                case Mods.List: ListMode.Mode(Navigation,todo,ref AppState); break;
                                case Mods.Edit: EditMode.Mode(todo,ref AppState); break;
                                case Mods.Input: InputMode.Mode(ref AppState,todo); break;
                                case Mods.Search: SearchMode.Mode(RootList,ref AppState);break;
                            }
                            ctx.UpdateTarget(RenderWindow.Render(Window,todo,Navigation,AppState));
                            ctx.Refresh();
                        }
                    });
            Storage.SaveAll(RootList);
        }

        private static List<Todo> Sort(List<Todo> list,AppState AppState)
        {

            var displayList = list
                .Where(t => string.IsNullOrEmpty(AppState.SearchInput) || 
                    t.Name.Contains(AppState.SearchInput, StringComparison.OrdinalIgnoreCase))
                .OrderBy(t => t.IsCompleted)        // Сначала невыполненные (false < true)
                .ThenByDescending(t => t.Priority)  // Потом важные
                .ToList();
            return displayList;
        }
    }
}
