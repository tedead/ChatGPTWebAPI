using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatGPTWebAPI.SharedObjects
{
    public class ImportResponse<T>
    {
        public int TotalCount;
        public int InsertedCount;
        public int UpdatedCount;
        public int InvalidCount;
        public List<T> InvalidObjects = new List<T>();

        public ImportResponse(int totalObjectCount, int insertedCount, int updatedCount, int invalidCount, List<T> invalidObjects)
        {
            TotalCount = totalObjectCount;
            InsertedCount = insertedCount;
            UpdatedCount = updatedCount;
            InvalidCount = invalidCount;
            InvalidObjects = invalidObjects;
        }
    }
}