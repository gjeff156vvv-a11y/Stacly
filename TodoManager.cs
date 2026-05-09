using System;
using System.Collections.Generic;
using Spectre.Console; 

namespace  TodoList
{
    public static class TodoManager
    {
        public static void AddTodo(List<Todo> todoList,ref AppState AppState)
        {
            var newTask = new Todo("", Priorities.Low);
            todoList.Add(newTask);
            AppState.EditingField = "NewName";
            AppState.EditingTodo = newTask;
            AppState.Mod = Mods.Input;
        }

        public static void RemoveTodo(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,List<Todo> todoList,AppState AppState)
        {
            if (todoList.Count > 0) 
            {
                Navigation.Peek().Tasks.Remove(todoList[AppState.SelectedIndex]);
            }
        }

        public static void RenameTodo(Todo selectTodo,AppState AppState)
        {
            AppState.EditingField = "Name";
            AppState.EditingTodo = selectTodo;
            AppState.Buffer = selectTodo.Name;
            AppState.Mod = Mods.Input;
        }

        public static void WriteDescription(Todo selectTodo,AppState AppState)
        {
            AppState.EditingField = "Description";
            AppState.EditingTodo = selectTodo;
            AppState.Buffer = selectTodo.Description;
            AppState.Mod = Mods.Input;

        }

        public static void CyclePriority(Todo todo)
        {
            todo.ChangePriority(todo.Priority switch
            {
                Priorities.Low => Priorities.Medium,
                Priorities.Medium => Priorities.High,
                Priorities.High => Priorities.Low,
        _       => Priorities.Low
            });
        }

        public static void WriteTags(Todo selectTodo,AppState AppState)
        {
            AppState.EditingField = "Tags";
            AppState.EditingTodo = selectTodo;
            AppState.Buffer = string.Join(", ", selectTodo.Tags);
            AppState.Mod = Mods.Input;

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
