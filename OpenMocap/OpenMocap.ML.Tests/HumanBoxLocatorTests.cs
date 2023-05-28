using OpenMocap.ML.Services;
using OpenMocap.ML.Services.Locators;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace OpenMocap.ML.Tests
{
    public class HumanBoxLocatorTests
    {
        private EnvService envService;
        private OnnxRuntimeOptionProvider options;
        private OnnxSessionBuilder sessionBuilder;
        private HumanBoxLocator humanBoxLocator;

        [SetUp]
        public void Setup()
        {
            options = new OnnxRuntimeOptionProvider();
            envService = new EnvService();
            sessionBuilder = new(envService, options);
            humanBoxLocator = new HumanBoxLocator(sessionBuilder);
        }

        [Test]
        public void Run_UsualImage_DetectOk()
        {
            var bytes = File.ReadAllBytes("Images/HumanBoxLocatorInput.png");
            var image = Image.Load<Rgb24>(bytes);

            humanBoxLocator.Run(image);
            var bestRect = humanBoxLocator.BestRect;


            var expected = new Rectangle(169, 245, 282, 283);
            var eps = 2;
            Assert.That(IsEqual(expected, bestRect, eps));


            //View = image.Clone();
            //var polygon = new RectangularPolygon(rect.X, rect.Y, rect.Width, rect.Height);
            //View.Mutate(x => x.Draw(
            //    Color.Red,
            //    5,
            //    polygon));
        }

        private static bool IsEqual(Rectangle a, Rectangle b, int eps)
            => IsEqual(a.X, b.X, eps) &&
            IsEqual(a.Y, b.Y, eps) &&
            IsEqual(a.Width, b.Width, eps) &&
            IsEqual(a.Height, b.Height, eps);

        private static bool IsEqual(int a, int b, int eps)
            => a - eps <= b && b <= a + eps;
    }
}
