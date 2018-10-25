namespace Innocellence.WeChat.Domain.Services
{
    /// <summary>
    /// some common service for common usage
    /// </summary>
    public class CommonDomainService
    {
        /// <summary>
        /// 投票中根据index的012转换成选项ABC
        /// </summary>
        /// <param name="i">index，0 based</param>
        /// <returns></returns>
        public static string Convert012ToABC(int i)
        {
            var asciiCode = i + 'A';
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                return "";
                //throw new Exception("ASCII Code is not valid.");
            }
        }
    }
}