using System;
using Spectre.Console;
using System.Text.Json.Serialization;
using System.IO;
using System.Collections.Generic;


namespace TodoList
{
    public class Todo
    {
        public string      Name        {get;private set;}
        public string      Description {get;private set;}
        public bool        IsCompleted {get;private set;}
        public DateTime    CreatedAt   {get;private set;}
        public Priorities  Priority    {get;private set;}
        public string      Tags        {get;private set;}
        public List<Todo>  SubTasks    {get;private set;}

        public Todo(string name,Priorities priority)
        {
            Name = name;
            Description = " ";
            IsCompleted = false;
            CreatedAt = DateTime.Now;
            Priority = priority;
            Tags = " ";
            SubTasks = new List<Todo>();
        }

        [JsonConstructor]
        public Todo(string name,string description,bool isCompleted,DateTime createdAt,Priorities priority,string tags,List<Todo> subTasks)
        {
            Name = name;
            Description = description;
            IsCompleted = isCompleted;
            CreatedAt = createdAt;
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
            IsCompleted = !IsCompleted;
            if(SubTasks.Count == 0 || SubTasks == null)
            {
                return ;
            }
            else 
            {
                foreach(var obj in SubTasks)
                {
                    obj.ChangeIsCompleted();
                }
            }
        }
        public void ChangePriority(Priorities num)
        {
            Priority = num;
        }
        public void ChangeTags(string newTag)
        {
            Tags = newTag;
        }
    }
}
