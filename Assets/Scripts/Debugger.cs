using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public static class Debugger
{
    static HttpClient client = new HttpClient();
    public static async Task LogError(string message)
    {
        message = $"\"{message}\"";
        await client.PostAsync("http://192.168.1.116:5000/Log",

            new StringContent(message, Encoding.UTF8, "application/json"));
    }
}
