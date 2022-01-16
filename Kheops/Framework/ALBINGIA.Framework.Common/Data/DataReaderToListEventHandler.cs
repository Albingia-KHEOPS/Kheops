using System.Collections.Generic;
using System.Data;

namespace ALBINGIA.Framework.Common.Data
{
  /// <summary>
  /// Delegate pour le mapping
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="pList"></param>
  /// <param name="pDr"></param>
  /// <param name="pOrdinals"></param>
  //[CodeAuthor("ZOUBAIER BOUAJAJA", "2010/09/19")]
  internal delegate void DataReaderToListEventHandler<T>(ICollection<T> pList, IDataReader pDr, int[] pOrdinals);
}
