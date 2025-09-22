namespace DataMinerApiAdapter.Models
{
    public class JobsUpdatedEvent
    {
        public int AmountOfCreated { get; set; }

        public int AmountOfUpdated { get; set; }

        public int AmountOfDeleted { get; set; }

        public string ModuleId { get; set; }
    }
}