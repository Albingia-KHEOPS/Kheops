using ALBINGIA.Framework.Common;
using OP.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.Services {
    public class DocumentService {
        private readonly IDbConnection connection;
        readonly CopieDocRepository copieDocRepository;

        public DocumentService(IDbConnection connection, CopieDocRepository copieDocRepository) {
            this.connection = connection;
            this.copieDocRepository = copieDocRepository;
        }

        public void CopyDocuments(Folder folder) {
            var docs = this.copieDocRepository.GetDocuments(folder);
            this.copieDocRepository.CopyDocList(folder, docs);
            this.copieDocRepository.DeleteDocuments(folder);
        }
    }
}
