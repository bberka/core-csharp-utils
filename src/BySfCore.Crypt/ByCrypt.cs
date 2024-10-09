using System.Security.Cryptography;
using System.Text;

namespace BySfCore.Crypt;

public sealed class ByCrypt
{
  private const int ITERATIONS = 32;

  public ByCrypt(string mainKey, string salt, EncryptionAlgorithm algorithm = EncryptionAlgorithm.AES) {
    MainKey = mainKey;
    Salt = salt;
    Algorithm = algorithm;
  }

  public string Salt { get; }
  public EncryptionAlgorithm Algorithm { get; }
  public string MainKey { get; }

  public byte[] EncryptRaw(byte[] data) {
    using var algorithm = GetAlgorithm();
    using var encryptor = algorithm.CreateEncryptor();
    return encryptor.TransformFinalBlock(data, 0, data.Length);
  }

  public byte[] DecryptRaw(byte[] data) {
    using var algorithm = GetAlgorithm();
    using var decryptor = algorithm.CreateDecryptor();
    return decryptor.TransformFinalBlock(data, 0, data.Length);
  }

  public string Encrypt(string data) {
    var plainBytes = Encoding.UTF8.GetBytes(data);
    var encryptedBytes = EncryptRaw(plainBytes);
    return Convert.ToBase64String(encryptedBytes);
  }

  public string Decrypt(string data) {
    var encryptedBytes = Convert.FromBase64String(data);
    var decryptedBytes = DecryptRaw(encryptedBytes);
    return Encoding.UTF8.GetString(decryptedBytes);
  }

  private SymmetricAlgorithm GetAlgorithm() {
    SymmetricAlgorithm algorithm = Algorithm switch {
      EncryptionAlgorithm.AES => Aes.Create(),
      EncryptionAlgorithm.DES => DES.Create(),
      EncryptionAlgorithm.RC2 => RC2.Create(),
      EncryptionAlgorithm.TripleDES => TripleDES.Create(),
      _ => throw new NotSupportedException("Encryption algorithm not supported.")
    };

    // Set key and IV
    var keyBytes = new Rfc2898DeriveBytes(MainKey, Encoding.UTF8.GetBytes(Salt), hashAlgorithm: HashAlgorithmName.SHA256, iterations: ITERATIONS).GetBytes(algorithm.KeySize / 8);
    algorithm.Key = keyBytes;
    algorithm.IV = keyBytes;

    return algorithm;
  }
}