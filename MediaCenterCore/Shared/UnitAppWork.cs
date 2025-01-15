using MediaCenterCore.Data;

namespace MediaCenterCore.Shared
{
    public class UnitAppWork
    {
        public UnitAppWork(MediaCenterDbContext mediaCenterDbContext)
        {
            MediaCenterDbContext = mediaCenterDbContext;
        }

        public MediaCenterDbContext MediaCenterDbContext { get; }

        public async Task SaveChanges()
        {
            await MediaCenterDbContext.SaveChangesAsync();
        }
    }
}
