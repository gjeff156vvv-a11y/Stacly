using System;
using System.Collections.Generic;
using Spectre.Console; 

namespace  TodoList
{
    public static class TodoManager
    {
        public static void AddTodo(List<Todo> todoList)
        {
            var newName = AnsiConsole.Ask<string>("Введити [green]название [/]?");
            var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<Priorities>()                         
                    .Title("выберети [green]васжность[/] :")
                    .AddChoices(Priorities.Low, Priorities.Medium, Priorities.High));

            todoList.Add(new Todo(newName,choice));
        }

        public static void RemoveTodo(List<Todo> todoList,int selectedIndex)
        {
            if (todoList.Count > 0) 
            {
                todoList.RemoveAt(selectedIndex);
            }
        }

        public static void RenameTodo(Todo selectTodo)
        {
            var newName = AnsiConsole.Ask<string>("Введити новое [green]название [/]?");
            selectTodo.ChangeName(newName);
        }

        public static void WriteDescription(Todo selectTodo)
        {
            var newDescription = AnsiConsole.Ask<string>("Введити новое [green]описание[/]?");
            selectTodo.ChangeDescription(newDescription);
        }

        public static void ChoiceNewPriority(Todo selectTodo)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<Priorities>()                        
                .Title("выберети [green]васжность[/] :")
                .AddChoices(Priorities.Low, Priorities.Medium, Priorities.High));
            selectTodo.ChangePriority(choice);
        }

        public static void WriteTags(Todo selectTodo)
        {
            var newTag = AnsiConsole.Ask<string>("Введити новый [green]тег[/]?");
            selectTodo.ChangeTags(newTag);
        }

        public static void PushNavigation(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,ref int selectedIndex)
        {
            Navigation.Push(((Navigation.Peek().Tasks[selectedIndex].SubTasks),Navigation.Peek().Tasks[selectedIndex]));
            if(Navigation.Peek().Tasks.Count == 0)TodoManager.AddTodo(Navigation.Peek().Tasks);
            selectedIndex = 0;
        }

        public static void PopNavigation(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,ref int selectedIndex)
        {
            if(Navigation.Peek().Parent != null)
            {
                Navigation.Pop();
            }
            selectedIndex = 0;
        }

        public static void MoveUp(List<Todo> todoList,ref int selectedIndex)
        {
            if(selectedIndex > 0)       
            {
                Todo selectTodo = todoList[selectedIndex];
                todoList.RemoveAt(selectedIndex);
                todoList.Insert(selectedIndex - 1,selectTodo);
                selectedIndex--;
            }
        }
        public static void MoveDown(List<Todo> todoList,ref int selectedIndex)
        {
            if(selectedIndex < todoList.Count -1)       
            {
                Todo selectTodo = todoList[selectedIndex];
                todoList.RemoveAt(selectedIndex);
                todoList.Insert(selectedIndex + 1,selectTodo);
                selectedIndex++;
            }
        }
    }
}
