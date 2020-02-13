using System.Drawing;
using System.Drawing.Text;

namespace Game {
    public class FontLoader {
        private static FontLoader instance;
        public static FontLoader Instance => instance ?? (instance = new FontLoader());

        public readonly StringFormat Left, Center, Right;
        public readonly StringFormat LeftVertical, CenterVertical, RightVertical;

        private PrivateFontCollection collection;
        private FontFamily fontFamily;

        public FontLoader() {
            collection = new PrivateFontCollection();
            collection.AddFontFile(@"data\spaceage.ttf");
            fontFamily = new FontFamily("Space Age", collection);
            Left = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center};
            LeftVertical = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionVertical};
            Center = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};
            CenterVertical = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionVertical};
            Right = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center};
            RightVertical = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionVertical};
        }

        public Font GetFont(float size) {
            return new Font(fontFamily, size);
        }
    }
}