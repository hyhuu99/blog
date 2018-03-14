using blog.ApiResponse.Authentication;
using blog.ApiResponse.Interface;
using blog.Code;
using DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace blog.ApiResponse
{
    public  class ApiResponse: IApiResponse
    {
        private static string BaseUrl = ConfigurationManager.AppSettings.Get("apiUrl");
        protected Token token { get; set; }
       public T Get<T>(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync(url).Result;
                return Res.Content.ReadAsAsync<T>().Result;
            }              
        }
        public string Post<T>(string url,T DTO)
        {
            string result = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if(!string.IsNullOrEmpty(Context.token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Context.token);
                HttpResponseMessage Res = client.PostAsJsonAsync(url,DTO).Result;
                if (Res.StatusCode == HttpStatusCode.OK)
                {
                    result = Res.Content.ReadAsStringAsync().Result;
                    return result;
                }
                return "false";
            }
        }
        public string Put<T>(string url, T DTO)
        {
            string result = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Context.token);
                HttpResponseMessage Res = client.PutAsJsonAsync(url, DTO).Result;
                if (Res.StatusCode == HttpStatusCode.OK)
                {
                    result = Res.Content.ReadAsStringAsync().Result;
                    return result;
                }
                return "false";
            }
        }
        public int Delete(string url)
        {
            string result = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Context.token);
                HttpResponseMessage Res = client.DeleteAsync(url).Result;
                if (Res.StatusCode == HttpStatusCode.OK)
                {
                    result = Res.Content.ReadAsStringAsync().Result;
                    return (int)StatusCode.SUCCESS;
                }
                else if (Res.StatusCode == HttpStatusCode.Unauthorized)
                    return (int)StatusCode.UNAUTHORIZED;
                return (int)StatusCode.FAIL;
            }
        }
        public bool Authentication(UserDTO userDTO)
        {
            string Url = "http://localhost:52905/authtoken";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var data = new Dictionary<string, string>()
                {
                {"grant_type", "password"},
                {"Username", userDTO.Email},
                {"Password", userDTO.Password},               
                };
                var req = new HttpRequestMessage(HttpMethod.Post, Url) { Content = new FormUrlEncodedContent(data) };
                HttpResponseMessage Res =  client.SendAsync(req).Result;
                if (Res.StatusCode==HttpStatusCode.OK)
                {
                    token = JsonConvert.DeserializeObject<Token>(Res.Content.ReadAsStringAsync().Result);
                    Context.token = token.AccessToken;
                    return true;
                }
                return false;
            }
        }

    }
          
}