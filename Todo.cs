using System;

namespace TodoList
{
    public class Todo
    {
        public int       ID          {get;private set;}
        public string    Name        {get;private set;}
        public string    Description {get;private set;}
        public bool      IsCompleted {get;private set;}
        public DateTime  CreatedAt   {get;private set;}
        public Priorities  Priority    {get;private set;}
        public string    Tags        {get;private set;}

        public Todo(int id,string name,string description,Priorities? priority,string tags)
        {
            ID = id;
            Name = name;
            Description = description ?? " ";
            IsCompleted = false;
            CreatedAt = DateTime.Now;
            Priority = priority ?? Priorities.Low;
            Tags = tags ?? " ";
        }
    }
}
