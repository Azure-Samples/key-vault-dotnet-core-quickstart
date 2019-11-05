// <directives>
using System;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
// </directives>

namespace key_vault_console_app
{
    class Program
    {
        static void Main(string[] args)
        {
            string secretName = "mySecret";

            // <authenticate>
            string keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME");
            var kvUri = "https://" + keyVaultName + ".vault.azure.net";

            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            // </authenticate>

            Console.Write("Input the value of your secret > ");
            string secretValue = Console.ReadLine();

            Console.Write("Creating a secret in " + keyVaultName + " called '" + secretName + "' with the value '" + secretValue + "` ...");

            // <setsecret>
            client.SetSecret(secretName, secretValue);
            // </setsecret>

            Console.WriteLine(" done.");

            Console.WriteLine("Forgetting your secret.");
            secretValue = "";
            Console.WriteLine("Your secret is '" + secretValue + "'.");

            Console.WriteLine("Retrieving your secret from " + keyVaultName + ".");

            // <getsecret>
            KeyVaultSecret secret = client.GetSecret(secretName);
            // </getsecret>

            Console.WriteLine("Your secret is '" + secret.Value + "'.");

            Console.Write("Deleting your secret from " + keyVaultName + " ...");

            // <deletesecret>
            client.StartDeleteSecret(secretName);
            // </deletesecret>

            System.Threading.Thread.Sleep(5000);
            Console.WriteLine(" done.");

        }
    }
}
