using System.Security.Cryptography;
using System.Text;
using XC.RSAUtil;

namespace CNF.Common.Core;

public class RSACrypt
{
    private readonly RsaPkcs1Util _rsaUtil;

    /// <summary>
    /// 实例化
    /// </summary>
    /// <param name="privateKey">私钥</param>
    /// <param name="publicKey">公钥</param>
    public RSACrypt(string privateKey, string publicKey)
    {
        var encoding = Encoding.UTF8;
        _rsaUtil = new RsaPkcs1Util(encoding, publicKey, privateKey, 1024);
    }

    /// <summary>
    /// 获得私钥和公钥
    /// [0]=privateKey  私钥 
    /// [1]=publicKey  公钥
    /// </summary>
    /// <returns></returns>
    public static List<string> GetKey()
    {
        return RsaKeyGenerator.Pkcs1Key(2048, true);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="code">加密代码</param>
    /// <returns></returns>
    public string Encrypt(string code)
    {
        return _rsaUtil.Encrypt(code, RSAEncryptionPadding.Pkcs1);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="code">解密代码</param>
    /// <returns></returns>
    public string Decrypt(string code)
    {
        return _rsaUtil.Decrypt(code, RSAEncryptionPadding.Pkcs1);
    }
}