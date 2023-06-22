// See https://aka.ms/new-console-template for more information
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class Program
{
    private static readonly HttpClient httpClient = new HttpClient();

    private static async Task Main(string[] args)
    {
        string clientId = "ABVoKKWhe71wUDp84Ce70mRVrmUaF3PvLQnJXYmcIvrYtXrs5b";
        string clientSecret = "91oiFBju63oi7Q9gmg2NVQzd1hFv2g5dhiEWpyVJ";
        string scope = "com.intuit.quickbooks.accounting";
        string accessTokenUrl = "https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer";

        // Obtener el token de acceso
        string accessToken = await GetAccessToken(clientId, clientSecret, scope, accessTokenUrl);

        if (!string.IsNullOrEmpty(accessToken))
        {
            // El token de acceso se ha obtenido correctamente
            Console.WriteLine("Token de acceso obtenido: " + accessToken);

            // Realizar otras operaciones con el token de acceso, como realizar solicitudes a otros recursos protegidos por OAuth 2
            // ...
        }
        else
        {
            // No se pudo obtener el token de acceso
            Console.WriteLine("No se pudo obtener el token de acceso.");
        }
        Console.Read();

        
    }

    static async Task<string> GetAccessToken(string clientId, string clientSecret, string scope, string accessTokenUrl)
    {
        // Configurar las credenciales de cliente
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}")));

        // Construir los parámetros de solicitud del token de acceso
        var requestContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("scope", scope)
        });

        // Enviar la solicitud HTTP POST para obtener el token de acceso
        HttpResponseMessage response = await httpClient.PostAsync(accessTokenUrl, requestContent);

        if (response.IsSuccessStatusCode)
        {
            // Analizar la respuesta JSON para obtener el token de acceso
            var responseContent = await response.Content.ReadAsStringAsync();
            // Asegúrate de tener instalado el paquete NuGet 'System.Text.Json' para utilizar esta línea
            var accessToken = System.Text.Json.JsonDocument.Parse(responseContent).RootElement.GetProperty("access_token").GetString();
            return accessToken;
        }
        else
        {
            // No se pudo obtener el token de acceso
            return null;
        }
    }
}