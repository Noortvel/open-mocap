namespace OpenMocap.Front
{
    public class AddressesOptions
    {
        public const string Section = "Addresses";

        public string? SelfHost { get; set; }

        public string? SelfPort { get; set; }

        public string? WorkerHost { get; set; }

        public string[]? WorkerPorts { get; set; }
    }
}
