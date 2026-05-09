using System;
using System.Collections.Generic;
using Spectre.Console; 

namespace  TodoList
{
    public static class TodoManager
    {
        public static void AddTodo(List<Todo> todoList,ref AppState AppState)
        {
            todoList.Add(new Todo("", Priorities.Low));
            AppState.Buffer = "";
            //AppState.SelectedIndex = todoList.IndexOf(newTask);
            AppState.Mod = Mods.Input;
        }

        public static void RemoveTodo(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,List<Todo> todoList,AppState AppState)
        {
            if (todoList.Count > 0) 
            {
                Navigation.Peek().Tasks.Remove(todoList[AppState.SelectedIndex]);
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

        public static void PushNavigation(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,ref AppState AppState)
        {
            Navigation.Push(((Navigation.Peek().Tasks[AppState.SelectedIndex].SubTasks),Navigation.Peek().Tasks[AppState.SelectedIndex]));
            //if(Navigation.Peek().Tasks.Count == 0)TodoManager.AddTodo(Navigation.Peek().Tasks);
            AppState.SelectedIndex = 0;
        }

        public static void PopNavigation(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,ref AppState AppState)
        {
            if(Navigation.Peek().Parent != null)
            {
                Navigation.Pop();
            }
            AppState.SelectedIndex = 0;
        }

        public static void MoveUp(List<Todo> todoList,ref AppState AppState)
        {
            if(AppState.SelectedIndex > 0)       
            {
                Todo selectTodo = todoList[AppState.SelectedIndex];
                todoList.RemoveAt(AppState.SelectedIndex);
                todoList.Insert(AppState.SelectedIndex - 1,selectTodo);
                AppState.SelectedIndex--;
            }
        }
        public static void MoveDown(List<Todo> todoList,ref AppState AppState)
        {
            if(AppState.SelectedIndex < todoList.Count -1)       
            {
                Todo selectTodo = todoList[AppState.SelectedIndex];
                todoList.RemoveAt(AppState.SelectedIndex);
                todoList.Insert(AppState.SelectedIndex + 1,selectTodo);
                AppState.SelectedIndex++;
            }
        }
    }
}
