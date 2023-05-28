using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMocap.CoreServices.Services
{
    public interface IFFmpegProcess
    {
        Task SendSplitToFrames(Guid videoId, CancellationToken cancellationToken);
    }
}
