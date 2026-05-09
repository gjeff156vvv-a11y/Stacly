using Spectre.Console;
using System;

namespace TodoList
{
    static class RenderWindow
    {
        public static Layout Render(Layout Window,List<Todo> todoList,Stack<(List<Todo> Tasks, Todo Parent)> Navigation,AppState AppState)
        {
            Panel leftContent;
            Panel rightContent;

            var activeList = Color.White;
            var activeEdit = Color.Gray;

            switch(AppState.Mod)
            {
                case Mods.List:
                    activeList = Color.White;
                    activeEdit = Color.Gray;
                    break;
                case Mods.Edit:
                    activeList = Color.Gray;
                    activeEdit = Color.White;
                    break;
            }


            leftContent = new Panel(View.DrawList(todoList,Navigation,AppState))
                .Header("Список задач")
                .Expand()
                .Border(BoxBorder.Rounded) // Тип границы
                .Padding(1,1)
                .BorderColor(activeList); // Тот самый цвет!

            if(todoList.Count > 0)
            {
                rightContent = new Panel(View.DrawEdit(todoList[AppState.SelectedIndex],AppState))
                    .Header(" Детали ")
                    .Expand()
                    .Border(BoxBorder.Rounded) // Тип границы
                    .BorderColor(activeEdit);
            }
            else
            { 
                rightContent = new Panel(new Text($"\nНичего не найдено..."))
                    .Header(" Детали ")
                    .Expand()
                    .Border(BoxBorder.Rounded) // Тип границы
                    .BorderColor(activeEdit);
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
            Window["ProgresBar"].Update(new Panel(new Text("Прогресс 0%")).Expand());
            Window["Tree"].Update(leftContent);
            Window["Hello"].Update(new Panel(AppState.Mod.ToString()));
            Window["Details"].Update(rightContent);
            Window["Comands"].Update(new Panel(new Text("j/k - навигация...")));

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
    }
}
