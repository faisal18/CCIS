using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace BatchToPR
{
    class Member_Hashing
    {
        public void Run()
        {
            Member obj_mem = new Member
            {
                id = 123,
                firstname = "Faisal",
                lastname = "Ansari",
                email = "fansari.u@gmail.com"
            };

            string concated = obj_mem.id + "|" + obj_mem.firstname + "|" + obj_mem.lastname + "|" + obj_mem.email;
            string variable_result = string.Empty;
            try
            {
                variable_result = Encrypt(concated);

                Console.WriteLine("Input String: " + concated);
                Console.WriteLine("Encrypted: " + variable_result);
                Console.WriteLine("Encrypted GetSha256Hash: " + GetSha256Hash(new SHA256Managed(), variable_result));
                Console.WriteLine("Encrypted GetSha256Hash XOR: " + xorIt("abc", GetSha256Hash(new SHA256Managed(), variable_result)));
                Console.WriteLine("Encrypted GetSha256Hash HEXA: " + BitConverter.ToString(Encoding.Default.GetBytes(concated)).Replace("-", ""));
                Console.WriteLine("Encrypted GetSha256Hash HEXA LZW: " + LZW.LZWCompressed(BitConverter.ToString(Encoding.Default.GetBytes(concated)).Replace("-", "")));

                Console.WriteLine("Encrypted Hashed: " + variable_result.GetHashCode());
                Console.WriteLine("Encrypted GetHashCodeInt64: " + variable_result);
                Console.WriteLine("Encrypted SHA256 Hashed: " + hash_sha256(variable_result));
                Console.WriteLine("Encrypted SHA512 hashed: " + hash_sha512(variable_result));
                Console.WriteLine("Encrypted Base64: " + Convert.ToBase64String(Encoding.UTF8.GetBytes(variable_result)));
                Console.WriteLine("Decrypted String: " + Decrypt(variable_result));

                Console.WriteLine("\n");

                string ED_string = EncryptDecrypt.ToEncrypt(concated);
                Console.WriteLine("ED Encrypt: " + ED_string);
                Console.WriteLine("ED GetSha256Hash: " + GetSha256Hash(new SHA256Managed(), ED_string));
                Console.WriteLine("ED GetSha256Hash XORiT: " + xorIt("abc", GetSha256Hash(new SHA256Managed(), ED_string)));
                Console.WriteLine("ED GetSha256Hash HEXA: " + BitConverter.ToString(Encoding.Default.GetBytes(ED_string)).Replace("-", ""));
                Console.WriteLine("ED GetSha256Hash HEXA LZW: " + LZW.LZWCompressed(BitConverter.ToString(Encoding.Default.GetBytes(ED_string)).Replace("-", "")));
                Console.WriteLine("ED Encrypt Hashed: " + ED_string.GetHashCode());
                Console.WriteLine("ED GetHashCodeInt64: " + GetHashCodeInt64(ED_string));
                Console.WriteLine("ED SHA256 hashed: " + hash_sha256(ED_string));
                Console.WriteLine("ED SHA512 hashed: " + hash_sha512(ED_string));


                Console.WriteLine("Run Again ? \nY/N");
                var key = Console.ReadKey();
                if (key.KeyChar == 'y')
                {
                    Run();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured !!!");
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "DHPOMemeber";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "DHPOMemeber";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string GetSha256Hash(SHA256 shaHash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string xorIt(string key, string input)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
                sb.Append((char)(input[i] ^ key[(i % key.Length)]));
            String result = sb.ToString();

            return result;
        }

        public static string hash_sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        public static string hash_sha512(string randomstring)
        {
            using (SHA512 sham = new SHA512Managed())
            {
                var result = sham.ComputeHash(Encoding.UTF8.GetBytes(randomstring));
                return Encoding.UTF8.GetString(result);
            }

        }

        public static long GetHashCodeInt64(string input)
        {
            var s1 = input.Substring(0, input.Length / 2);
            var s2 = input.Substring(input.Length / 2);

            var x = 0;//((long)s1.GetHashCode()) << 0x20 | s2.GetHashCode();

            return x;
        }

        static void MD5Create(string concated)
        {
            string source = concated;
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, source);

                Console.WriteLine("The MD5 hash of " + source + " is: " + hash + ".");

                Console.WriteLine("Verifying the hash...");

                if (VerifyMd5Hash(md5Hash, source, hash))
                {
                    Console.WriteLine("The hashes are the same.");
                }
                else
                {
                    Console.WriteLine("The hashes are not same.");
                }
            }
        }
        static string XOR(string data)
        {
            byte[] theKey = new byte[] { 34, 56, 98 };
            int theValue = int.Parse(data);

            int cyphered = ((theValue & 0xff) ^ theKey[0]) |
           ((((theValue >> 8) & 0xff) ^ theKey[1]) << 8) |
           ((((theValue >> 16) & 0xff) ^ theKey[2]) << 16);

            string finalValue = cyphered.ToString().PadLeft(7, '0');

            int scyphered = int.Parse(finalValue);

            int decyphered = ((scyphered & 0xff) ^ theKey[0]) |
                             ((((scyphered >> 8) & 0xff) ^ theKey[1]) << 8) |
                             ((((scyphered >> 16) & 0xff) ^ theKey[2]) << 16);

            return decyphered.ToString();
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int GetStableHash(string s)
        {
            int MUST_BE_LESS_THAN = 1000000000; // 9 decimal digits

            uint hash = 0;
            // if you care this can be done much faster with unsafe 
            // using fixed char* reinterpreted as a byte*
            foreach (byte b in System.Text.Encoding.Unicode.GetBytes(s))
            {
                hash += b;
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }
            // final avalanche
            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);
            // helpfully we only want positive integer < MUST_BE_LESS_THAN
            // so simple truncate cast is ok if not perfect
            return (int)(hash % MUST_BE_LESS_THAN);
        }

        public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }
        public static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public struct Member
        {
            public int id;
            public string firstname;
            public string lastname;
            public string email;

        }

    }


    public class EncryptDecrypt
    {
        #region Declaration

        static readonly byte[] TripleDesKey1 = new byte[] { 15, 11, 7, 21, 34, 32, 33, 5, 23, 13, 23, 41, 43, 41, 7, 19, 91, 91, 47, 7, 37, 13, 19, 41 };
        static readonly byte[] TripleDesiv1 = new byte[] { 5, 23, 13, 23, 41, 43, 41, 7 };

        #endregion


        /// <summary>
        /// To Encrypt String
        /// </summary>
        /// <param name="value">String To Encrypt</param>
        /// <returns>Returns Encrypted String</returns>
        public static string ToEncrypt(string value)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider
            {
                Key = TripleDesKey1,
                IV = TripleDesiv1
            };

            MemoryStream ms;

            if (value.Length >= 1)
                ms = new MemoryStream(((value.Length * 2) - 1));
            else
                ms = new MemoryStream();

            ms.Position = 0;
            CryptoStream encStream = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] plainBytes = System.Text.Encoding.UTF8.GetBytes(value);
            encStream.Write(plainBytes, 0, plainBytes.Length);
            encStream.FlushFinalBlock();
            encStream.Close();

            return Convert.ToBase64String(plainBytes);
        }

        /// <summary>
        /// To Decrypt Data Encrypted From TripleDEC Algoritham
        /// </summary>
        /// <param name="value">String Value To Decrypt</param>
        /// <returns>Return Decrypted Data</returns>
        public static string ToDecrypt(string value)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            //System.IO.MemoryStream ms = new System.IO.MemoryStream(((value.Length * 2) - 1));
            MemoryStream ms;
            if (value.Length >= 1)
                ms = new MemoryStream(((value.Length * 2) - 1));
            else
                ms = new MemoryStream();

            ms.Position = 0;
            CryptoStream encStream = new CryptoStream(ms, des.CreateDecryptor(TripleDesKey1, TripleDesiv1), CryptoStreamMode.Write);
            byte[] plainBytes = Convert.FromBase64String(value);
            encStream.Write(plainBytes, 0, plainBytes.Length);
            return System.Text.Encoding.UTF8.GetString(plainBytes);
        }

    }

    public class HexadecimalEncoding
    {
        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }

        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
        }
    }

    public class LZW
    {
        public static string LZWCompressed(string data)
        {
            List<int> compressed = Compress(data);

            Console.WriteLine("LZW DECOMPReSS: " + LZWDecompress(compressed));

            return string.Join(", ", compressed);
        }

        public static string LZWDecompress(List<int> data)
        {
            string decompressed = Decompress(data);
            return decompressed;
        }
        public static List<int> Compress(string uncompressed)
        {
            // build the dictionary
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(((char)i).ToString(), i);

            string w = string.Empty;
            List<int> compressed = new List<int>();

            foreach (char c in uncompressed)
            {
                string wc = w + c;
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {
                    // write w to output
                    compressed.Add(dictionary[w]);
                    // wc is a new sequence; add it to the dictionary
                    dictionary.Add(wc, dictionary.Count);
                    w = c.ToString();
                }
            }

            // write remaining output if necessary
            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);

            return compressed;
        }

        public static string Decompress(System.Collections.Generic.List<int> compressed)
        {
            // build the dictionary
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(i, ((char)i).ToString());

            string w = dictionary[compressed[0]];
            compressed.RemoveAt(0);
            StringBuilder decompressed = new StringBuilder(w);

            foreach (int k in compressed)
            {
                string entry = null;
                if (dictionary.ContainsKey(k))
                    entry = dictionary[k];
                else if (k == dictionary.Count)
                    entry = w + w[0];

                decompressed.Append(entry);

                // new sequence; add it to the dictionary
                dictionary.Add(dictionary.Count, w + entry[0]);

                w = entry;
            }

            return decompressed.ToString();
        }
    }
}