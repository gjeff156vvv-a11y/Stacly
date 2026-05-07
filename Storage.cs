using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Spectre.Console;

namespace TodoList
{
    public static class Storage
    {
        private static string path = GetPath();

        public static string GetPath()
        {
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TodoList.json");
            if(!File.Exists(path))
	        {
                File.Create(path).Close();
	        }
            return path;
        }

        public static List<Todo> ReadAll()
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                if(fs.Length <= 0)
                    return new List<Todo>();
                else
                    return  JsonSerializer.Deserialize<List<Todo>>(fs);         
            }
        }

        // Новый метод: перезаписывает файл целиком (нужен для удаления и изменения)
        public static void SaveAll(List<Todo> task)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                JsonSerializer.Serialize<List<Todo>>(fs, task);
            }
        }
    }
}
