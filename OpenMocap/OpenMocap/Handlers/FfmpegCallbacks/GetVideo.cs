using Microsoft.AspNetCore.Mvc;
using OpenMocap.BackgroundServices;
using OpenMocap.CoreServices.Services;
using OpenMocap.Services;

namespace OpenMocap.Handlers.FfmpegCallbacks
{
    public static class GetVideo
    {
        public const string UrlPattern = "/ffmpeg_callback/get_video/{id}";

        private static async Task<IResult> Handle(
            [FromRoute] Guid id,
            [FromServices] IVideoRepository videoRepository,
            CancellationToken token)
        {
            var bytes = await videoRepository.Get(id, token);
            return Results.File(bytes);
        }

        public static void Map(WebApplication app)
            => app.MapGet(UrlPattern, Handle);
    }
}
