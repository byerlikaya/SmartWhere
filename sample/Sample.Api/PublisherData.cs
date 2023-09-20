using Sample.Common;

namespace Sample.Api
{
    public static class PublisherData
    {
        public static void FillDummyData()
        {
            PublisherRepository publisherRepository = new();
            var publisher = DataInitializer.FillMockData().ToList();
            publisherRepository.AddRange(publisher);
        }
    }
}
