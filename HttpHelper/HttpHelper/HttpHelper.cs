using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;


public class HttpHelper
{
    private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

    #region HttpGet
    /// <summary>
    /// 使用Get方法获取字符串结果（加入Cookie）
    /// </summary>
    /// <param name="url"></param>
    /// <param name="cookieContainer"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string HttpGet(string url, CookieContainer cookieContainer = null, Encoding encoding = null)
    {
        Stream stream = GetStream(url, cookieContainer);
        StreamReader streamReader = new StreamReader(stream, encoding ?? Encoding.UTF8);
        return streamReader.ReadToEnd();
    }
    /// <summary>
    /// 获取字符流
    /// </summary>
    /// <param name="url"></param>
    /// <param name="cookieContainer"></param>
    /// <returns></returns>
    public static Stream GetStream(string url, CookieContainer cookieContainer)
    {
        HttpWebRequest httpWebRequest = null;
        HttpWebResponse httpWebResponse = null;
        try
        {
            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpWebRequest.CookieContainer = cookieContainer;
            httpWebRequest.Method = "GET";
            httpWebRequest.ServicePoint.ConnectionLimit = int.MaxValue;
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();
            return responseStream;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>  
    /// 创建GET方式的HTTP请求  
    /// </summary>  
    /// <param name="url">请求的URL</param>  
    /// <param name="timeout">请求的超时时间</param>  
    /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
    /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
    /// <returns></returns>  
    public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
    {
        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentNullException("url");
        }
        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        request.Method = "GET";
        request.UserAgent = DefaultUserAgent;
        if (!string.IsNullOrEmpty(userAgent))
        {
            request.UserAgent = userAgent;
        }
        if (timeout.HasValue)
        {
            request.Timeout = timeout.Value;
        }
        if (cookies != null)
        {
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(cookies);
        }
        return request.GetResponse() as HttpWebResponse;
    }

    #endregion
}
