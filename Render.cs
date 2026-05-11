using Spectre.Console;
using Spectre.Console.Rendering;
using System;

namespace Stacly
{
    static class RenderWindow
    {
        public static Layout Render(Layout Window,List<Todo> todoList,Stack<(List<Todo> Tasks, Todo Parent)> Navigation,AppCoordinator state)
        {
            //сылки на контент
            var tree = ComponentRenderer.DrawList(todoList,Navigation,state);
            var dital = ComponentRenderer.DrawEdit(todoList[state.SelectedIndex],state,todoList);
            int total = 0, completed = 0;
            var progresBar = ComponentRenderer.DrawBar(todoList,ref completed,ref total,state);
            var hello = ComponentRenderer.DrawHello(state);
            var help = ComponentRenderer.DrawStatusBar(state);
            var search = ComponentRenderer.DrawSearch(state);

            Panel Tree = CreateStyledPanel(tree, "СПИСОК ЗАДАЧ", Color.White,BoxBorder.Rounded);
            Panel Dital = CreateStyledPanel(dital, "ДИТАЛИ", Color.White, BoxBorder.Rounded);
            Panel ProgresBar = CreateStyledPanel(progresBar, $"[white]Прогресс: {completed}/{total}[/]", Color.White, BoxBorder.Rounded);
            Panel Hello = CreateStyledPanel(hello, "", Color.White,BoxBorder.Rounded);
            Panel Help = CreateStyledPanel(help, "", Color.White,BoxBorder.Rounded);
            Panel searchBox = CreateStyledPanel(search, "ПОИСК", Color.White, BoxBorder.Rounded);



            switch(state.Mod)
            {
                case Mods.List:
                    Tree = CreateStyledPanel(tree, "СПИСОК ЗАДАЧ", Color.White,BoxBorder.Double);
                    Dital = CreateStyledPanel(dital, "ДИТАЛИ", Color.White, BoxBorder.Rounded);
                    break;
                case Mods.Edit:
                    Tree = CreateStyledPanel(tree, "СПИСОК ЗАДАЧ", Color.White,BoxBorder.Rounded);
                    Dital = CreateStyledPanel(dital, "ДИТАЛИ", Color.White, BoxBorder.Double);
                    break;
                case Mods.ConfirmDelete:
                    Tree = CreateStyledPanel(tree, "СПИСОК ЗАДАЧ", Color.Red,BoxBorder.Double);
                    break;
            }

            if(state.Message != null && state.MessageError == true && state.MessageUtil > DateTime.Now)
                Help = CreateStyledPanel(help, "", Color.Red,BoxBorder.Rounded);
            else if(state.Message != null && state.MessageError == false && state.MessageUtil > DateTime.Now)
                Help = CreateStyledPanel(help, "", Color.Yellow,BoxBorder.Rounded);
            else Help = CreateStyledPanel(help, "", Color.White,BoxBorder.Rounded);

                 
            if(state.SearchBuffer.StartsWith("#"))
                searchBox = CreateStyledPanel(search, "ПОИСК", Color.Magenta, BoxBorder.Rounded);
            else searchBox = CreateStyledPanel(search, "ПОИСК", Color.Cyan, BoxBorder.Rounded);

            Window["Search"].Update(searchBox);
            Window["ProgresBar"].Update(ProgresBar);
            Window["Tree"].Update(Tree);
            Window["Hello"].Update(Hello);
            Window["Details"].Update(Dital);
            Window["Comands"].Update(Help);

            return Window;
        }

        public static Layout InitializeLayout()
        {
            var layout = new Layout("Root")
                .SplitRows(
                        new Layout("Stackly"),
                        new Layout("Comands").Size(3)
                );

            layout["Stackly"].SplitColumns(
                    new Layout("List"),
                    new Layout("Edit"));

            layout["List"].SplitRows(
                    new Layout("Search").Size(3),
                    new Layout("ProgresBar").Size(3),
                    new Layout("Tree"));

            layout["Edit"].SplitRows(
                    new Layout("Hello").Size(6),
                    new Layout("Details"));

            return layout;

        }

        private static Panel CreateStyledPanel(IRenderable content, string title, Color borderColor, BoxBorder border)
        {
            return new Panel(content)
                .Header($" {title} ")
                .Border(border)
                .BorderColor(borderColor)
                .Expand();
        }

    }
}
