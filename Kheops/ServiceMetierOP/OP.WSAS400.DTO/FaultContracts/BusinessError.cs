using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;
using System.Collections.Generic;
using System.Linq;

namespace OPWebService {
    public class BusinessError {
        public BusinessError() { }
        public BusinessError(BusinessValidationException ex) {
            Errors = ex?.Errors.ToList() ?? new List<ValidationError>();
        }
        [HandledByMvcError]
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
    }
}