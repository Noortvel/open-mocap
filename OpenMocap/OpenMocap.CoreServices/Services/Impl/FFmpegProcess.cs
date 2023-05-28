using Microsoft.Extensions.Logging;
using OpenMocap.Core;
using System.Diagnostics;

namespace OpenMocap.CoreServices.Services.Impl
{
    internal class FFmpegProcess : IFFmpegProcess
    {
        private const string FileName = "ffmpeg";
        private readonly ILogger<FFmpegProcess> _logger;
        private readonly IAddressProvider _addressProvider;

        public FFmpegProcess(
            ILogger<FFmpegProcess> logger,
            IAddressProvider addressProvider)
        {
            _logger = logger;
            _addressProvider = addressProvider;
        }

        // TODO: Rewrok to byte[] argument
        public async Task SendSplitToFrames(
            Guid processId,
            CancellationToken cancellationToken)
        {
            var baseUrl = _addressProvider.GetInternalAddress();

            using var process = BackgroundProcessFactory.BuildProcess(FileName);

            process.StartInfo.ArgumentList.Add("-v");
            process.StartInfo.ArgumentList.Add("error");

            process.StartInfo.ArgumentList.Add("-i");
            process.StartInfo.ArgumentList.Add($"{baseUrl}/ffmpeg_callback/get_video/{processId}");

            //process.StartInfo.ArgumentList.Add("-listen");
            //process.StartInfo.ArgumentList.Add("1");

            process.StartInfo.ArgumentList.Add("-c:v");
            process.StartInfo.ArgumentList.Add("png");

            process.StartInfo.ArgumentList.Add("-f");
            process.StartInfo.ArgumentList.Add("image2pipe");
            process.StartInfo.ArgumentList.Add($"{baseUrl}/ffmpeg_callback/push_frames/{processId}");

            process.OutputDataReceived += OutputDataReceivedEventHandler;
            process.ErrorDataReceived += ErrorDataReceived;
            process.Exited += ExitedHandler;

            _logger.LogInformation("Start ffmpeg");
            process.Start();
            process.StandardInput.Flush();
            process.StandardInput.Close();

            await process.WaitForExitAsync(cancellationToken);
            _logger.LogInformation("Exited");
            var stdErr = await process.StandardError.ReadToEndAsync();
            var stdOut = await process.StandardOutput.ReadToEndAsync();
            _logger.LogInformation($"StdOut: {stdOut}");
            _logger.LogInformation($"StdErr: {stdErr}");
        }

        public Task FinishCallback(Guid processId, CancellationToken token)
        {
            return Task.CompletedTask;
        }


        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            _logger.LogInformation("ErrorDataReceived");
        }

        private void OutputDataReceivedEventHandler(object sender, DataReceivedEventArgs e)
        {
            _logger.LogInformation("OutputDataReceivedEventHandler");
        }

        private void ExitedHandler(object? sender, EventArgs e)
        {
            _logger.LogInformation("ExitedHandler");
        }
    }
}
