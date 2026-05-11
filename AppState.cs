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

        //главный список для сохранения
        public List<Todo>  RootList {get; private set;}

        public AppCoordinator()
        {
            RootList = Storage.ReadAll();
        }

        //паременты для изменения текстовых значений
        public string InputBuffer = "";
        public EditingField EditingField = EditingField.Name;
        public Todo? EditingTodo = null;

        //паременты для поиска 
        public string SearchBuffer = "";
        public List<Todo> FoundItems = null;
        public int FoundMatchIndex = 0;
        public string TagSuggestion = ""; // Хранит найденный вариант (например, "work")

        //сохранения
        public string? Message {get;private set;}
        public DateTime MessageUtil {get;private set;}
        public bool MessageError {get;private set;}

        public void SetMessage(string message,bool error,int second = 3)
        {
            Message = message;
            MessageUtil = DateTime.Now.AddSeconds(second);
            MessageError = error;
        }

        

    }
}
