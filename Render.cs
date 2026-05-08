using Spectre.Console;
using System;

namespace TodoList
{
    static class RenderWindow
    {
        public static void Render(Mods mod,List<Todo> todoList,int selectedIndex,Stack<(List<Todo> Tasks, Todo Parent)> Navigation,bool isExpendMod)
        {
            //создание разметки
            var layout = new Layout("Root")
                .SplitRows(
                        new Layout("Stackly"),
                        new Layout("Comands").Size(6));
            
            layout["Stackly"].SplitColumns(
                    new Layout("List"),
                    new Layout("Edit"));

            layout["List"].SplitRows(
                    new Layout("Search").Size(4),
                    new Layout("ProgresBar").Size(4),
                    new Layout("Tree").Ratio(15));

            layout["Edit"].SplitRows(
                    new Layout("Hello").Size(8),
                    new Layout("Details").Ratio(15));


            var activeList = Color.White;
            var activeEdit = Color.Gray;

            var leftContent = new Panel(View.DrawList(todoList,selectedIndex,Navigation,isExpendMod))
                .Header("Список задач")
                .Expand()
                .Border(BoxBorder.Rounded) // Тип границы
                .BorderColor(activeList) // Тот самый цвет!
                .Padding(2,0);

            var rightContent = new Panel(View.DrawEdit(todoList[selectedIndex]))
                .Header(" Детали ")
                .Expand()
                .Border(BoxBorder.Rounded) // Тип границы
                .BorderColor(activeEdit)
                .Padding(2,0);

            switch(mod)
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

            layout["Tree"].Update(leftContent);
            layout["Details"].Update(rightContent);
            AnsiConsole.Write(layout);
        }
    }
}
