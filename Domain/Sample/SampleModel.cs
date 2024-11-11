namespace Domain.Sample
{
    public class SampleModel : BaseModel
    {
        public SampleModel(Guid id, string name, string description, int price, DateTime created, DateTime modified) 
            : base(id, created, modified)
        {
            Name = name;
            Description = description;
            Price = price;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}