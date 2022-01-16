using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albingia.Hexavia.DataAccess
{
    public class BaseRepository
    {
        protected DataAccessManager dataAccessManager;

        public BaseRepository(DataAccessManager dataAccessManager)
        {
            this.dataAccessManager = dataAccessManager;
        }
    }
}
