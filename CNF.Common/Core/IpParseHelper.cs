using System.Net;
using System.Text;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace CNF.Common.Core;

public class IpParseHelper
{
    /// <summary>
    /// 获取电脑 MAC(物理)地址
    /// </summary>
    /// <returns></returns>
    public static string GetMACIp()
    {
        //本地计算机网络连接信息
        IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
        //获取本机所有网络连接
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

        //获取本机电脑名
        var HostName = computerProperties.HostName;
        //获取域名
        var DomainName = computerProperties.DomainName;

        if (nics == null || nics.Length < 1)
        {
            return "";
        }

        var MACIp = "";
        foreach (NetworkInterface adapter in nics)
        {
            var adapterName = adapter.Name;

            var adapterDescription = adapter.Description;
            var NetworkInterfaceType = adapter.NetworkInterfaceType;
            if (adapterName == "本地连接")
            {
                PhysicalAddress address = adapter.GetPhysicalAddress();
                byte[] bytes = address.GetAddressBytes();

                for (int i = 0; i < bytes.Length; i++)
                {
                    MACIp += bytes[i].ToString("X2");

                    if (i != bytes.Length - 1)
                    {
                        MACIp += "-";
                    }
                }
            }
        }

        return MACIp;
    }

    public static string GetAddressByIP(string IP)
    {
        try
        {
            if (string.IsNullOrEmpty(IP))
            {
                return IP;
            }

            string url = "http://whois.pconline.com.cn/ipJson.jsp?callback=testJson&ip=" + IP;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "text/html;chartset=UTF-8";
            request.UserAgent =
                "Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 48.0) Gecko / 20100101 Firefox / 48.0"; //火狐用户代理
            request.Method = "GET";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream streamResponse = response.GetResponseStream())
                {
                    using (StreamReader streanReader = new StreamReader(streamResponse, Encoding.GetEncoding("gb2312")))
                    {
                        string retString = streanReader.ReadToEnd();

                        string t = retString.Substring(retString.IndexOf("{\""),
                            retString.IndexOf(");}") - retString.IndexOf("{\""));
                        Ipinfos m = (Ipinfos)JsonConvert.DeserializeObject(t, typeof(Ipinfos));

                        string IPProvince = m?.Pro == "" ? "其它地区" : m.Pro;
                        string IPCity = m?.City;
                        return $"{IPProvince}-{IPCity}";
                    }
                }
            }
        }
        catch
        {
            return string.Empty;
        }
    }

    public class Ipinfos
    {
        public string Pro { get; set; }
        public string City { get; set; }
    }
}