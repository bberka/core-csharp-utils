using System.Security.Cryptography;
using System.Text;

namespace BySfCore.Crypt;

public sealed class BayCrypt
{
  private const int ITERATIONS = 32;

  public BayCrypt(string mainKey, string salt, EncryptionAlgorithm algorithm = EncryptionAlgorithm.AES) {
    MainKey = mainKey;
    Salt = salt;
    Algorithm = algorithm;
  }

  public string Salt { get; }
  public EncryptionAlgorithm Algorithm { get; }
  public string MainKey { get; }

  public byte[] EncryptRaw(byte[] data) {
    using (var algorithm = GetAlgorithm()) {
      using (var encryptor = algorithm.CreateEncryptor()) {
        return encryptor.TransformFinalBlock(data, 0, data.Length);
      }
    }
  }

  public byte[] DecryptRaw(byte[] data) {
    using (var algorithm = GetAlgorithm()) {
      using (var decryptor = algorithm.CreateDecryptor()) {
        return decryptor.TransformFinalBlock(data, 0, data.Length);
      }
    }
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
    SymmetricAlgorithm algorithm;
    switch (Algorithm) {
      case EncryptionAlgorithm.AES:
        algorithm = Aes.Create();
        break;
      case EncryptionAlgorithm.DES:
        algorithm = DES.Create();
        break;
      case EncryptionAlgorithm.RC2:
        algorithm = RC2.Create();
        break;
      case EncryptionAlgorithm.TripleDES:
        algorithm = TripleDES.Create();
        break;
      // case EncryptionAlgorithm.AES256:
      //   algorithm = Aes.Create();
      //   algorithm.KeySize = 256;
      //   break;
      default:
        throw new NotSupportedException("Encryption algorithm not supported.");
    }

    // Set key and IV
    var keyBytes = new Rfc2898DeriveBytes(MainKey, Encoding.UTF8.GetBytes(Salt), hashAlgorithm: HashAlgorithmName.SHA256, iterations: ITERATIONS).GetBytes(algorithm.KeySize / 8);
    algorithm.Key = keyBytes;
    algorithm.IV = keyBytes;

    return algorithm;
  }
}