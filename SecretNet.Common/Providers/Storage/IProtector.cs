namespace SecretNet.Common.Providers.Storage
{
    public interface IProtector
    {
        byte[] Encrypt(byte[] buffer);
        byte[] Decrypt(byte[] buffer);
    }
}