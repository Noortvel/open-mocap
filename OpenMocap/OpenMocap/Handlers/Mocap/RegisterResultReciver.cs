using Microsoft.AspNetCore.Mvc;
using OpenMocap.Domain.Dtos;
using OpenMocap.Services;

namespace OpenMocap.Handlers.Mocap
{
    public static class RegisterResultReciver
    {
        public const string UrlPattern = "/mocap/register_result_reciver";

        public static IResult Handle(
            [FromBody] RegisterResultReciverDto dto,
            [FromServices] CallbacksRepository callbacksRepository,
            CancellationToken token)
        {
            callbacksRepository.Add(dto.Url);
            return Results.Ok();
        }

        public static void Map(WebApplication app)
            => app.MapPost(UrlPattern, Handle);
    }
}
