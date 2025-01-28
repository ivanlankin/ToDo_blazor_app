using BlazorWebApp.Models;
using BlazorWebApp.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BlazorWebApp.RabbitMq;
using System.Text.Json;

namespace BlazorWebApp
{
    public class DemoApiClientService
    {
        private readonly IRabbitMqService _rabbitMqService;
        private Task response;

        public DemoApiClientService(IRabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        public async Task<List<TaskToDo>?> GetTaskToDos()
        {
            var request = new { Action = "\"GetTaskToDos\"", Model = new { }, Id = 0 };
            string response = await _rabbitMqService.SendMessage(request.ToString());
            return await Task.FromResult(JsonSerializer.Deserialize<List<TaskToDo>?>(response));
            
        }

        public async Task<TaskToDo> GetById(int id)
        {
            TaskToDo ex = new TaskToDo();
            var request = new { Action = "\"GetById\"", Model = ex, Id = id };
            string response = await _rabbitMqService.SendMessage(request.ToString());
            return await Task.FromResult(JsonSerializer.Deserialize<TaskToDo>(response));
        }

        public async Task<List<UserAccount>?> GetUser()
        {
            UserAccount user = new UserAccount();
            var request = new { Action = "\"GetUser\"", Model = user, Id = 0 };
            string response = await _rabbitMqService.SendMessage(request.ToString());
            return await Task.FromResult(JsonSerializer.Deserialize<List<UserAccount>?>(response));
        }

        public async Task<TaskToDo> SaveTaskToDo(TaskToDo taskToDo)
        {
            var request = new { Action = "\"SaveTaskToDo\"", Model = taskToDo, Id = 0 };
            string response = await _rabbitMqService.SendMessage(request.ToString());
            return await Task.FromResult(JsonSerializer.Deserialize<TaskToDo>(response));
        }

        public async Task<TaskToDo> UpdateTaskToDo(TaskToDo taskToDo)
        {
            var request = new { Action = "\"UpdateTaskToDo\"", Model = taskToDo, Id = 0 };
            string response = await _rabbitMqService.SendMessage(request.ToString());
            return await Task.FromResult(JsonSerializer.Deserialize<TaskToDo>(response));
        }

        public async Task<TaskToDo> DeleteTaskToDo(int id)
        {
            TaskToDo ex = new TaskToDo();
            var request = new { Action = "\"DeleteTaskToDo\"", Model = ex, Id = id };
            string response = await _rabbitMqService.SendMessage(request.ToString());
            return await Task.FromResult(JsonSerializer.Deserialize<TaskToDo>(response));
        }

        public async Task<UserAccount> SaveUser(UserAccount userAccount)
        {
            var request = new { Action = "\"SaveUser\"", Model = userAccount, Id = 0 };
            string response = await _rabbitMqService.SendMessage(request.ToString());
            return await Task.FromResult(JsonSerializer.Deserialize<UserAccount>(response));
        }

        public async Task<UserAccount> UpdateUser(UserAccount userAccount)
        {
            var request = new { Action = "\"UpdateUser\"", Model = userAccount, Id = 0 };
            string response = await _rabbitMqService.SendMessage(request.ToString());
            return await Task.FromResult(JsonSerializer.Deserialize<UserAccount>(response));
        }

        public async Task<UserAccount> DeleteUser(int id)
        {
            UserAccount user = new UserAccount();
            var request = new { Action = "\"DeleteUser\"", Model = user, Id = id };
            string response = await _rabbitMqService.SendMessage(request.ToString());
            return await Task.FromResult(JsonSerializer.Deserialize<UserAccount>(response));
        }

    }
}
