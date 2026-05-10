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
            AppState.EditingField = EditingField.Name;
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
            AppState.EditingField = EditingField.Name;
            AppState.EditingTodo = selectTodo;
            AppState.Buffer = selectTodo.Name;
            AppState.Mod = Mods.Input;
        }

        public static void WriteDescription(Todo selectTodo,AppState AppState)
        {
            AppState.EditingField = EditingField.Description;
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
            AppState.EditingField = EditingField.Tags;
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

        public static List<Todo> Sort(List<Todo> list,AppState AppState)
        {
            var displayList = list
                .OrderBy(t => t.IsCompleted)        // Сначала невыполненные (false < true)
                .ThenByDescending(t => t.Priority)  // Потом важные
                .ToList();
            return displayList;
        }

        public static void MoveUpSearch(AppState AppState,List<Todo> todoList)
        {
            if(AppState.CurentFoundMatch > 0)
            {
                AppState.CurentFoundMatch = AppState.FoundItems.Count -1;
            }
            AppState.CurentFoundMatch--;
            int index = todoList.IndexOf(AppState.FoundItems[AppState.CurentFoundMatch]);
            AppState.SelectedIndex = index;
        }

        public static void MoveDownSearch(AppState AppState,List<Todo> todoList)
        {
            if(AppState.CurentFoundMatch < AppState.FoundItems.Count -1)
            {
                AppState.CurentFoundMatch = 0;     
            }
            AppState.CurentFoundMatch++;
            int index = todoList.IndexOf(AppState.FoundItems[AppState.CurentFoundMatch]);
            AppState.SelectedIndex = index;

        }

        public static (int completed, int total) GetDeepProgress(IEnumerable<Todo> tasks)
        {
            int completed = 0;
            int total = 0;

            foreach (var task in tasks)
            {
                // 1. Считаем саму текущую задачу
                total++;
                if (task.IsCompleted) completed++;

                // 2. Если есть подзадачи, рекурсивно считаем их
                if (task.SubTasks != null && task.SubTasks.Count > 0)
                {
                    // ВАЖНО: Результат вызова НУЖНО ПРИБАВИТЬ к текущим счетчикам
                    var sub = GetDeepProgress(task.SubTasks);
                    completed += sub.completed; // Плюсуем выполненные подзадачи
                    total += sub.total;         // Плюсуем общее число подзадач
                }
            }
            return (completed, total);
        }

    }
}
