namespace OpenMocap.ML
{
    public class MlOptions
    {
        public OnnxRuntimeType OnnxRuntimeType { get; set; } = OnnxRuntimeType.Cpu;

        public int? GpuDevice { get; set; }
    }
}
