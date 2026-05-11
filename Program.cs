using System;
using Spectre.Console;
using System.Collections.Generic;

namespace Stacly
{
    class Program
    {

        public static void Main(string[] str)
        {
            var state = new AppCoordinator();

            //состояние приложения
            bool Running = true;

            var Navigation = new Stack<(List<Todo> Tasks,Todo? Parent)>();
            Navigation.Push((state.RootList,null));

            var Window = RenderWindow.InitializeLayout();

            Console.Clear();


            AnsiConsole.Live(Window)
                .Start(ctx => 
                    {
                        var currentList = TodoManager.Sort(Navigation.Peek().Tasks,state);
                        ctx.UpdateTarget(RenderWindow.Render(Window,currentList,Navigation,state));
                        ctx.Refresh();

                        while(Running)
                        {
                            switch(state.Mod)
                            {
                                case Mods.List: ListMode.ProcessKey(Navigation,currentList,ref state,ref Running); break;
                                case Mods.Edit: EditMode.ProcessKey(currentList,ref state); break;
                                case Mods.Input: InputMode.ProcessKey(ref state,currentList); break;
                                case Mods.Search: SearchMode.ProcessKey(state);break;
                                case Mods.ConfirmDelete: ConfirmDeleteMode.ProcessKey(state,Navigation,currentList);break;
                            }
                            if(state.IsDirty == true) 
                            {
                                currentList = TodoManager.Sort(Navigation.Peek().Tasks,state);
                                state.IsDirty = false;
                                Storage.SaveAll(state.RootList);
                            }
                            ctx.UpdateTarget(RenderWindow.Render(Window,currentList,Navigation,state));
                            ctx.Refresh();

                            Thread.Sleep(50);
                        }
                    });
        }

    }
}
