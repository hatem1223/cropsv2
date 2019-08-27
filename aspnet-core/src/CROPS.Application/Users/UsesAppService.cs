using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Abp.UI;
using CROPS.Dtos;
using CROPS.HttpClientUtility;
using CROPS.Users.Contracts;
using CROPS.Users.DTOs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CROPS.Users
{
    public class UsersAppService : IUsersAppService
    {
        private readonly IHttpClientManager _client;
        private readonly IConfiguration _config;

        public UsersAppService(IHttpClientManager client,IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        public async Task<UserRoleDTO> Create(UserRoleDTO user)
        {
            string url = _config["App:IdentityServerAddress"] + "api/users/";
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(new Uri(url), content);
            if (response.IsSuccessStatusCode)
            {
                using (HttpContent result = response.Content)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var createdUser = JsonConvert.DeserializeObject<UserRoleDTO>(responseBody);
                    return createdUser;
                }
            }
            else
            {
                var result = response.Content.ReadAsStringAsync().Result;
                throw new UserFriendlyException(result);
            }
        }

        public async Task Delete(EntityDto<Guid> user)
        {
            string url = _config["App:IdentityServerAddress"] + "api/users/" + user.Id;
            HttpResponseMessage response = await _client.DeleteAsync(new Uri(url));
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                var result = response.Content.ReadAsStringAsync().Result;
                throw new UserFriendlyException(result);
            }
        }

        public async Task<UserRoleDTO> Get(EntityDto<Guid> input)
        {
            //string url = _config.GetValue<string>("App:IdentityServerAddress") + "api/users/";

            string url = _config["App:IdentityServerAddress"] + "api/users/";
            HttpResponseMessage response = await _client.GetAsync(new Uri(url + input.Id.ToString()));

            if (response.IsSuccessStatusCode)
            {
                using (HttpContent result = response.Content)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var createdUser = JsonConvert.DeserializeObject<UserRoleDTO>(responseBody);
                    return createdUser;
                }
            }
            else
            {
                var result = response.Content.ReadAsStringAsync().Result;
                throw new UserFriendlyException(result);
            }
        }

        public async Task<PagedResultDto<UserRoleDTO>> GetAll(FilteredResultRequestDto input)
        {
            string url = _config["App:IdentityServerAddress"] + "api/users/?pagesize=" +input.MaxResultCount+"&pageNumber="+input.SkipCount;
            HttpResponseMessage response = await _client.GetAsync(new Uri(url));

            if (response.IsSuccessStatusCode)
            {
                using (HttpContent result = response.Content)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<PagedResultDto<UserRoleDTO>>(responseBody);
                    return users;
                }
            }
            else
            {
                var result = response.Content.ReadAsStringAsync().Result;
                throw new UserFriendlyException(result);
            }
        }

        public async Task<UserRoleDTO> Update(UserRoleDTO user)
        {
            string url = _config["App:IdentityServerAddress"] + "api/users/";
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(new Uri( url), content);
            if (response.IsSuccessStatusCode)
            {
                using (HttpContent result = response.Content)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var createdUser = JsonConvert.DeserializeObject<UserRoleDTO>(responseBody);
                    return createdUser;
                }
            }
            else
            {
                var result = response.Content.ReadAsStringAsync().Result;
                throw new UserFriendlyException(result);
            }
        }
    }
}
