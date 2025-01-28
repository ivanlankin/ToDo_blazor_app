using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ApiClient.Models;
using ApiClient.Models.ApiModels;
using ApiClient.RabbitMq;

namespace ApiClient
{
    public class DemoApiClientService
    {
        private readonly InterfaceRabbitMQ _rabbitMqService;

        public DemoApiClientService(InterfaceRabbitMQ rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        public async Task<List<TaskToDo>?> GetTaskToDos()
        {
            var response = await _rabbitMqService.SendMessageWithResponse("", "get_task_todos");
            return JsonSerializer.Deserialize<List<TaskToDo>>(response);
        }

        public async Task<TaskToDo?> GetById(int id)
        {
            var response = await _rabbitMqService.SendMessageWithResponse(id.ToString(), "get_task_todo_by_id");
            return JsonSerializer.Deserialize<TaskToDo>(response);
        }

        public async Task SaveTaskToDo(TaskToDo taskToDo)
        {
            var message = JsonSerializer.Serialize(taskToDo);
            await _rabbitMqService.SendMessageWithResponse(message, "save_task_todo");
        }

        public async Task UpdateTaskToDo(TaskToDo taskToDo)
        {
            var message = JsonSerializer.Serialize(taskToDo);
            await _rabbitMqService.SendMessageWithResponse(message, "update_task_todo");
        }

        public async Task DeleteTaskToDo(int id)
        {
            await _rabbitMqService.SendMessageWithResponse(id.ToString(), "delete_task_todo");
        }

        public async Task<List<UserAccount>?> GetUser()
        {
            var response = await _rabbitMqService.SendMessageWithResponse("", "get_users");
            return JsonSerializer.Deserialize<List<UserAccount>>(response);
        }

        public async Task SaveUser(UserAccount userAccount)
        {
            var message = JsonSerializer.Serialize(userAccount);
            await _rabbitMqService.SendMessageWithResponse(message, "save_user");
        }

        public async Task UpdateUser(UserAccount userAccount)
        {
            var message = JsonSerializer.Serialize(userAccount);
            await _rabbitMqService.SendMessageWithResponse(message, "update_user");
        }

        public async Task DeleteUser(int id)
        {
            await _rabbitMqService.SendMessageWithResponse(id.ToString(), "delete_user");
        }
    }
}
