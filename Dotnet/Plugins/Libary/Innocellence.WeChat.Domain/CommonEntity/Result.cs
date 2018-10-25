namespace Innocellence.WeChat.Domain.CommonEntity
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
    }
}
