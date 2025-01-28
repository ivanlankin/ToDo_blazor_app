using System;
using System.Collections.Generic;
using System.Text;

namespace ApiClient.Models.ApiModels
{
    public class TaskToDo
    {
        public int Id { get; set; }
        public string? TaskName { get; set; }
        public bool Success { get; set; }
        public string? UserName { get; set; }
    }
}
