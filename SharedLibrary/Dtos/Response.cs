using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public class Response<T> where T : class
    {
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
        public ErrorDto Error { get; private set; }
        // json data'ya dönüştüğünde ignore etsin
        // Bunu kendi iç yapımızda kullanıcaz client bilmeyecek
        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        //Başarılı durumda
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T>() { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        // Başarılı durumda boş data dönebiliriz
        public static Response<T> Success(int statusData)
        {
            return new Response<T>() { Data = default, StatusCode = statusData, IsSuccessful = true };
        }

        // Başarısız durumlar Multi erros
        public static Response<T> Fail(ErrorDto errorDto, int statusCode)
        {
            return new Response<T>() { Error = errorDto, StatusCode = statusCode, IsSuccessful = false };
        }

        // Başarısız single error
        public static Response<T> Fail(string errorMessage, int statusCode, bool isShow)
        {
            var errorDto = new ErrorDto(errorMessage, isShow);

            return new Response<T>() { Error = errorDto, StatusCode = statusCode, IsSuccessful = false };
        }

    }
}
