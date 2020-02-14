using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;

namespace Game {
    public class FontLoader {
        private static FontLoader instance;
        public static FontLoader Instance => instance ?? (instance = new FontLoader());

        public static readonly StringFormat LeftAlignment = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center},
            CenterAlignment = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center},
            RightAlignment = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center};
        public static readonly StringFormat LeftVerticalAlignment = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionVertical},
            CenterVerticalAlignment = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionVertical},
            RightVerticalAlignment = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionVertical};

        private readonly Dictionary<float, Font> fonts;
        private readonly FontFamily fontFamily;

        private FontLoader() {
            var collection = new PrivateFontCollection();
            collection.AddFontFile(@"data\spaceage.ttf");
            fontFamily = new FontFamily("Space Age", collection);
            fonts = new Dictionary<float, Font>();
        }

        public Font this[float fontSize] => fonts.ContainsKey(fontSize) ? fonts[fontSize] : fonts[fontSize] = new Font(fontFamily, fontSize);
    }
}