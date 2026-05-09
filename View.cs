using System;
using Spectre.Console;

namespace TodoList
{
    static class View
    {

        public static Tree DrawList(List<Todo> todoList,Stack<(List<Todo> Tasks, Todo Parent)> Navigation,AppState AppState)
        {
            Tree tree;
            if(Navigation.Peek().Parent == null)
                tree = new Tree("TodoList");
            else 
                tree = new Tree(Navigation.Peek().Parent.Name);

            for (int i = 0; i < todoList.Count; i++)
            {
                string[] colors = TodoColors(todoList[i]);
                TreeNode todo = null;

                if(AppState.SelectedIndex == i)
                {
                    todo = tree.AddNode($"[CadetBlue_1]{i+1,-2}[/] [{colors[1]}]{Markup.Escape(colors[2]),-7}[/] [{colors[0]}]{todoList[i].Name}[/]");
                }
                else if(AppState.EditingTodo == todoList[i])
                { 
                    if(AppState.Mod == Mods.Input && AppState.EditingField == "Name")
                    {
                        todo = tree.AddNode($"[CadetBlue_1]{i+1,-2}[/] [{colors[1]}]{Markup.Escape(colors[2]),-7}[/] [{colors[0]}]{AppState.Buffer}_[/]");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(AppState.SearchBuffer.ToLower().Trim()) && AppState.FoundItems.Contains(todoList[i])) 
                    {
                        todo = tree.AddNode($"[DarkOrange]{i+1,-2}[/] [{colors[1]}]{Markup.Escape(colors[2]),-7}[/] [{colors[0]}]{todoList[i].Name}[/]");
                    }
                    else todo = tree.AddNode($"{i+1,-2} [{colors[1]}]{Markup.Escape(colors[2]),-7}[/] [{colors[0]}]{todoList[i].Name}[/]");
                }

                if(AppState.IsExpanded  == true && todo != null)
                {
                     AddNodeRecurse(todo,todoList[i].SubTasks);
                }
            }
            return tree;
        }

        private static void AddNodeRecurse(TreeNode todo,List<Todo> todoList)
        {
            for(var i = 0; i < todoList.Count;i++)
            {
                string[]  colors = TodoColors(todoList[i]);
                if(todoList.Count <= 0)return;
                else
                {
                    var subtodo = todo.AddNode($"[gray]{Markup.Escape(colors[2]),-7} {todoList[i].Name}[/]");
                    AddNodeRecurse(subtodo,todoList[i].SubTasks);
                }
            }

        }

        public static Grid DrawEdit(Todo selectTodo,AppState AppState)
        {
            string[] colors = TodoColors(selectTodo);
            string displayName = (AppState.EditingField == "Name")
                ? AppState.Buffer + "_" 
                : selectTodo.Name;
            string displayDesc = (AppState.EditingField == "Description") 
                ? AppState.Buffer + "_" 
                : selectTodo.Description;

            string displayTags = (AppState.EditingField == "Tags") 
                ? AppState.Buffer + "_" 
                : string.Join(", ", selectTodo.Tags);

            var grid = new Grid();

            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow("");
            grid.AddRow($"[{colors[0]}]Имя:[/]",$"[{colors[0]}]{displayName}[/]");
            grid.AddRow("");
            grid.AddRow($"[yellow]Важность:[/]",$"[yellow]{selectTodo.Priority}[/]");
            grid.AddRow("");
            grid.AddRow($"[green]Описание:[/]",$"[green]{displayDesc}[/]");
            grid.AddRow("");
            grid.AddRow($"[{colors[1]}]Статус:[/]",$"[{colors[1]}]{Markup.Escape(colors[2])}[/]");
            grid.AddRow("");
            grid.AddRow($"[yellow]Теги:[/]",$"[yellow]{displayTags}[/]");
            grid.AddRow("");
            grid.AddRow($"Время создания:",$"{selectTodo.CreatedAt}");
            grid.AddRow("");
            grid.AddRow($"Время выполнения:",$"{selectTodo.CompleteAt}");
            grid.AddRow("");

            return grid;              
        }

        public static BreakdownChart DrawBar(List<Todo> todoList,ref int completed ,ref int total)
        {
            (completed, total) = TodoManager.GetDeepProgress(todoList);
            int remaining = total - completed;

            var chart = new BreakdownChart()
                .FullSize() // Растянуть на всю ширину панели
                .AddItem("Done", completed, Color.Green)
                .AddItem("Todo", remaining, Color.Red);

            // Если задач совсем нет, можно добавить пустой элемент, чтобы график не падал
            if (total == 0) chart.AddItem("No tasks", 1, Color.Grey);

            return chart;
        }

        private static string[] TodoColors(Todo todo)
        {
            string[] colors = new string[3];
            colors[0] = "white";
            colors[1] = "white";
            switch(todo.Priority)
            {
                case Priorities.Low: colors[0] = "blue"; break;
                case Priorities.Medium: colors[0] = "yellow"; break;
                case Priorities.High: colors[0] = "red"; break;
                default: colors[0] = "white"; break; // На всякий случай
            }

            colors[1] = todo.IsCompleted ? "green" : "red";
            colors[2] = todo.IsCompleted ? "[x]" : "[ ]";
            return colors;
        }
    }
}
