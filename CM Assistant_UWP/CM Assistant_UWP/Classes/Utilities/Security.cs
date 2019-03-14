using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using System.Security.Cryptography;

namespace CM_Assistant_UWP.Classes.Utilities
{
    class Security
    {
        public static bool SetPassword(string input)
        {
            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                //Try to add a password to the Windows Credential store
                try
                {
                    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                    Random rng = new Random();
                    localSettings.Values["salt"] = rng.Next(int.MinValue, int.MaxValue);
                    //Set salt to a random integer

                    var password = new Windows.Security.Credentials.PasswordCredential
                    {
                        UserName = "user",
                        Password = Encoding.Default.GetString(GenerateHash(input, localSettings.Values["salt"].ToString())),
                        //Store the string representation of the password entered salted with the random integer created earlier
                        Resource = "CM-Assistant"
                    };

                    vault.Add(password);
                    return true;
                }
                //If it fails then there must already be a password set
                //Remove it and then set again
                catch (Exception)
                {
                    vault.Remove(vault.FindAllByUserName("user").Single());
                    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                    Random rng = new Random();
                    localSettings.Values["salt"] = rng.Next(int.MinValue, int.MaxValue);
                    //Set salt to a random integer

                    var password = new Windows.Security.Credentials.PasswordCredential
                    {
                        UserName = "user",
                        Resource = "CM-Assistant",
                        Password = Encoding.Default.GetString(GenerateHash(input, localSettings.Values["salt"].ToString()))
                        //Store the string representation of the password entered salted with the random integer created earlier
                    };
                    return true;
                }
            }
            catch (Exception)
            {
                //Return false if all fails
                return false;
            }
        }
        /// <summary>
        /// Sets a new password which the user must enter correctly at launch
        /// </summary>
        /// <param name="input">The password to be set</param>
        /// <returns></returns>

        public static bool CheckPassword(string input)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            //Check if there is a password set already
            if (vault.FindAllByUserName("user").Single() != null)
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                Windows.Security.Credentials.PasswordCredential temp = vault.FindAllByUserName("user").Single();
                temp.RetrievePassword();

                //Return true or false if the hash of the password provided matches the one stored
                if (temp.Password == Encoding.Default.GetString(GenerateHash(input, localSettings.Values["salt"].ToString())))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //Return false if there is no password set
                return false;
            }
        }
        /// <summary>
        /// Returns true if the provided password is correct
        /// </summary>

        public static void RemovePassword()
        {
            try
            {
                //Try to remove the password from the vault
                var vault = new Windows.Security.Credentials.PasswordVault();
                vault.Remove(vault.FindAllByUserName("user").First());
            }
            catch (Exception e)
            {
                //Inform the user that there was an error in the event that the password cannot be removed
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Error removing password" + Environment.NewLine + e.Message,
                    CloseButtonText = "Okay"
                };
            }
        }
        /// <summary>
        /// Removes the password from the vault
        /// </summary>
        /// 
   
        public static byte[] GenerateHash(string input, string salt)
        {
            SHA256 encrypt = SHA256.Create();
            encrypt.ComputeHash(Encoding.UTF8.GetBytes(input).Concat(Encoding.UTF8.GetBytes(salt)).ToArray());
            //Calculate the SHA256 hash of the input and the salt concatenated together 

            byte[] output = encrypt.Hash;
            encrypt.Dispose();
            return output;
            //Save the hash before disposing of the SHA256 object, then return the hash

        }
    }
}
