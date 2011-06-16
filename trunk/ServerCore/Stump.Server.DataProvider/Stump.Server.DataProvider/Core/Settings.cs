
using Stump.Core.Attributes;

namespace Stump.Server.DataProvider.Core
{
    /// <summary>
    ///   Global settings defined by the config file
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Path of content folder
        /// </summary>
        [Variable]
        public static string ContentPath = "./../../content/";

        /// <summary>
        /// Path of static folder
        /// </summary>
        [Variable]
        public static string StaticPath = "./../../static/";
    }
}