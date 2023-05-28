using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using OpenMocap.CoreServices.Services;

namespace OpenMocap
{
    public static class HttpHandlersConfigure
    {
        public static void AddHttpHandlers(this WebApplication app)
        {
            // Public
            app.MapPost(
                "/async_run_split",
                AsyncRunSplit);
            app.MapGet(
                "/async_get_keypoints/{id}",
                AsyncGetHumanKeyPoints);
            app.MapGet(
                "/video_processing_progress/{operationId}",
                GetVideoProcessingProgress);

            Handlers.Mocap.RunAsync.Map(app);
            Handlers.Mocap.RegisterResultReciver.Map(app);

            app.MapGet(
                "/mocap/progress/{operationId}",
                GetVideoProcessingProgress);

            // Internal
            OpenMocap.Handlers.FfmpegCallbacks.GetVideo.Map(app);
            OpenMocap.Handlers.FfmpegCallbacks.PushFrames.Map(app);

            app.MapGet("/frames_raw/{id}", GetRawFrames);
            app.MapGet("/frames_archive/{id}", GetFramesArchive);
        }

        private static async Task<IResult> AsyncRunSplit(
            IFormFile video,
            IAsyncVideoFramer asyncVideoFramer,
            CancellationToken token)
        {
            using var httpFileStream = video.OpenReadStream();
            var processId = await asyncVideoFramer.RunSplit(httpFileStream, token);
            return Results.Ok(processId);
        }

        private static async Task<IResult> AsyncGetHumanKeyPoints(
            [FromRoute] Guid id,
            IVideoMocaperService videoMocaperService,
            CancellationToken token)
        {
            var keypoints = await videoMocaperService.GetHumanKeypoints(id, token);
            return Results.Ok(keypoints);
        }

        private static IResult GetVideoProcessingProgress(
            IVideoProgressMetric videoProcessingMetricService,
            [FromRoute] Guid operationId)
        {
            var result = videoProcessingMetricService.GetOrDefault(operationId);
            return Results.Ok(result);
        }



        private static async Task<IResult> GetRawFrames(
            [FromRoute] Guid id,
            IImagesRepository imagesRepository,
            CancellationToken token)
        {
            var bytes = await imagesRepository.Get(id, token);
            return Results.File(bytes);
        }

        private static async Task<IResult> GetFramesArchive(
            [FromRoute] Guid id,
            IAsyncVideoFramer asyncVideoFramer,
            CancellationToken token)
        {
            throw new NotImplementedException();
            //var imagesBytes = await asyncVideoFramer.GetSplitedImagesStreamBytes(id, token);
            //byte[] result;
            //using (var archiveMemoryStream = new MemoryStream())
            //{
            //    using (var archive = new ZipArchive(
            //        archiveMemoryStream,
            //        ZipArchiveMode.Create,
            //        true))
            //    {
            //        int index = 0;
            //        foreach (var image in imagesBytes)
            //        {
            //            ZipArchiveEntry entry = archive.CreateEntry($"f_{index}.png");
            //            using (var entryStream = entry.Open())
            //            {
            //                entryStream.Write(image, 0, image.Length);
            //            }
            //            index++;
            //        }
            //    }

            //    result = archiveMemoryStream.ToArray();
            //}

            //return Results.File(
            //    result,
            //    fileDownloadName: "frames.zip");
        }
    }
}
