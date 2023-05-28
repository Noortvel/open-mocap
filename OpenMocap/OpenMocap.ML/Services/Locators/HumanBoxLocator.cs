using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;


namespace OpenMocap.ML.Services.Locators
{
    public class HumanBoxLocator
    {
        public static readonly Size InputSize = new(640, 640);

        private readonly Size Yolo5Size = InputSize;
        private readonly List<Rectangle> rects = new();

        private readonly OnnxSessionBuilder _onnxSessionBuilder;
        private readonly InferenceSession _inferenceSession;

        public HumanBoxLocator(OnnxSessionBuilder onnxSessionBuilder)
        {
            _onnxSessionBuilder = onnxSessionBuilder;
            _inferenceSession = _onnxSessionBuilder.Build(ModelType.Yolo5m);
        }

        public int ExpandValue { get; set; } = 50;
        public Rectangle BestRect { get; private set; }
        public float BestRectConfidence { get; private set; }

        public IReadOnlyCollection<Rectangle> Rects
            => rects;

        private Size originalSize;
        public void Run(Image<Rgb24> original)
        {
            originalSize = original.Size;
            if (originalSize.Width != InputSize.Width ||
                originalSize.Height != InputSize.Height)
            {
                throw new InvalidOperationException(
                    $"Image size must be eq to locator input size({InputSize.Width}, {InputSize.Height})");
            }

            BestRectConfidence = 0;
            BestRect = Rectangle.Empty;
            rects.Clear();
            RunModel(original);

        }

        private void RunModel(Image<Rgb24> image)
        {
            var input = new DenseTensor<float>(new[] { 1, 3, Yolo5Size.Height, Yolo5Size.Width });
            var scale = 1 / 255f;

            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<Rgb24> pixelSpan = accessor.GetRowSpan(y);
                    for (int x = 0; x < accessor.Width; x++)
                    {
                        input[0, 0, y, x] = pixelSpan[x].R * scale;
                        input[0, 1, y, x] = pixelSpan[x].G * scale;
                        input[0, 2, y, x] = pixelSpan[x].B * scale;
                    }
                }
            });

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("images", input)
            };

            var session = _inferenceSession;
            //using var session = _onnxSessionBuilder.Build(ModelType.Yolo5m);
            using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = session.Run(inputs);
            DecodeOutput(results.Single());
        }

        private void DecodeOutput(
            DisposableNamedOnnxValue result)
        {
            int size = 2142000;// output_tensor[0].GetTensorTypeAndShapeInfo().GetElementCount(); // 1x25200x85=2142000
            int dimensions = 85; // 0,1,2,3 ->box,4->confidence，5-85 -> coco classes confidence 
            int rows = size / dimensions; //25200
            int confidenceIndex = 4;
            int labelStartIndex = 5;

            var output = result.AsTensor<float>();
            for (int i = 0; i < rows; ++i)
            {
                int index = i * dimensions;
                if (output.GetValue(index + confidenceIndex) <= 0.4f) continue;
               

                for (int j = labelStartIndex; j < dimensions; ++j)
                {
                    var value = output.GetValue(index + j) * output.GetValue(index + confidenceIndex);
                    output.SetValue(index + j, value);
                }

                for (int k = labelStartIndex; k < dimensions; ++k)
                {
                    float rectConfidence = output.GetValue(index + k);
                    if (rectConfidence <= 0.5f) continue;

                    var v1 = output.GetValue(index);
                    var v2 = output.GetValue(index + 2);
                    var v3 = output.GetValue(index + 1);
                    var v4 = output.GetValue(index + 3);

                    var lX = (v1 - v2 / 2);//left x
                    var tY = (v3 - v4 / 2);//top y
                    var rX = (v1 + v2 / 2);//right x
                    var bY = (v3 + v4 / 2);//bottom y
                    var width = rX - lX;
                    var height = bY - tY;

                    var rect = new Rectangle(
                        (int)lX,
                        (int)tY,
                        (int)width,
                        (int)height);

                    rects.Add(rect);
                    if(rectConfidence > BestRectConfidence)
                    {
                        BestRect = rect;
                        BestRectConfidence = rectConfidence;
                    }
                }
            }
        }
    }
}
