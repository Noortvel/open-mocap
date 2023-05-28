using Microsoft.AspNetCore.Mvc;
using OpenMocap.BackgroundServices;
using OpenMocap.Services;

namespace OpenMocap.Handlers.Mocap
{
    public static class RunAsync
    {
        public const string UrlPattern = "/mocap/run_async";

        public static IResult Handle(
            [FromBody] MocapJob dto,
            [FromServices] MocapJobsQueue jobsQueue,
            [FromServices] CallbacksRepository callbacksRepository,
            CancellationToken token)
        {
            if(callbacksRepository.Count == 0)
            {
                return Results.Problem(
                    detail: "Not registered result recive callbacks",
                    statusCode: 400);
            }

            jobsQueue.Enqueue(dto);

            return Results.Ok();
        }

        public static void Map(WebApplication app)
            => app.MapPost(UrlPattern, Handle);
    }
}
