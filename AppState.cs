using System;
using System.Collections.Generic;

namespace TodoList
{
    public class AppState 
    {
        public bool IsExpanded = false;
        public int SelectedIndex = 0;

        //паременты для изменения текстовых значений
        public string Buffer = "";
        public Mods Mod = Mods.List;
        public string EditingField = "";
        public Todo? EditingTodo = null;

        //паременты для поиска 
        public string SearchBuffer = "";
        public List<Todo> FoundItems = null;
        public int CurentFoundMatch = 0;
    }
}
