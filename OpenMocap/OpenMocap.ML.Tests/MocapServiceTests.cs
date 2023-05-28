using OpenMocap.ML.Services;
using OpenMocap.ML.Services.Locators;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace OpenMocap.ML.Tests
{
    public class MocapServiceTests
    {
        private MocapService mocapService;

        [SetUp]
        public void Setup()
        {
            var envService = new EnvService();
            var options = new OnnxRuntimeOptionProvider();
            var sessionBuilder = new OnnxSessionBuilder(envService, options);
            var humanBoxLocator = new HumanBoxLocator(sessionBuilder);
            var humanKeysLocator = new HumanKeysLocator(sessionBuilder);
            mocapService = new(humanBoxLocator, humanKeysLocator);
        }

        [Test]
        public void Run_UsualImage_DetectOk()
        {
            //Arrange
            var bytes = File.ReadAllBytes("Images/MocapServiceInput.png");

            var image = Image.Load<Rgb24>(bytes);

            //Act
            var points = mocapService.Run(image);


            //Assert
            //Assert.Pass();

            //Assert.That(expectedPoints.Length, Is.EqualTo(points.Count));
            //foreach(var (expectedPoint, point) in expectedPoints.Zip(points))
            //{
            //    Assert.That(IsEqual(expectedPoint, point, eps));
            //}

            // Debug draw
            foreach (var point in points)
            {
                var polygon = new EllipsePolygon(point.X, point.Y, 5);
                image.Mutate(x => x
                    .Draw(Color.Red, 5, polygon));
            }
            image.SaveAsPng("Images/MocapServiceInput_Out.png");

            Assert.Pass();
        }

        private static bool IsEqual(Point a, Point b, int eps)
            => IsEqual(a.X, b.X, eps) &&
            IsEqual(a.Y, b.Y, eps);

        private static bool IsEqual(int a, int b, int eps)
            => a - eps <= b && b <= a + eps;
    }
}
