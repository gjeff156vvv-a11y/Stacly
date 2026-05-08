using Spectre.Console;
using System;
using System.Linq;

namespace TodoList
{
    static class RenderWindow
    {
        public static Layout Render(Layout Window,Mods mod,List<Todo> todoList,int selectedIndex,Stack<(List<Todo> Tasks, Todo Parent)> Navigation,bool isExpendMod)
        {

            var activeList = Color.White;
            var activeEdit = Color.Gray;

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

             var displayList = todoList
                .OrderBy(t => t.IsCompleted)        // Сначала невыполненные (false < true)
                .ThenByDescending(t => t.Priority)  // Потом важные
                .ToList();

            var leftContent = new Panel(View.DrawList(displayList,selectedIndex,Navigation,isExpendMod))
                .Header("Список задач")
                .Expand()
                .Border(BoxBorder.Rounded) // Тип границы
                .BorderColor(activeList); // Тот самый цвет!

            var rightContent = new Panel(View.DrawEdit(todoList[selectedIndex]))
                .Header(" Детали ")
                .Expand()
                .Border(BoxBorder.Rounded) // Тип границы
                .BorderColor(activeEdit);
            
            Window["Search"].Update(new Panel(new Text("Поиск...")));
            Window["ProgresBar"].Update(new Panel(new Text("Прогресс 0%")));
            Window["Tree"].Update(leftContent);
            Window["Hello"].Update(new Panel(new Text("Привет!")));
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
                    new Layout("Tree").Size(53));

            layout["Edit"].SplitRows(
                    new Layout("Hello").Size(6),
                    new Layout("Details").Size(53));

            return layout;

        }
    }
}
