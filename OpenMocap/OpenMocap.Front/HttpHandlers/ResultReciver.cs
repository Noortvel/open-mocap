using Microsoft.AspNetCore.Mvc;
using OpenMocap.Domain.Dtos;
using OpenMocap.Front.Services;

namespace OpenMocap.Front.HttpHandlers
{
    public static class ResultReciver
    {
        public const string UrlPattern = "/mocap/result";

        public static IResult Handle(
            [FromBody] MocapResultDto dto,
            [FromServices] MocapResultsStorage resultsStorage,
            CancellationToken token)
        {
            resultsStorage.Add(dto);
            return Results.Ok();
        }

        public static void Map(WebApplication app)
            => app.MapPost(UrlPattern, Handle);
    }
}
