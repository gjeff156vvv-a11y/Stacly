using System;
using System.Collections.Generic;

namespace TodoList
{
    public class AppState 
    {
        public bool Running = true;
        public bool IsExpanded = false;
        public int SelectedIndex = 0;
        public string Buffer = "";
        public Mods Mod = Mods.List;
        public string EditingField = "";
    }
}
