using Spectre.Console;
using Spectre.Console.Rendering;
using System;

namespace Stacly
{
    static class RenderWindow
    {
        public static Layout Render(Layout Window,List<Todo> todoList,Stack<(List<Todo> Tasks, Todo Parent)> Navigation,AppState AppState)
        {
            //сылки на контент
            var tree = View.DrawList(todoList,Navigation,AppState);
            var dital = View.DrawEdit(todoList[AppState.SelectedIndex],AppState);
            int total = 0, completed = 0;
            var progresBar = View.DrawBar(todoList,ref completed,ref total);
            var hello = View.DrawHello(AppState);
            var help = View.DrawStatusBar(AppState);

            Panel Tree = CreateStyledPanel(tree, "СПИСОК ЗАДАЧ", Color.White,BoxBorder.Rounded);
            Panel Dital = CreateStyledPanel(dital, "ДИТАЛИ", Color.White, BoxBorder.Rounded);
            Panel ProgresBar = CreateStyledPanel(progresBar, $"[white]Прогресс: {completed}/{total}[/]", Color.White, BoxBorder.Rounded);
            Panel Hello = CreateStyledPanel(hello, "", Color.White,BoxBorder.Rounded);
            Panel Help = CreateStyledPanel(help, "", Color.White,BoxBorder.Rounded);



            switch(AppState.Mod)
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

            if(todoList.Count <= 0)
            {
                Dital = CreateStyledPanel(new Text("/nНИЧЕГО НЕ НАЙДЕНО"), "ДИТАЛИ", Color.White, BoxBorder.Rounded);
            }
                   
            Text text;
            if(AppState.Mod == Mods.Search) text = new Text(AppState.SearchBuffer + "█", new Style(Color.Yellow));
            else text = new Text(" ");
            var searchBox = new Panel(text)
                .Header(" ПОИСК ")
                .Border(BoxBorder.Rounded)
                .Expand()
                .BorderColor(AppState.SearchBuffer.StartsWith("#") ? Color.Magenta : Color.Cyan); 
                // Цвет рамки меняется, если ищем по тегам!


            Window["Search"].Update(searchBox);
            Window["ProgresBar"].Update(ProgresBar);
            Window["Tree"].Update(Tree);
            Window["Hello"].Update(Hello);
            Window["Details"].Update(Dital);
            Window["Comands"].Update(Help);

            return Window;
        }

        public static Layout CreatWindow()
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
