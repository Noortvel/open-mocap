using Microsoft.AspNetCore.Mvc;

namespace OpenMocap.Front.HttpHandlers
{
    public static class SendToWorker
    {
        public const string UrlPattern = "/mocap/process_async";

        public static async Task<IResult> Handle(
            IFormFile video,
            [FromServices] OpenMocapClient openMocapClient,
            CancellationToken token)
        {
            using var httpFileStream = video.OpenReadStream();
            using var memoryStream = new MemoryStream();
            await httpFileStream.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();
            var operationId = Guid.NewGuid();

            await openMocapClient.Run(new(operationId, bytes), token);

            return Results.Ok(
                new
                {
                    Id = operationId,
                });
        }

        public static void Map(WebApplication app)
            => app.MapPost(UrlPattern, Handle);
    }
}
