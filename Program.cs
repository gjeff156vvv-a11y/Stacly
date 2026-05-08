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
            List<Todo>  todoList = Storage.ReadAll();

            var Navigation = new Stack<(List<Todo> Tasks,Todo? Parent)>();
            Navigation.Push((todoList,null));

            var Window = RenderWindow.CreatWindow();

            Console.Clear();

            AnsiConsole.Live(Window)
                .Start(ctx => 
                    {
                        while(AppState.Running)
                        {
                            switch(Mods)
                            {
                                case Mods.List: ListMode.Mode(Navigation,Navigation.Peek().Tasks,ref AppState,ref Mods); break;
                                case Mods.Edit: EditMode.Mode(Navigation.Peek().Tasks,ref AppState,ref Mods); break;
                                case Mods.Search: SearchMode.Mode(Navigation,Navigation.Peek().Tasks,ref AppState,ref Mods); break;
                            }
                            ctx.UpdateTarget(RenderWindow.Render(Window,Mods,Navigation.Peek().Tasks,AppState.SelectedIndex,Navigation,AppState.IsExpanded));
                            ctx.Refresh();
                        }
                    });
            Storage.SaveAll(todoList);
        }
    }
}
