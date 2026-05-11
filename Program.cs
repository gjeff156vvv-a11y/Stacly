using System;
using Spectre.Console;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices;

namespace Stacly
{
    class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_MAXIMIZE = 3; // Код команды "Развернуть"


        public static void Main(string[] str)
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                IntPtr handle = GetConsoleWindow();
                ShowWindow(handle, SW_MAXIMIZE);
            }
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var state = new AppCoordinator();

            //состояние приложения
            bool Running = true;

            var Navigation = new Stack<(List<Todo> Tasks,Todo? Parent)>();
            Navigation.Push((state.RootList,null));

            var Window = RenderWindow.InitializeLayout();

            AnsiConsole.Cursor.Hide();
            Console.Clear();

            try
            {
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
            finally
            {
                // Этот код выполнится в любом случае при выходе
                AnsiConsole.Cursor.Show(); 
                //Console.Clear();
                AnsiConsole.MarkupLine("[yellow]Stacly закрыт. До встречи![/]");
            }
        }

    }
}
