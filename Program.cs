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
            var Mods = new Mods();
            Mods = Mods.List;
            //главный список для сохранения
            List<Todo>  RootList = Storage.ReadAll();

            var Navigation = new Stack<(List<Todo> Tasks,Todo? Parent)>();
            Navigation.Push((RootList,null));

            var Window = RenderWindow.CreatWindow();

            Console.Clear();

            AnsiConsole.Live(Window)
                .Start(ctx => 
                    {
                        while(AppState.Running)
                        {
                            var todo = StartSort(Navigation.Peek().Tasks);
                            switch(Mods)
                            {
                                case Mods.List: ListMode.Mode(Navigation,todo,ref AppState,ref Mods); break;
                                case Mods.Edit: EditMode.Mode(todo,ref AppState,ref Mods); break;
                                case Mods.Search: SearchMode.Mode(Navigation,todo,ref AppState,ref Mods); break;
                            }
                            todo = StartSort(Navigation.Peek().Tasks);
                            ctx.UpdateTarget(RenderWindow.Render(Window,Mods,todo,AppState.SelectedIndex,Navigation,AppState.IsExpanded));
                            ctx.Refresh();
                        }
                    });
            Storage.SaveAll(RootList);
        }

        private static List<Todo> StartSort(List<Todo> list)
        {

            var displayList = list
                .OrderBy(t => t.IsCompleted)        // Сначала невыполненные (false < true)
                .ThenByDescending(t => t.Priority)  // Потом важные
                .ToList();

            return displayList;
        }
    }
}
