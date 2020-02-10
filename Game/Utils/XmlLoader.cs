using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using GXPEngine.Core;

namespace Game.Utils {
    public static class XmlLoader {
        private static T Load<T>(string path) where T : class {
            using (var stream = File.OpenRead(path)) {
                var serializer = new XmlSerializer(typeof(T));
                return serializer.Deserialize(stream) as T;
            }
        }

        // public static XmlGameLevel LoadGameLevel(string path) {
        //     return Load<XmlGameLevel>(path);
        // }
    }
}