using Microsoft.AspNetCore.Mvc;
using OnlineShop.Shared.Interface;
using OnlineShop.Shared.Models;

namespace OnlineShop.Shared.Controllers
{
    public class ApiBaseController : ControllerBase
    {


        /// <summary>
        /// Hàm trả result OK chung
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Dữ liệu trả về</param>
        /// <param name="message">Message trả về</param>
        /// <param name="code">Mã message</param>
        /// <param name="group">Thuộc về module hoặc nhóm thông tin nào</param>
        /// <returns></returns>
        public string ResultOk<T>(T data, string message = "", string code = "")
        {
            return MessageReposity.MessageSerializeObject<T>(true, message ?? "Thành công", data);
        }

        /// <summary>
        /// Hàm trả result Fail chung
        /// </summary>
        /// <param name="e"></param>
        /// <param name="code">Mã message</param>
        /// <param name="group">Thuộc về module hoặc nhóm thông tin nào</param>
        /// <returns></returns>
        public string ResultFail(Exception e, string code = "", string group = "")
        {
            return MessageReposity.MessageSerializeObject<object>(false, e.Message);
        }

        /// <summary>
        /// Hàm trả result Fail chung (Chỉ định message)
        /// </summary>
        /// <param name="message">Message trả về</param>
        /// <param name="code">Mã message</param>
        /// <param name="group">Thuộc về module hoặc nhóm thông tin nào</param>
        /// <returns></returns>
        public string ResultFail(string message, string code = "", string group = "")
        {
            return MessageReposity.MessageSerializeObject<object>(false, message);
        }
    }
}
