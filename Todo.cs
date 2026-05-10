using System;
using Spectre.Console;
using System.Text.Json.Serialization;
using System.IO;
using System.Collections.Generic;


namespace Stacly
{
    public class Todo
    {
        public string       Name        {get;private set;}
        public string       Description {get;private set;}
        public bool         IsCompleted {get;private set;}
        public DateTime     CreatedAt   {get;private set;}
        public DateTime?    CompleteAt  {get;private set;}
        public Priorities   Priority    {get;private set;}
        public List<string> Tags        {get;private set;}
        public List<Todo>   SubTasks    {get;private set;}

        public Todo(string name,Priorities priority)
        {
            Name = name;
            Description = " ";
            IsCompleted = false;
            CreatedAt = DateTime.Now;
            Priority = priority;
            Tags = new List<string>();
            SubTasks = new List<Todo>();
        }

        [JsonConstructor]
        public Todo(string name,string description,bool isCompleted,DateTime createdAt,DateTime? completeAt,Priorities priority,List<string> tags,List<Todo> subTasks)
        {
            Name = name;
            Description = description;
            IsCompleted = isCompleted;
            CreatedAt = createdAt;
            CompleteAt = completeAt;
            Priority = priority;
            Tags = tags;
            SubTasks = subTasks;
        }

        public void ChangeName(string newName)
        {
            Name = newName;
        }
        public void ChangeDescription(string newDescription)
        {
            Description = newDescription;
        }
        public void ChangeIsCompleted()
        {
            ChangeIsCompleted(!IsCompleted);
        }

        public void ChangeIsCompleted(bool status)
        {
            IsCompleted = status;
            if(SubTasks.Count > 0)
            {
                foreach(var obj in SubTasks)
                {
                    obj.ChangeIsCompleted(status);
                }
            }

            switch(status)
            {
                case false:CompleteAt = null;break;
                case true: CompleteAt = DateTime.Now;break;
            }

        }
        public void ChangePriority(Priorities num)
        {
            Priority = num;
        }
        public void ChangeTags(string newTag)
        {
            // Разделяем, убираем пробелы по краям и фильтруем пустые записи
            Tags = newTag.Split(',', StringSplitOptions.RemoveEmptyEntries)
                 .Select(t => t.Trim())
                 .ToList();        
        }
    }
}
