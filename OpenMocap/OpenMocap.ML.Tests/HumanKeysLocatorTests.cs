using OpenMocap.ML.Services;
using OpenMocap.ML.Services.Locators;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace OpenMocap.ML.Tests
{
    public class HumanKeysLocatorTests
    {
        private EnvService envService;
        private OnnxRuntimeOptionProvider options;
        private OnnxSessionBuilder sessionBuilder;
        private HumanKeysLocator humanKeysLocator;

        [SetUp]
        public void Setup()
        {
            envService = new EnvService();
            options = new();
            sessionBuilder = new(envService, options);
            humanKeysLocator = new(sessionBuilder);
        }

        [Test]
        public void Run_UsualImage_DetectOk()
        {
            //Arrange
            var bytes = File.ReadAllBytes("Images/HumanKeysLocatorInputBlack.png");
            var image = Image.Load<Rgb24>(bytes);

            //Act
            humanKeysLocator.Run(image);
            var points = humanKeysLocator.Points;

            //Assert
            var eps = 1;
            var expectedPoints = new Point[]
            {
                new(25, 12), new(26, 12),
                new(25, 12), new(26, 13),
                new(23, 13), new(29, 17),
                new(21, 18), new(35, 16),
                new(13, 18), new(42, 15),
                new(7, 18), new(28, 31),
                new(23, 31), new(30, 41),
                new(21, 42), new(31, 51),
                new(19, 50),
            };

            Assert.That(expectedPoints.Length, Is.EqualTo(points.Count));
            foreach(var (expectedPoint, point) in expectedPoints.Zip(points))
            {
                Assert.That(IsEqual(expectedPoint, point, eps));
            }

            // Debug draw
            //foreach (var point in points)
            //{
            //    var polygon = new EllipsePolygon(point.X, point.Y, 1);
            //    image.Mutate(x => x
            //        .Resize(HumanKeysLocator.OutputSize)
            //        .Draw(Color.Red, 1, polygon));
            //}
            //image.SaveAsPng("Images/HumanKeysLocatorInputBlack_Out.png");
        }

        private static bool IsEqual(Point a, Point b, int eps)
            => IsEqual(a.X, b.X, eps) &&
            IsEqual(a.Y, b.Y, eps);

        private static bool IsEqual(int a, int b, int eps)
            => a - eps <= b && b <= a + eps;
    }
}
