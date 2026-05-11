using System;
using System.Collections.Generic;
using Spectre.Console; 

namespace  Stacly
{
    public static class TodoManager
    {
        public static void AddTodo(List<Todo> todoList,ref AppCoordinator state)
        {
            var newTask = new Todo("", Priorities.Low);
            todoList.Add(newTask);
            state.EditingField = EditingField.Name;
            state.EditingTodo = newTask;
            state.Mod = Mods.Input;
        }

        public static void RemoveTodo(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,List<Todo> todoList,AppCoordinator state)
        {
            Todo selectTodo = todoList[state.SelectedIndex];
            if (todoList.Count > 0) 
            {
                Navigation.Peek().Tasks.Remove(selectTodo);
            }
            state.SelectedIndex = Math.Clamp(state.SelectedIndex, 0, Math.Max(0, todoList.Count - 1));

        }

        public static void RenameTodo(Todo selectTodo,AppCoordinator state)
        {
            state.EditingField = EditingField.Name;
            state.EditingTodo = selectTodo;
            state.InputBuffer = selectTodo.Name;
            state.Mod = Mods.Input;
        }

        public static void WriteDescription(Todo selectTodo,AppCoordinator state)
        {
            state.EditingField = EditingField.Description;
            state.EditingTodo = selectTodo;
            state.InputBuffer = selectTodo.Description;
            state.Mod = Mods.Input;

        }

        public static void CyclePriority(Todo todo,AppCoordinator state)
        {
            todo.ChangePriority(todo.Priority switch
            {
                Priorities.Low => Priorities.Medium,
                Priorities.Medium => Priorities.High,
                Priorities.High => Priorities.Low,
        _       => Priorities.Low
            });
            state.SetMessage($"Приретет изменен на {todo.Priority}",false);
        }

        public static void WriteTags(Todo selectTodo,AppCoordinator state)
        {
            state.EditingField = EditingField.Tags;
            state.EditingTodo = selectTodo;
            state.InputBuffer = string.Join(", ", selectTodo.Tags);
            state.Mod = Mods.Input;

        }

        public static void PushNavigation(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,ref AppCoordinator state)
        {
            Navigation.Push(((Navigation.Peek().Tasks[state.SelectedIndex].SubTasks),Navigation.Peek().Tasks[state.SelectedIndex]));
            //if(Navigation.Peek().Tasks.Count == 0)TodoManager.AddTodo(Navigation.Peek().Tasks);
            state.SelectedIndex = 0;
        }

        public static void PopNavigation(Stack<(List<Todo> Tasks,Todo? Parent)> Navigation,ref AppCoordinator state)
        {
            var parent = Navigation.Peek().Parent;
            if(Navigation.Peek().Parent != null)
            {
                Navigation.Pop();
            }
            int foundIndex = Navigation.Peek().Tasks.IndexOf(parent);
            // Если не нашли или список пуст, ставим 0, иначе найденный индекс
            state.SelectedIndex = foundIndex >= 0 && (foundIndex < Navigation.Peek().Tasks.Count -1) ? foundIndex : 0;
        }

        public static void MoveUp(List<Todo> todoList,ref AppCoordinator state)
        {
            if(state.SelectedIndex > 0)       
            {
                Todo selectTodo = todoList[state.SelectedIndex];
                todoList.RemoveAt(state.SelectedIndex);
                todoList.Insert(state.SelectedIndex - 1,selectTodo);
                state.SelectedIndex--;
            }
        }
        public static void MoveDown(List<Todo> todoList,ref AppCoordinator state)
        {
            if(state.SelectedIndex < todoList.Count -1)       
            {
                Todo selectTodo = todoList[state.SelectedIndex];
                todoList.RemoveAt(state.SelectedIndex);
                todoList.Insert(state.SelectedIndex + 1,selectTodo);
                state.SelectedIndex++;
            }
        }

        public static List<Todo> Sort(List<Todo> list,AppCoordinator state)
        {
            var displayList = list
                .OrderBy(t => t.IsCompleted)        // Сначала невыполненные (false < true)
                .ThenByDescending(t => t.Priority)  // Потом важные
                .ToList();
            return displayList;
        }

        public static void MoveUpSearch(AppCoordinator state,List<Todo> todoList)
        {
            // Если мы НЕ на первом элементе — идем вверх
            if (state.FoundMatchIndex > 0)
            {
                state.FoundMatchIndex--;
            }
            else // Если мы на самом верху — прыгаем в конец
            {
                state.FoundMatchIndex = state.FoundItems.Count - 1;
            }
            int index = todoList.IndexOf(state.FoundItems[state.FoundMatchIndex]);
            state.SelectedIndex = index;
        }

        public static void MoveDownSearch(AppCoordinator state,List<Todo> todoList)
        {
            // Если мы НЕ на последнем элементе — просто идем вниз
            if (state.FoundMatchIndex < state.FoundItems.Count - 1)
            {
                state.FoundMatchIndex++;
            }
            else // Если мы уже на последнем — прыгаем в начало
            {
                state.FoundMatchIndex = 0;
            }
            // И ТОЛЬКО ПОТОМ берем элемент из списка
            int index = todoList.IndexOf(state.FoundItems[state.FoundMatchIndex]);
            state.SelectedIndex = index;
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

        public static string RelativeTime(DateTime? date)
        {
            if(date == null) return null;
            var time = DateTime.Now - date;

            string relativeTime;
            if(time.Value.Days >= 1)
            {
                relativeTime = time.Value.ToString("dd.MM.yy");
            }
            else if(time.Value.Hours >= 1)
            {
                if(time.Value.Hours > 4)
                    relativeTime = $"{time.Value.Hours} часов назад";
                else relativeTime = $"{time.Value.Hours} часа назад";
            }
            else if(time.Value.Minutes >= 1)
            {
                if(time.Value.Minutes > 4)
                    relativeTime = $"{time.Value.Minutes} минут назад";
                else relativeTime = $"{time.Value.Minutes} минуты назад";
            }
            else relativeTime = $"несколько секунд назад";

            return relativeTime;
        }

    }
}
