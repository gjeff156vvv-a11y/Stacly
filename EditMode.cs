using System;
using Spectre.Console;

namespace TodoList
{
    static class EditMode
    {
         public static void Mode(List<Todo> todoList,ref AppState AppState,ref Mods Mods)
         {
            Todo selectTodo = todoList[AppState.SelectedIndex];
           
            View.DrawEdit(selectTodo);
            // Читаем нажатие клавиши БЕЗ ожидания Enter
            var key = Console.ReadKey(true).KeyChar;

            switch (key)
            {
                case 'r': TodoManager.RenameTodo(selectTodo); break;
                case 'd': TodoManager.WriteDescription(selectTodo); break;               
                case 'p': TodoManager.ChoiceNewPriority(selectTodo); break;
                case 't': TodoManager.WriteTags(selectTodo); break;
                case 'c': selectTodo.ChangeIsCompleted(); break;
                case 'q': Mods = Mods.List; break;
            }
         }
    }
}
