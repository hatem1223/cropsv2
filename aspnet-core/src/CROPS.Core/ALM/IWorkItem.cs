namespace CROPS.ALM
{
    public interface IWorkItem
    {
        int? SourceId { get; set; }

        int? ProjectId { get; set; }

        int? IsActive { get; set; }

        void CopyFrom(IWorkItem workItem);
    }
}