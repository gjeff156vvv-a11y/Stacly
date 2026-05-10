using System;
using Spectre.Console;
using System.Collections.Generic;

namespace Stacly
{
    class Program
    {

        public static void Main(string[] str)
        {
            var AppState = new AppState();

            //состояние приложения
            bool Running = true;

            //главный список для сохранения
            List<Todo>  RootList = Storage.ReadAll();

            var Navigation = new Stack<(List<Todo> Tasks,Todo? Parent)>();
            Navigation.Push((RootList,null));

            var Window = RenderWindow.CreatWindow();

            Console.Clear();


            AnsiConsole.Live(Window)
                .Start(ctx => 
                    {
                        var todo = TodoManager.Sort(Navigation.Peek().Tasks,AppState);
                        ctx.UpdateTarget(RenderWindow.Render(Window,todo,Navigation,AppState));
                        ctx.Refresh();

                        while(Running)
                        {
                            todo = TodoManager.Sort(Navigation.Peek().Tasks,AppState);
                            ctx.UpdateTarget(RenderWindow.Render(Window,todo,Navigation,AppState));
                            ctx.Refresh();
                            switch(AppState.Mod)
                            {
                                case Mods.List: ListMode.Mode(Navigation,todo,ref AppState,ref Running); break;
                                case Mods.Edit: EditMode.Mode(todo,ref AppState); break;
                                case Mods.Input: InputMode.Mode(ref AppState,todo); break;
                                case Mods.Search: SearchMode.Mode(RootList,AppState);break;
                                case Mods.ConfirmDelete: ConfirmDeleteMode.Mode(AppState,Navigation,todo);break;
                            }
                            ctx.UpdateTarget(RenderWindow.Render(Window,todo,Navigation,AppState));
                            ctx.Refresh();
                        }
                    });
            Storage.SaveAll(RootList);
        }

    }
}
