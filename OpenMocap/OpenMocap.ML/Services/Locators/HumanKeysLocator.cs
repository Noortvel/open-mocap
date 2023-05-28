using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Numerics;

namespace OpenMocap.ML.Services.Locators
{
    public sealed class HumanKeysLocator : IDisposable
    {
        public static readonly Size InputSize = new(192, 256);
        public static readonly Size OutputSize = new(48, 64);

        private readonly Size HRNetSize = InputSize;
        private readonly Size HrNetOutSize = OutputSize;
        private readonly Vector3 NormalizeMean = new Vector3(0.485f, 0.456f, 0.406f);
        private readonly Vector3 NormalizeStd = new Vector3(0.229f, 0.224f, 0.225f);

        private const int KEYPOINTS_COUNT = 17;

        private Point[] points = new Point[KEYPOINTS_COUNT];
        public IReadOnlyCollection<Point> Points => points;


        private readonly OnnxSessionBuilder _onnxSessionBuilder;
        private readonly InferenceSession _inferenceSession;

        public HumanKeysLocator(OnnxSessionBuilder onnxSessionBuilder)
        {
            _onnxSessionBuilder = onnxSessionBuilder;
            _inferenceSession = _onnxSessionBuilder.Build(ModelType.HRNet);
        }

        public void Run(Image<Rgb24> image)
        {
            var imageSize = image.Size;
            if (imageSize.Width != HRNetSize.Width ||
                imageSize.Height != HRNetSize.Height)
            {
                throw new InvalidOperationException(
                    $"Image size must be eq to locator input size({HRNetSize.Width}, {HRNetSize.Height})");
            }

            var input = new DenseTensor<float>(new[] { 1, 3, HRNetSize.Height, HRNetSize.Width });

            image.ProcessPixelRows(accessor =>
            {
                var (mean2, std2) = GetMeanStd(accessor);
                SetNormalizedTensor(accessor, input, mean2, std2);
            });

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("input.1", input),
            };

            var session = _inferenceSession;
            //using var session = _onnxSessionBuilder.Build(ModelType.HRNet);
            using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = session.Run(inputs);
            var result = results.Single();
            DecodeOutput(result);
        }

        private void SetNormalizedTensor(
            PixelAccessor<Rgb24> accessor,
            DenseTensor<float> input,
            Vector3 mean2,
            Vector3 std2)
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                Span<Rgb24> pixelSpan = accessor.GetRowSpan(y);
                for (int x = 0; x < accessor.Width; x++)
                {
                    var r = pixelSpan[x].R / (float)255;
                    var g = pixelSpan[x].G / (float)255;
                    var b = pixelSpan[x].B / (float)255;
                    input[0, 0, y, x] = r;
                    input[0, 1, y, x] = g;
                    input[0, 2, y, x] = b;

                    input[0, 0, y, x] = (input[0, 0, y, x] - mean2.X) / std2.X;
                    input[0, 1, y, x] = (input[0, 1, y, x] - mean2.Y) / std2.Y;
                    input[0, 2, y, x] = (input[0, 2, y, x] - mean2.Z) / std2.Z;
                    input[0, 0, y, x] = input[0, 0, y, x] * NormalizeStd.X + NormalizeMean.X;
                    input[0, 1, y, x] = input[0, 1, y, x] * NormalizeStd.Y + NormalizeMean.Y;
                    input[0, 2, y, x] = input[0, 2, y, x] * NormalizeStd.Z + NormalizeMean.Z;
                }
            }
        }

        private (Vector3 Mean, Vector3 Std) GetMeanStd(PixelAccessor<Rgb24> accessor)
        {
            var len = accessor.Height * accessor.Width;
            var mean2 = new Vector3(0, 0, 0);
            for (int y = 0; y < accessor.Height; y++)
            {
                Span<Rgb24> pixelSpan = accessor.GetRowSpan(y);
                for (int x = 0; x < accessor.Width; x++)
                {
                    mean2.X += pixelSpan[x].R;
                    mean2.Y += pixelSpan[x].B;
                    mean2.Z += pixelSpan[x].G;
                }
            }
            mean2 /= len;

            var std2 = new Vector3(0, 0, 0);
            for (int y = 0; y < accessor.Height; y++)
            {
                Span<Rgb24> pixelSpan = accessor.GetRowSpan(y);
                for (int x = 0; x < accessor.Width; x++)
                {
                    var r = pixelSpan[x].R - mean2.X;
                    var g = pixelSpan[x].G - mean2.X;
                    var b = pixelSpan[x].B - mean2.X;
                    std2.X += r * r;
                    std2.Y += g * g;
                    std2.Z += b * b;
                }
            }
            std2 /= len;
            std2 = new Vector3(MathF.Sqrt(std2.X), MathF.Sqrt(std2.Y), MathF.Sqrt(std2.Z));

            mean2 /= 255;
            std2 /= 255;

            return (mean2, std2);
        }

        private void DecodeOutput(
            DisposableNamedOnnxValue result)
        {
            var tensor = result.AsTensor<float>();
            for (int layer = 0; layer < KEYPOINTS_COUNT; layer++)
            {
                int maxX = -1;
                int maxY = -1;
                float maxValue = float.NegativeInfinity;
                for (int y = 0; y < HrNetOutSize.Height; y++)
                {
                    for (int x = 0; x < HrNetOutSize.Width; x++)
                    {
                        //float val = tensor.GetValue(y * OUT_SIZE.Width + x);
                        int indx = layer * HrNetOutSize.Width * HrNetOutSize.Height + y * HrNetOutSize.Width + x;
                        float tVal = tensor.GetValue(indx);
                        if (tVal > maxValue)
                        {
                            maxValue = tVal;
                            maxY = y;
                            maxX = x;
                        }
                    }
                }

                points[layer] = new Point(maxX, maxY);
            }
        }

        public void Dispose()
        {
            _inferenceSession?.Dispose();
        }
    }
}

//{ 0, "nose"},
//{ 1, "left_eye"},
//{ 2, "right_eye"},
//{ 3  ,"left_ear"},
//{ 4  ,"right_ear"},
//{ 5  ,"left_shoulder"},
//{ 6  ,"right_shoulder"},
//{ 7  ,"left_elbow"},
//{ 8  ,"right_elbow"},
//{ 9  ,"left_wrist"},
//{ 10 , "right_wrist"},
//{ 11 , "left_hip"},
//{ 12 , "right_hip"},
//{ 13 , "left_knee"},
//{ 14 , "right_knee"},
//{ 15 , "left_ankle"},
//{ 16 , "right_ankle"}
