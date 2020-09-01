using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AbpDtoGenerator.Base
{
    public abstract class BaseModel
    {
        protected T DClone<T>() where T : BaseModel
        {
            T t = (T)((object)this);
            T result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, this);
                memoryStream.Seek(0L, SeekOrigin.Begin);
                result = (T)((object)binaryFormatter.Deserialize(memoryStream));
                memoryStream.Close();
            }
            return result;
        }
    }
}