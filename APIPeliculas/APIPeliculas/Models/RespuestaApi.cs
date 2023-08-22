using System.Net;
using XAct.Messages;

namespace APIPeliculas.Models
{
    public class RespuestaApi
    {

        public RespuestaApi()
        {
            ErrorMessages = new List<string>();
        }
      

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
/*
   public RespuestaApi(HttpStatusCode statusCode, bool isSuccess, List<string> errorMessages, object result)
        {
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            ErrorMessages = errorMessages;
            Result = result;
            
        }

  */