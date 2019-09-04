// <directives>
using System;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
// </directives>

namespace akvdotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            Program P = new Program();
            string secretName = "mySecret";

            // kvURL must be updated to the URL of your key vault
            string kvURL = "https://akvdotnetqs.vault.azure.net";

            // <authentication>

            string clientId = Environment.GetEnvironmentVariable("akvClientId");
            string clientSecret = Environment.GetEnvironmentVariable("akvClientSecret");
            string tenantId = Environment.GetEnvironmentVariable("akvTenantId");
            string subscriptionId = Environment.GetEnvironmentVariable("akvSubscriptionId");

            KeyVaultClient kvClient = new KeyVaultClient(async (authority, resource, scope) =>
            {
                var adCredential = new ClientCredential(clientId, clientSecret);
                var authenticationContext = new AuthenticationContext(authority, null);
                return (await authenticationContext.AcquireTokenAsync(resource, adCredential)).AccessToken;
            });
            // </authentication>


            Console.Write("Input the value of your secret > ");
            string secretValue = Console.ReadLine();

            Console.WriteLine("Your secret is '" + secretValue + "'.");

            Console.Write("Saving the value of your secret to your key vault ...");

            // <setsecret>
            var result = P.SetSecret(kvClient, kvURL, secretName, secretValue);
            // </setsecret>
            System.Threading.Thread.Sleep(5000);

            Console.WriteLine("done.");

            Console.WriteLine("Forgetting your secret.");
            secretValue = "";
            Console.WriteLine("Your secret is '" + secretValue + "'.");
            Console.WriteLine("Retrieving your secret from key vault.");

            var fetchedSecret = P.GetSecret(kvClient, kvURL, secretName);

            secretValue = fetchedSecret.Result;
            Console.WriteLine("Your secret is " + secretValue);
        }


        /// <returns> The created or the updated secret </returns>
        public async Task<bool> SetSecret(KeyVaultClient kvClient, string kvURL, string secretName, string secretValue)
        {
            // <setsecret>
            await kvClient.SetSecretAsync($"{kvURL}", secretName, secretValue);
            // </setsecret>

            return true;
        }

        public async Task<string> GetSecret(KeyVaultClient kvClient, string kvURL, string secretName)
        {
            // <getsecret>                
            var keyvaultSecret = await kvClient.GetSecretAsync($"{kvURL}", secretName).ConfigureAwait(false);
            // </getsecret>
            return keyvaultSecret.Value;
        }
    }
}
