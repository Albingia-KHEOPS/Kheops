namespace Hexavia.Repository
{
    /// <summary>
    /// BaseRepository
    /// </summary>
    public class BaseRepository
    {
        protected DataAccessManager DataAccessManager { get; set; }

        public BaseRepository(DataAccessManager dataAccessManager)
        {
            DataAccessManager = dataAccessManager;
        }
    }
}
