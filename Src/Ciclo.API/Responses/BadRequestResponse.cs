using System.Net;
using System.Text.Json.Serialization;

namespace Ciclo.API.Responses;

public class BadRequestResponse : Response
{
    public BadRequestResponse()
    {
        Title = "Ocorreram um ou mais erros de validação!";
        Status = (int)HttpStatusCode.BadRequest;
    }

    public BadRequestResponse(List<string>? erros) : this()
    {
        Erros = erros ?? new List<string>();
    }

    [JsonPropertyOrder(order: 3)]
    public List<string>? Erros { get; private set; }
}