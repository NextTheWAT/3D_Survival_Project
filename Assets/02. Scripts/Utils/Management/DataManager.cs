using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Utils.Management
{
    public class DataManager : Singleton<DataManager>
    {
        /// <summary>
        /// The setting that names the property with camel case when serializing the json file.
        /// </summary>
        private readonly JsonSerializerSettings _settings =
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        private DataManager()
        {

        }

        #region STATIC METHOD API



        #endregion

        #region STATIC PROPERTIES API
 


        #endregion
    }
}
