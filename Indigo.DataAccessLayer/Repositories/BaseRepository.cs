namespace Indigo.DataAccessLayer.Repositories
{
    public abstract class BaseRepository
    {
        protected IndigoDataContext DataContext { get; set; }

        protected BaseRepository()
        {
            this.DataContext = new IndigoDataContext();
        }
    }
}