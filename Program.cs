using System;
using Spectre.Console;
using System.Collections.Generic;

namespace Stacly
{
    class Program
    {

        public static void Main(string[] str)
        {
            var AppCoordinator = new AppCoordinator();

            //состояние приложения
            bool Running = true;

            //главный список для сохранения
            List<Todo>  RootList = Storage.ReadAll();

            var Navigation = new Stack<(List<Todo> Tasks,Todo? Parent)>();
            Navigation.Push((RootList,null));

            var Window = RenderWindow.InitializeLayout();

            Console.Clear();


            AnsiConsole.Live(Window)
                .Start(ctx => 
                    {
                        var currentList = TodoManager.Sort(Navigation.Peek().Tasks,AppCoordinator);
                        ctx.UpdateTarget(RenderWindow.Render(Window,currentList,Navigation,AppCoordinator));
                        ctx.Refresh();

                        while(Running)
                        {
                            currentList = TodoManager.Sort(Navigation.Peek().Tasks,AppCoordinator);
                            ctx.UpdateTarget(RenderWindow.Render(Window,currentList,Navigation,AppCoordinator));
                            ctx.Refresh();
                            switch(AppCoordinator.Mod)
                            {
                                case Mods.List: ListMode.ProcessKey(Navigation,currentList,ref AppCoordinator,ref Running); break;
                                case Mods.Edit: EditMode.ProcessKey(currentList,ref AppCoordinator); break;
                                case Mods.Input: InputMode.ProcessKey(ref AppCoordinator,currentList); break;
                                case Mods.Search: SearchMode.ProcessKey(RootList,AppCoordinator);break;
                                case Mods.ConfirmDelete: ConfirmDeleteMode.ProcessKey(AppCoordinator,Navigation,currentList);break;
                            }
                            ctx.UpdateTarget(RenderWindow.Render(Window,currentList,Navigation,AppCoordinator));
                            ctx.Refresh();
                        }
                    });
            Storage.SaveAll(RootList);
        }

    }
}
