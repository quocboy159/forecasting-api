namespace ForecastingSystem.Domain.Interfaces
{
    public interface IForecastingSystemEncryptor {
        string Encrypt(string stringValue);
        string Decrypt(string stringValue);
        ForecastingSystemEncryptor UseEncryptionKey(string key);
    }
}
