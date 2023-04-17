using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace CodeTratherTeacher
{
    internal class Cryptog
    {
        ///<summary>
        /// Decrypts a file using AES.
        /// AES Key and IV are pulled from the begining of the file.
        /// inputFile is the path to the file to be decrypted.
        /// outputFile is the path to where the decrypted file is written to.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        public static void decryptFile(string inputFile, string outputFile)
        {
            // Check arguments.
            if (inputFile == null || inputFile.Length <= 0)
                throw new ArgumentNullException("inputFile");
            if (outputFile == null || outputFile.Length <= 0)
                throw new ArgumentNullException("outputFile");

            // Create an Aes object
            using (Aes myAes = Aes.Create())
            {
                // Create byte arrays to get the length of
                // the encrypted key and IV.
                // These values were stored as 4 bytes each
                // at the beginning of the encrypted package.
                byte[] LenK = new byte[4];
                byte[] LenIV = new byte[4];

                // Use FileStream objects to read the encrypted
                // file (inFs) and save the decrypted file (outFs).
                using (var inFs = new FileStream(inputFile, FileMode.Open))
                {
                    inFs.Seek(0, SeekOrigin.Begin);
                    inFs.Read(LenK, 0, 3);
                    inFs.Seek(4, SeekOrigin.Begin);
                    inFs.Read(LenIV, 0, 3);

                    // Convert the lengths to integer values.
                    int lenK = BitConverter.ToInt32(LenK, 0);
                    int lenIV = BitConverter.ToInt32(LenIV, 0);

                    // Determine the start postition of the ciphter text (startC) and its length(lenC).
                    int startC = lenK + lenIV + 8;
                    int lenC = (int)inFs.Length - startC;

                    // Create the byte arrays for the encrypted Aes key and the IV
                    byte[] KeyEncrypted = new byte[lenK];
                    byte[] IV = new byte[lenIV];

                    // Extract the key and IV starting from index 8 after the length values.
                    inFs.Seek(8, SeekOrigin.Begin);
                    inFs.Read(KeyEncrypted, 0, lenK);
                    inFs.Seek(8 + lenK, SeekOrigin.Begin);
                    inFs.Read(IV, 0, lenIV);

                    // Use RSACryptoServiceProvider to decrypt the AES key.
                    string rsaPrivKey = "<RSAKeyValue><Modulus>2hdKHmqbwgm1x6ugtliJs7ImbbI/rYhsq1aKpjG8QKdUKqr7vVKUP+k6eLZeHcrAcAQ08B6gWn4CVAUezkhnAV07oWi7VCjnh5MsZvKSYytsewnbuBdoocjo+4eXVMjt4Jq0RRKqAoCgIwC8RK6CtZV6ENGmkK+ite9Y2s8Zoq0=</Modulus><Exponent>AQAB</Exponent><P>5QN1nMWA+hVflwJY5+h4sK2KoVNsfUi/fLrtNi0S4WBTmNE3nOmsM9oKsOWJD4x2119ChAYLzfJIQNxNWLt2Zw==</P><Q>88pVyw8+kH5bEatyIocezzCWXUE2qSv/LZX3RrmsCvuBaGtJGDrKes8kegjQkmt4JbaWS9vNtF3QRo4dG3npyw==</Q><DP>f4y2s7MYy7Cdxchr5fYHSjfNr158XSboZ7rgpTzjeB0jUkisVbubymFVdQLSnJNaGUgYDtojNvgLH/zTI2l9Xw==</DP><DQ>sTCBlLn6viioZjpXFVNiCDMHRrZMVT7eFDLoa+YtbjoIf21izhKE8ie2GmBnv9QOmlKQAIi8hPielXlbHIpKaw==</DQ><InverseQ>npTpOi55D4qg8smqUV+KIOYyUXTjHmTAyRRyGdhiry+fcSx5sA1+rT78U3G64kYX1yVsoLSMjYqX8OIBsJDfSw==</InverseQ><D>lAIsRho53OT0Hi9HIZlS0sY7uES5XI7ymRFhhUrJpOMqhs6FjEYH4JvrF9NEalmYYi0otDFEyEUuVVEoR/zxEcYROKRh2EjfPHmENVDElI64TDnNItQn4GJ5+2FA2GPaJc8gbf6+TFbRj0fxuOxJHmB721qv41T59WN8eXnl4Y0=</D></RSAKeyValue>";
                    bool DoOAEPPadding = false;

                    byte[] KeyDecrypted;
                    using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                    {
                        RSA.FromXmlString(rsaPrivKey);
                        KeyDecrypted = RSA.Decrypt(KeyEncrypted, DoOAEPPadding);
                    }

                    // Decrypt the key.
                    ICryptoTransform transform = myAes.CreateDecryptor(KeyDecrypted, IV);

                    // Decrypt the cipher text from  the FileSteam of the encrypted file (inFs) into the FileStream for the decrypted file (outFs).
                    using (var outFs = new FileStream(outputFile, FileMode.Create))
                    {
                        int count = 0;
                        int offset = 0;

                        // blockSizeBytes can be any arbitrary size.
                        int blockSizeBytes = myAes.BlockSize / 8;
                        byte[] data = new byte[blockSizeBytes];

                        // By decrypting a chunk a time,
                        // you can save memory and
                        // accommodate large files.

                        // Start at the beginning
                        // of the cipher text.
                        inFs.Seek(startC, SeekOrigin.Begin);
                        using (var outStreamDecrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                        {
                            do
                            {
                                count = inFs.Read(data, 0, blockSizeBytes);
                                offset += count;
                                outStreamDecrypted.Write(data, 0, count);
                            } while (count > 0);

                            outStreamDecrypted.FlushFinalBlock();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Select multiple files to decrypt at once and check that the decrypted version was created.
        /// </summary>
        public static void decryptSubmit()
        {
            OpenFileDialog x = new OpenFileDialog();
            x.Filter = "KatBee Files (*.katb)|*.katb";
            x.Multiselect = true;
            x.ShowDialog();
            string[] result = x.FileNames;
            string[] decrypted = new string[result.Length];
            int index = 0;

            foreach (string inputFilePath in result)
            {
                //MessageBox.Show(y, "Selected Item", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string outFile = GetFileNameAppendVariation(inputFilePath, @"_decrypted");

                outFile = Path.ChangeExtension(outFile, @".zip");

                // decrypt the file
                decryptFile(inputFilePath, outFile);

                decrypted[index] = outFile;
                index++;
            }

            // check to see if decryption was succesful
            bool check = true;
            foreach (string file in decrypted)
            {
                if (!File.Exists(file))
                {
                    MessageBox.Show(file);
                    check = false;
                }

            }
            if (check == true)
            {
                MessageBox.Show("Decryption Succesful!");
            }
            else
            {
                MessageBox.Show("Not all files were decrypted.");
            }
        }

        /// <summary>
        /// helper function to change name of file from file path
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="variation"></param>
        /// <returns></returns>
        private static string GetFileNameAppendVariation(string fileName, string variation)
        {
            string finalPath = Path.GetDirectoryName(fileName);

            string newfilename = String.Concat(Path.GetFileNameWithoutExtension(fileName), variation, Path.GetExtension(fileName));

            return Path.Combine(finalPath, newfilename);
        }
    }
}
