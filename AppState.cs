using System;
using System.Collections.Generic;

namespace Stacly
{
    public class AppCoordinator 
    {
        public bool IsExpanded = false;
        public bool IsDirty = false;
        public int SelectedIndex = 0;
        public Mods Mod = Mods.List;

        //паременты для изменения текстовых значений
        public string InputBuffer = "";
        public EditingField EditingField = EditingField.Name;
        public Todo? EditingTodo = null;

        //паременты для поиска 
        public string SearchBuffer = "";
        public List<Todo> FoundItems = null;
        public int FoundMatchIndex = 0;
    }
}
