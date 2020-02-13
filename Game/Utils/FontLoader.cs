using System.Drawing;
using System.Drawing.Text;

namespace Game {
    public class FontLoader {
        private static FontLoader instance;
        public static FontLoader Instance => instance ?? (instance = new FontLoader());

        public StringFormat Left, Center, Right;

        private PrivateFontCollection collection;
        private FontFamily fontFamily;

        public FontLoader() {
            collection = new PrivateFontCollection();
            collection.AddFontFile(@"data\spaceage.ttf");
            fontFamily = new FontFamily("Space Age", collection);
            Left = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center};
            Center = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};
            Right = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center};
        }

        public Font GetFont(float size) {
            return new Font(fontFamily, size);
        }
    }
}