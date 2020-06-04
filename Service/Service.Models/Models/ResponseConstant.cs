using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Service.Models.Models
{
    public static class ResponseConstant
    {
        public static BaseResponse OK => new BaseResponse()
        {
            Version = "1.0",
            Datetime = DateTime.UtcNow.ToString("u"),
            Timestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds(),
            Status = "success",
            Code = 200,
            Message = "OK",
            Data = null,
            Errors = null
        };

        public static BaseResponse SAVE_SUCCESS => new BaseResponse()
        {
            Version = "1.0",
            Datetime = DateTime.UtcNow.ToString("u"),
            Timestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds(),
            Status = HttpStatusCode.OK.ToString(),
            Code = (int)HttpStatusCode.OK,
            Message = "Save data success.",
            Data = null,
            Errors = null
        };

        public static BaseResponse DUPLICATE_ENTRY => new BaseResponse()
        {
            Version = "1.0",
            Datetime = DateTime.UtcNow.ToString("u"),
            Timestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds(),
            Status = HttpStatusCode.BadRequest.ToString(),
            Code = (int)HttpStatusCode.BadRequest,
            Message = "Duplicate entry",
            Data = null,
            Errors = null
        };

        public static BaseResponse UPDATE_SUCCESS => new BaseResponse()
        {
            Version = "1.0",
            Datetime = DateTime.UtcNow.ToString("u"),
            Timestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds(),
            Status = HttpStatusCode.OK.ToString(),
            Code = (int)HttpStatusCode.OK,
            Message = "Update data success.",
            Data = null,
            Errors = null
        };

        public static BaseResponse NO_RESULT => new BaseResponse()
        {
            Version = "1.0",
            Datetime = DateTime.UtcNow.ToString("u"),
            Timestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds(),
            Status = HttpStatusCode.BadRequest.ToString(),
            Code = 600,
            Message = "No Result.",
            Data = null,
            Errors = null
        };
    }
}
