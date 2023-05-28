using OpenMocap.ML.Services.Locators;
using OpenMocap.ML.Services.Transformers;


namespace OpenMocap.ML.Services
{
    public class MocapService
    {
        private readonly ToBox _toBox;
        private readonly AfterBox _afterBox;
        private readonly ToKeys _toKeys;
        private readonly AfterKeys _afterKeys;

        private readonly HumanBoxLocator _humanBoxLocator;
        private readonly HumanKeysLocator _humanKeysLocator;

        public MocapService(
            HumanBoxLocator humanBoxLocator,
            HumanKeysLocator humanKeysLocator)
        {
            _toBox = new();
            _afterBox = new();
            _toKeys = new ToKeys(HumanKeysLocator.InputSize);
            _afterKeys = new(HumanKeysLocator.InputSize, HumanKeysLocator.OutputSize);

            _humanBoxLocator = humanBoxLocator;
            _humanKeysLocator = humanKeysLocator;
        }

        public IReadOnlyList<Vector2i> Run(Image<Rgb24> originalImage)
        {
            var procesingImage = originalImage.Clone(ctx =>
            {
                _toBox.Forward(originalImage.Size, HumanBoxLocator.InputSize, ctx);
            });

            _humanBoxLocator.Run(procesingImage);
            procesingImage.Mutate(ctx =>
            {
                _afterBox.Forward(ctx, ctx.GetCurrentSize(), _humanBoxLocator.BestRect);
                _toKeys.Forward(ctx, ctx.GetCurrentSize());
            });

            //procesingImage.SaveAsPng("Images/procesingImage_toKeys.png");

            _humanKeysLocator.Run(procesingImage);

            var humanPoints = _humanKeysLocator.Points;

            var humanPointsFitted = new List<Vector2i>();
            foreach (var point in humanPoints)
            {
                var afterKeys = _afterKeys.Inverse(point);
                var tpose = _toKeys.Inverse(afterKeys);
                var afterbox = _afterBox.Inverse(tpose);
                var toBox = _toBox.Inverse(afterbox);
                humanPointsFitted.Add(toBox);
            }

            return humanPointsFitted;
        }
    }
}
