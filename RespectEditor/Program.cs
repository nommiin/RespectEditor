using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace RespectEditor {
    class Program {
        public static byte[] Key = new byte[] { 0x44, 0x43, 0x46, 0x32, 0x33, 0x35, 0x37, 0x37, 0x45, 0x39, 0x41, 0x46, 0x34, 0x35, 0x42, 0x30, 0x38, 0x38, 0x31, 0x42, 0x35, 0x35, 0x46, 0x44, 0x34, 0x37, 0x42, 0x35, 0x41, 0x34, 0x46, 0x38 };
        public static byte[] IV = new byte[] { 0x36, 0x41, 0x39, 0x37, 0x42, 0x30, 0x35, 0x39, 0x32, 0x30, 0x43, 0x43, 0x34, 0x41, 0x37, 0x41 };
        static void Main(string[] args) {
            if (args.Length >= 2) {
                if (File.Exists(args[1]) == true) {
                    switch (args[0].Trim().ToLower()) {
                        case "encrypt": {
                            byte[] _result = Encrypt(File.ReadAllText(args[1]));
                            if (args.Length > 2) {
                                File.WriteAllBytes(args[2], _result);
                            } else {
                                File.WriteAllBytes(Path.ChangeExtension(args[1], ".dat"), _result);
                            }
                            break;
                        }

                        case "decrypt": {
                            string _result = Decrypt(File.ReadAllBytes(args[1]));
                            if (args.Length > 2) {
                                File.WriteAllText(args[2], _result);
                            } else {
                                File.WriteAllText(Path.ChangeExtension(args[1], ".json"), _result);
                            }
                            break;
                        }
                    }
                } else {
                    Console.WriteLine("RespectEditor ERROR: The provided file \"" + args[1] + "\" does not exist!");
                }
            } else {
                Console.WriteLine("RespectEditor Usage:\n  - RespectEditor encrypt <path>[.json]\n  - RespectEditor decrypt <path>[.dat]");
            }
        }

        public static byte[] Encrypt(string _data) {
            using (Aes aesInstance = Aes.Create()) {
                aesInstance.Key = Key;
                aesInstance.IV = IV;

                ICryptoTransform aesEncrypt = aesInstance.CreateEncryptor(aesInstance.Key, aesInstance.IV);
                using (MemoryStream msEncrypt = new MemoryStream()) {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesEncrypt, CryptoStreamMode.Write)) {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) {
                            swEncrypt.Write(_data);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        public static string Decrypt(byte[] _data) {
            using (Aes aesAlg = Aes.Create()) {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform aesDecrypt = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(_data)) {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesDecrypt, CryptoStreamMode.Read)) {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
