using System;
using Spectre.Console;

namespace Stacly
{
    static class ComponentRenderer
    {

        public static Tree DrawList(List<Todo> todoList,Stack<(List<Todo> Tasks, Todo Parent)> Navigation,AppCoordinator state)
        {
            Tree tree;
            if(Navigation.Peek().Parent == null)
                tree = new Tree("TodoList").Guide(TreeGuide.Ascii).Style(Style.Parse("grey bold"));
            else 
                tree = new Tree(Navigation.Peek().Parent.Name).Guide(TreeGuide.Ascii).Style(Style.Parse("grey bold"));

            for (int i = 0; i < todoList.Count; i++)
            {
                string[] colors = GetThemeForTodo(todoList[i]);
                TreeNode todo = null;
                string date;
                if(todoList[i].IsCompleted == true) 
                    date = TodoManager.RelativeTime(todoList[i].CompleteAt);
                else date = TodoManager.RelativeTime(todoList[i].CreatedAt);

                if(state.SelectedIndex == i)
                {
                    todo = tree.AddNode($"[CadetBlue_1]{i+1,-2}[/] [{colors[1]}]{Markup.Escape(colors[2]),-7}[/] [{colors[0]}]{todoList[i].Name,-10}[/][{colors[1]}]{date}[/] ");
                }
                else if(state.EditingTodo == todoList[i])
                { 
                    if(state.Mod == Mods.Input && state.EditingField == EditingField.Name)
                    {
                        todo = tree.AddNode($"[CadetBlue_1]{i+1,-2}[/] [{colors[1]}]{Markup.Escape(colors[2]),-7}[/] [{colors[0]}]{state.InputBuffer}{FlashCursor(),-10}[/][{colors[1]}]{date}[/] ");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(state.SearchBuffer.ToLower().Trim()) && state.FoundItems.Contains(todoList[i])) 
                    {
                        todo = tree.AddNode($"[DarkOrange]{i+1,-2}[/] [{colors[1]}]{Markup.Escape(colors[2]),-7}[/] [{colors[0]}]{todoList[i].Name,-10}[/][{colors[1]}]{date}[/] ");
                    }
                    else todo = tree.AddNode($"{i+1,-2} [{colors[1]}]{Markup.Escape(colors[2]),-7}[/] [{colors[0]}]{todoList[i].Name,-10}[/][{colors[1]}]{date}[/] ");
                }

                if(state.IsExpanded  == true && todo != null)
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
                string[]  colors = GetThemeForTodo(todoList[i]);
                if(todoList.Count <= 0)return;
                else
                {
                    var subtodo = todo.AddNode($"[gray]{Markup.Escape(colors[2]),-7} {todoList[i].Name}[/]");
                    AddNodeRecurse(subtodo,todoList[i].SubTasks);
                }
            }

        }

        public static Grid DrawEdit(Todo selectTodo,AppCoordinator state,List<Todo> todoList)
        {
            var grid = new Grid();

            grid.AddColumn();
            grid.AddColumn();

            if(todoList.Count > 0)
            {
                string[] colors = GetThemeForTodo(selectTodo);
                string displayName = (state.Mod == Mods.Input)
                    ? state.InputBuffer + FlashCursor()
                    : selectTodo.Name;
                string displayDesc = (state.EditingField == EditingField.Description) 
                    ? state.InputBuffer + FlashCursor()
                    : selectTodo.Description;

                string displayTags = (state.EditingField == EditingField.Tags) 
                    ? state.InputBuffer + FlashCursor()
                    : string.Join(", ", selectTodo.Tags);

            
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
                grid.AddRow($"Время создания:",$"{TodoManager.RelativeTime(selectTodo.CreatedAt)}");
                grid.AddRow("");
                grid.AddRow($"Время выполнения:",$"{TodoManager.RelativeTime(selectTodo.CompleteAt)}");
                grid.AddRow("");
            }
            else 
            {
                grid.AddRow("");
                grid.AddRow("НИЧЕГО НЕ НАЙДЕНО");
            }


            return grid;              
        }

        public static BreakdownChart DrawBar(List<Todo> todoList,ref int completed ,ref int total,AppCoordinator state)
        {
            (completed, total) = TodoManager.GetDeepProgress(state.RootList);
            int remaining = total - completed;

            var chart = new BreakdownChart()
                .FullSize() // Растянуть на всю ширину панели
                .AddItem("Done", completed, Color.Green)
                .AddItem("Todo", remaining, Color.Red);

            // Если задач совсем нет, можно добавить пустой элемент, чтобы график не падал
            if (total == 0) chart.AddItem("No tasks", 1, Color.Grey);

            return chart;
        }

        public static Grid DrawHello(AppCoordinator state)
        {
            // Примерная идея для содержимого:
            var grid = new Grid().AddColumn();
            grid.AddRow("");
            grid.AddRow("[bold cadetblue_1]STACKLY TUI[/] [gray]v1.0[/]");
            grid.AddRow("");
            grid.AddRow($"[yellow]Mode:[/] [invert]{state.Mod}[/]");
            //grid.AddRow($"[blue]Total Tasks:[/] {TodoManager.GetDeepProgress(rootList).total}");
            return grid;

        }

        public static Markup DrawStatusBar(AppCoordinator state)
        {
            string text = "";
            if(state.Message == null || state.MessageUtil <= DateTime.Now)
            {
                switch(state.Mod)
                {
                    case Mods.List: text = "[bold aqua]j/k[/] [gray]move[/] | [bold aqua]Shift+j/k[/] [gray]reorder[/] | [bold aqua]h/l[/] [gray]up/in[/] | [bold aqua]Space[/] [gray]done[/] | [bold aqua]n/N[/] [gray]new/find[/] | [bold aqua]d[/] [gray]del[/] | [bold aqua]/[/] [gray]search[/] | [bold aqua]q[/] [gray]quit[/]";break;
                    case Mods.Edit: text = "[bold yellow]r[/] [gray]rename[/] | [bold yellow]d[/] [gray]desc[/] | [bold yellow]p[/] [gray]priority[/] | [bold yellow]t[/] [gray]tags[/] | [bold red]Esc[/] [gray]back[/]";break;
                    case Mods.Input: text = "[bold green]Enter[/] [gray]save[/] | [bold red]Esc[/] [gray]cancel[/]";break;
                    case Mods.Search:text = "[bold orange1]Enter/Esc[/] [gray]finish[/] | [bold gray]Type to filter...[/]";break;
                    default: text = "";break;
                    case Mods.ConfirmDelete: text = "[bold yellow]Потвердити удаление: [/][green]Y[/] / [red] N [/]";break;
                }
            }
            else
            {
                string color;
                if(state.MessageError == true) color = "red";
                else color = "yellow";
                text = $"[{color}]{state.Message}[/]";
            }

            var help = new Markup(text);

            return help;
        }

        public static Markup DrawSearch(AppCoordinator state)
        {
            Markup text;
            if(state.Mod == Mods.Search) 
            {
                // Во View.cs при отрисовке буфера
                string displayBuffer = state.SearchBuffer;
                if (!string.IsNullOrEmpty(state.TagSuggestion))
                {
                    // Отрисовываем оставшуюся часть тега серым
                    var typedPart = state.SearchBuffer.Split('#').Last();
                    var suggestionPart = state.TagSuggestion.Substring(typedPart.Length);
                    displayBuffer += $"[grey]{suggestionPart}[/]";
                }

                text = new Markup(displayBuffer + FlashCursor(), new Style(Color.Yellow));
            }
            else text = new Markup(" ");

            return text; 
        }

        private static string[] GetThemeForTodo(Todo todo)
        {
            string[] colors = new string[3];
            colors[0] = "white";
            colors[1] = "white";
            switch(todo.Priority)
            {
                case Priorities.Low: colors[0] = "yellow"; break;
                case Priorities.Medium: colors[0] = "blue"; break;
                case Priorities.High: colors[0] = "red"; break;
                default: colors[0] = "white"; break; // На всякий случай
            }

            colors[1] = todo.IsCompleted ? "green" : "red";
            colors[2] = todo.IsCompleted ? "[x]" : "[ ]";
            return colors;
        }

        private static string FlashCursor()
        {
            var flashTime = DateTime.Now.Millisecond;
            if(flashTime % 1000 < 500)
                return " ";
            else return "█";

        }
    }
}
