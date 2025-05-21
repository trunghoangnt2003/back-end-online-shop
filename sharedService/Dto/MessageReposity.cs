using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace OnlineShop.Shared.Models
{
    public static class MessageReposity
    {
        const string SUCCESS = "Thành công";
        const string FAIL = "Thất bại";
        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
            WriteIndented = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            //ReferenceHandler = ReferenceHandler.Preserve
        };

        public static string MessageSerializeObject<T>(bool status, string message, T item, string code = "")
        {
            try
            {
                return System.Text.Json.JsonSerializer.Serialize(new MessageDto<T>
                {
                    status = status,
                    message = message,
                    data = item,
                    code = code
                }, options);
            }
            catch (Exception ex)
            {
                return System.Text.Json.JsonSerializer
                                    .Serialize(
                                    new MessageDto
                                    {
                                        status = false,
                                        message = ex.Message,
                                        code = nameof(FAIL)
                                    }, options);
            }
        }

        public static string MessageSerializeObject<T>(bool status, string message, string code = "")
        {
            try
            {
                return System.Text.Json.JsonSerializer.Serialize(new MessageDto
                {
                    status = status,
                    message = message,
                    data = null,
                    code = code
                }, options);

            }
            catch (Exception ex)
            {
                return System.Text.Json.JsonSerializer
                                    .Serialize(
                                    new MessageDto
                                    {
                                        status = false,
                                        message = ex.Message,
                                        data = null,
                                        code = nameof(FAIL)
                                    }, options);
            }
        }
    }

    public class MessageDto
    {
        /// <summary>
        /// Trạng thái quy định thất bại hay thành công
        /// </summary>
        public bool status { set; get; }

        /// <summary>
        /// Message mô tả
        /// </summary>
        public string message { set; get; }

        /// <summary>
        /// Mã thông báo/Lỗi
        /// </summary>
        public string code { set; get; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public object data { set; get; }
    }

    public class MessageDto<T>
    {
        /// <summary>
        /// Trạng thái quy định thất bại hay thành công
        /// </summary>
        public bool status { set; get; }

        /// <summary>
        /// Message mô tả
        /// </summary>
        public string message { set; get; }

        /// <summary>
        /// Mã thông báo/Lỗi
        /// </summary>
        public string code { set; get; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public T data { set; get; }
    }
}
