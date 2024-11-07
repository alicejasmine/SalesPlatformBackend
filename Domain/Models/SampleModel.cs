namespace Domain.Models
{
    public class SampleModel : BaseModel
    {
        public SampleModel(string name, string description, int price)
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