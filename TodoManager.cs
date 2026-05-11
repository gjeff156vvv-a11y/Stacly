using System;
using System.Collections.Generic;
using Spectre.Console; 

namespace  Stacly
{
    public static class TodoManager
    {
        public static void AddTodo(List<Todo> todoList,ref AppCoordinator AppCoordinator)
        {
            var newTask = new Todo("", Priorities.Low);
            todoList.Add(newTask);
            AppCoordinator.EditingField = EditingField.Name;
            AppCoordinator.EditingTodo = newTask;
            AppCoordinator.Mod = Mods.Input;
        }

        public static void RemoveTodo(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,List<Todo> todoList,AppCoordinator AppCoordinator)
        {
            if (todoList.Count > 0) 
            {
                Navigation.Peek().Tasks.Remove(todoList[AppCoordinator.SelectedIndex]);
            }
        }

        public static void RenameTodo(Todo selectTodo,AppCoordinator AppCoordinator)
        {
            AppCoordinator.EditingField = EditingField.Name;
            AppCoordinator.EditingTodo = selectTodo;
            AppCoordinator.InputBuffer = selectTodo.Name;
            AppCoordinator.Mod = Mods.Input;
        }

        public static void WriteDescription(Todo selectTodo,AppCoordinator AppCoordinator)
        {
            AppCoordinator.EditingField = EditingField.Description;
            AppCoordinator.EditingTodo = selectTodo;
            AppCoordinator.InputBuffer = selectTodo.Description;
            AppCoordinator.Mod = Mods.Input;

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

        public static void WriteTags(Todo selectTodo,AppCoordinator AppCoordinator)
        {
            AppCoordinator.EditingField = EditingField.Tags;
            AppCoordinator.EditingTodo = selectTodo;
            AppCoordinator.InputBuffer = string.Join(", ", selectTodo.Tags);
            AppCoordinator.Mod = Mods.Input;

        }

        public static void PushNavigation(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,ref AppCoordinator AppCoordinator)
        {
            Navigation.Push(((Navigation.Peek().Tasks[AppCoordinator.SelectedIndex].SubTasks),Navigation.Peek().Tasks[AppCoordinator.SelectedIndex]));
            //if(Navigation.Peek().Tasks.Count == 0)TodoManager.AddTodo(Navigation.Peek().Tasks);
            AppCoordinator.SelectedIndex = 0;
        }

        public static void PopNavigation(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,ref AppCoordinator AppCoordinator)
        {
            var parent = Navigation.Peek().Parent;
            if(Navigation.Peek().Parent != null)
            {
                Navigation.Pop();
            }
            int foundIndex = Navigation.Peek().Tasks.IndexOf(parent);
            // Если не нашли или список пуст, ставим 0, иначе найденный индекс
            AppCoordinator.SelectedIndex = foundIndex >= 0 && (foundIndex < Navigation.Peek().Tasks.Count -1) ? foundIndex : 0;
        }

        public static void MoveUp(List<Todo> todoList,ref AppCoordinator AppCoordinator)
        {
            if(AppCoordinator.SelectedIndex > 0)       
            {
                Todo selectTodo = todoList[AppCoordinator.SelectedIndex];
                todoList.RemoveAt(AppCoordinator.SelectedIndex);
                todoList.Insert(AppCoordinator.SelectedIndex - 1,selectTodo);
                AppCoordinator.SelectedIndex--;
            }
        }
        public static void MoveDown(List<Todo> todoList,ref AppCoordinator AppCoordinator)
        {
            if(AppCoordinator.SelectedIndex < todoList.Count -1)       
            {
                Todo selectTodo = todoList[AppCoordinator.SelectedIndex];
                todoList.RemoveAt(AppCoordinator.SelectedIndex);
                todoList.Insert(AppCoordinator.SelectedIndex + 1,selectTodo);
                AppCoordinator.SelectedIndex++;
            }
        }

        public static List<Todo> Sort(List<Todo> list,AppCoordinator AppCoordinator)
        {
            var displayList = list
                .OrderBy(t => t.IsCompleted)        // Сначала невыполненные (false < true)
                .ThenByDescending(t => t.Priority)  // Потом важные
                .ToList();
            return displayList;
        }

        public static void MoveUpSearch(AppCoordinator AppCoordinator,List<Todo> todoList)
        {
            if(AppCoordinator.FoundMatchIndex > 0)
            {
                AppCoordinator.FoundMatchIndex = AppCoordinator.FoundItems.Count -1;
            }
            AppCoordinator.FoundMatchIndex--;
            int index = todoList.IndexOf(AppCoordinator.FoundItems[AppCoordinator.FoundMatchIndex]);
            AppCoordinator.SelectedIndex = index;
        }

        public static void MoveDownSearch(AppCoordinator AppCoordinator,List<Todo> todoList)
        {
            if(AppCoordinator.FoundMatchIndex < AppCoordinator.FoundItems.Count -1)
            {
                AppCoordinator.FoundMatchIndex = 0;     
            }
            AppCoordinator.FoundMatchIndex++;
            int index = todoList.IndexOf(AppCoordinator.FoundItems[AppCoordinator.FoundMatchIndex]);
            AppCoordinator.SelectedIndex = index;

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
