using System;
using System.Security.Cryptography;
using System.Text;

namespace DAL.Helper
{
    class MemberHashCode
    {

        public static string GenerateMemberCode(Entities.PersonInformation _member)
        {
            try
            {
                string concated = "";
                //_member.PayerType + "-";
                //concated += _member.PolicyID + "-";
                //concated += _member.Gender + "-";
                //concated += _member.DOB + "-";
                //concated += _member.ReferrenceID + "-";
                //concated += _member.InsurerID + "-";

                //concated += _member.PolicyID + "|";

                string HashCOde = Encrypt(concated);
                // return (Encrypt(concated).GetHashCode());
              //  Logger.Info(HashCOde);
                return HashCOde.GetHashCode().ToString();

            }
            catch (Exception ex )
            {
                Operations.Logger.LogError(ex);
                throw;
            }
        }

        public static Entities.PersonInformation GenerateMemberData(Entities.PersonInformation _member)
        {
            try
            {
              //  string concated = "";

                //_member.PayerType + "-";
                //concated += _member.InsurerID + "-";
                //concated += _member.TPAID + "-";
                //concated += _member.FirstName + "-";
                //concated += _member.Gender + "-";
                //concated += _member.DOB + "-";
                //concated += _member.LastName + "-";
                //concated += _member.ReferrenceID + "-";
                //concated += _member.PolicyID ;



                //concated += _member.PolicyID + "|";
                //_member.MemberDataComputed = concated;
                //string HashCOde = Encrypt(concated);
                ////   _member.MemberCode = Member_Hashing.

                //_member.GeneratedHash = HashCOde;
                //string FinalMemberID = HashCOde.GetHashCode().ToString();
                //_member.MemberCode = Math.Abs((Int64.Parse(FinalMemberID))).ToString();




                // return (Encrypt(concated).GetHashCode());
                //  Logger.Info(HashCOde);
                return _member;

            }
            catch (Exception ex)
            {
                Operations.Logger.LogError(ex);
                throw;
            }
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "DHPO_MemberRegister_V2";
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
            //public int id;
            //public string firstname;
            //public string lastname;
            //public string email;

        }


    }
}
