using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using OpenMocap.CoreServices.Services;
using OpenMocap.Services;

namespace OpenMocap.Handlers.FfmpegCallbacks
{
    public static class PushFrames
    {
        public const string UrlPattern = "/ffmpeg_callback/push_frames/{id}";

        private static async Task<IResult> Handle(
            HttpContext context,
            [FromRoute] Guid id,
            [FromServices] IAsyncVideoFramer asyncVideoFramer,
            [FromServices] SplitedEventsQueue splitedEventsQueue,
            CancellationToken token)
        {
            var bodySizeFeature =
                context.Features.Get<IHttpMaxRequestBodySizeFeature>();
            if (bodySizeFeature is not null)
            {
                bodySizeFeature.MaxRequestBodySize = 1073741824; // set limit 1G (or null for unlimited)
            }

            var stream = context.Request.Body;
            await asyncVideoFramer.SaveRawImages(id, stream, token);
            splitedEventsQueue.Enqueue(new(id));

            return Results.Ok();
        }

        public static void Map(WebApplication app)
            => app.MapPost(UrlPattern, Handle);
    }
}
