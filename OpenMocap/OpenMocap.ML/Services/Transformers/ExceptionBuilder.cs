namespace OpenMocap.ML.Services.Transformers;

internal static class ExceptionBuilder
{
    public static void ThrowNotForwarded()
    {
        throw new InvalidOperationException("Transformer is not Forwarded");
    }
}
