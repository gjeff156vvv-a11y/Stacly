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

            while(AppState.Running)
            {
                Console.Clear();

                RenderWindow.Render(Mods,Navigation.Peek().Tasks,AppState.SelectedIndex,Navigation,AppState.IsExpanded);
                switch(Mods)
                {
                    case Mods.List: ListMode.Mode(Navigation,Navigation.Peek().Tasks,ref AppState,ref Mods); break;
                    case Mods.Edit: EditMode.Mode(Navigation.Peek().Tasks,ref AppState,ref Mods); break;
                    case Mods.Search: SearchMode.Mode(Navigation,Navigation.Peek().Tasks,ref AppState,ref Mods); break;
                }
                                                    }
            Storage.SaveAll(todoList);
        }
    }
}
