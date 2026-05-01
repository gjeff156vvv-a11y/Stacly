using System;
using System.Collections.Generic;

namespace  TodoList
{
   public class TodoManager
   {
       List<Todo> todoList = Storage.ReadAll();
   }
}
