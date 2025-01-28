using Demo.ApiClient.Models;
using Demo.ApiClient.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApiClient
{
    public class DemoApiClientService
    {
        private readonly HttpClient _httpClient;

        public DemoApiClientService(ApiClientOptions apiClientOptions)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri(apiClientOptions.ApiBaseAddress);
        }
        
        public async Task<List<TaskToDo>?> GetTaskToDos()
        {
            return await _httpClient.GetFromJsonAsync<List<TaskToDo>?>("/api/TaskToDo");
        }

        public async Task<TaskToDo?> GetById(int id)
        {
            return await _httpClient.GetFromJsonAsync<TaskToDo?>($"/api/TaskToDo/{id}");
        }

        public async Task SaveTaskToDo(TaskToDo taskToDo)
        {
            await _httpClient.PostAsJsonAsync("/api/TaskToDo", taskToDo);
        }

        public async Task UpdateTaskToDo(TaskToDo taskToDo)
        {
            await _httpClient.PutAsJsonAsync("/api/TaskToDo", taskToDo);
        }

        public async Task DeleteTaskToDo(int id)
        {
            await _httpClient.DeleteAsync($"/api/TaskToDo/{id}");
        }

        public async Task<List<UserAccount>?> GetUser()
        {
            return await _httpClient.GetFromJsonAsync<List<UserAccount>?>("/api/UserAccount");
        }

        public async Task SaveUser(UserAccount userAccount)
        {
            await _httpClient.PostAsJsonAsync("/api/UserAccount", userAccount);
        }

        public async Task UpdateUser(UserAccount userAccount)
        {
            await _httpClient.PutAsJsonAsync("/api/UserAccount", userAccount);
        }

        public async Task DeleteUser(int id)
        {
            await _httpClient.DeleteAsync($"/api/UserAccount/{id}");
        }
    }
}
