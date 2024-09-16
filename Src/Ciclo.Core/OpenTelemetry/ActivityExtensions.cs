using System.Net;
using System.Text;
using System.Data;
using MySqlConnector;
using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Ciclo.Core.OpenTelemetry;

public static class ActivityExtensions
{
    public static async void AddRequestBody(this Activity activity, HttpRequest httpRequest)
    {
        if (httpRequest.Method != "POST")
        {
            return;
        }
        
        IFormCollection? form = null;
        var formString = string.Empty;

        if (httpRequest.HasFormContentType)
        {
            form = httpRequest.Form;

            foreach (var key in form.Keys.Where(key => key.Contains("senha", StringComparison.CurrentCultureIgnoreCase)))
            {
                form.TryGetValue(key, out var formValue);
                formValue = "********";
            }
        }
        else
        {
            var bodyRequest = await new StreamReader(httpRequest.Body).ReadToEndAsync();

            formString = MaskValues(bodyRequest);

            var injectedRequestStream = new MemoryStream();
            var bytesToWrite = Encoding.UTF8.GetBytes(bodyRequest);
            injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
            injectedRequestStream.Seek(0, SeekOrigin.Begin);
            httpRequest.Body = injectedRequestStream;
        }

        formString = form == null 
            ? formString 
            : JsonConvert.SerializeObject(form, Formatting.Indented);

        activity.SetTag("request.body", formString);
    }

    public static async Task AddResponseBody(this Activity? activity, HttpResponse httpResponse)
    {
        if (activity is null)
        {
            return;
        }

        var statusToIgnore = new List<int>
        {
            (int)HttpStatusCode.OK, (int)HttpStatusCode.Created, (int)HttpStatusCode.NoContent,
            (int)HttpStatusCode.Accepted
        };
        
        if (statusToIgnore.Contains(httpResponse.StatusCode))
        {
            return;
        }
        
        var bodyRequest = await new StreamReader(httpResponse.Body).ReadToEndAsync();
        
        var injectedRequestStream = new MemoryStream();
        var bytesToWrite = Encoding.UTF8.GetBytes(bodyRequest);
        injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
        injectedRequestStream.Seek(0, SeekOrigin.Begin);
        httpResponse.Body = injectedRequestStream;
        
        activity.SetTag("response.body", bodyRequest);
    }
    
    public static void AddAuthInfos(this Activity activity, HttpRequest httpRequest)
    {
        var token = httpRequest.Headers["authorization"].ToString();
        if (string.IsNullOrWhiteSpace(token) || token.EndsWith("undefined"))
        {
            return;
        }
        
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var jsonToken = handler.ReadToken(token.Replace("Bearer ", ""));
            var tokenS = jsonToken as JwtSecurityToken;

            activity.SetTag("usuario.id",
                tokenS!.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value);
            activity.SetTag("usuario.nome",
                tokenS.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Name)?.Value);
            activity.SetTag("usuario.email",
                tokenS.Claims.FirstOrDefault(t => t.Type == "email")?.Value);
            activity.SetTag("usuario.tipo",
                tokenS.Claims.FirstOrDefault(t => t.Type == "TipoUsuario")?.Value);
            activity.SetTag("usuario.administrador",
                tokenS.Claims.FirstOrDefault(t => t.Type == "Administrador")?.Value);
        }
        catch (SecurityTokenMalformedException _)
        {
            activity.SetTag("AuthStatus", "Malformed token");
        }
        catch (Exception _)
        {
            activity.SetTag("AuthStatus", "Unable to read the token");
        }
    }

    public static void ReplaceDbParamValues(this Activity activity, IDbCommand command)
    {
        var stringBuilder = new StringBuilder(command.CommandText);

        foreach (MySqlParameter parameter in command.Parameters)
        {
            stringBuilder.Replace(parameter.ParameterName, parameter.Value?.ToString());
        }
                            
        activity.SetTag("db.statement", stringBuilder.ToString());
    }
    
    private static string MaskValues(string json)
    {
        var obj = JsonConvert.DeserializeObject(json);
        if (obj is null)
        {
            return string.Empty;
        }

        if (obj.GetType() == typeof(JArray))
        {
            for (var i = 0; i < ((JArray)obj).Count; i++)
            {
                var item = (object)((JArray)obj)[i];
                MaskProps(ref item);
            }
            
            return JsonConvert.SerializeObject(obj);
        }
        
        MaskProps(ref obj);
        return JsonConvert.SerializeObject(obj);
    }

    private static void MaskProps(ref object obj)
    {
        var toMask = new List<string> { "senha", "password", "key", "secret" };
        ((JObject)obj)
            .Properties()
            .Where(prop => toMask.Exists(m => prop.Name.Contains(m, StringComparison.CurrentCultureIgnoreCase)))
            .ToList()
            .ForEach(prop => prop.Value = "******");
        
        ((JObject)obj)
            .Properties()
            .Where(c => c.Value.GetType() == typeof(JObject))
            .Select(prop => (object)prop.Value)
            .ToList()
            .ForEach(propObj => MaskProps(ref propObj));
    }
}
