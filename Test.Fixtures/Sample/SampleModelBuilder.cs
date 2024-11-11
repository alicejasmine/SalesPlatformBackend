using Domain.Sample;

namespace Test.Fixtures.Sample
{
    public class SampleModelBuilder
    {
        private Guid _id = SampleModelFixture.DefaultSample.Id;
        private string _name = SampleModelFixture.DefaultSample.Name;
        private string _description = SampleModelFixture.DefaultSample.Description;
        private int _price = SampleModelFixture.DefaultSample.Price;
        private DateTime _created = SampleModelFixture.DefaultSample.Created;
        private DateTime _modified = SampleModelFixture.DefaultSample.Modified;

        public SampleModel Build() => new(_id, _name, _description, _price, _created, _modified);
        
        public SampleModelBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public SampleModelBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public SampleModelBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public SampleModelBuilder WithPrice(int price)
        {
            _price = price;
            return this;
        }

        public SampleModelBuilder WithCreated(DateTime created)
        {
            _created = created;
            return this;
        }

        public SampleModelBuilder WithModified(DateTime modified)
        {
            _modified = modified;
            return this;
        }

    }
}