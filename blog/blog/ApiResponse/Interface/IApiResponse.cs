using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace blog.ApiResponse.Interface
{
    public interface IApiResponse
    {
        T Get<T>(string url);
        string Post<T>(string url, T DTO);
        string Put<T>(string url, T DTO);
        int Delete(string url);
        bool Authentication(UserDTO userDTO);
    }
}