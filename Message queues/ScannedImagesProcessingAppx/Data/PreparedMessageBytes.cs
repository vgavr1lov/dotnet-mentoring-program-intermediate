namespace DataLibrary
{
    public class PreparedMessageBytes
    {
        public byte[]? DataBytes { get; set; }
        public int ActualDataLength { get; set; }
        public int? PositionIdentifier { get; set; }
        public long? ClusterSize { get; set; }
    }
}
