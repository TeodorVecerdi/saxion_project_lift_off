using System;
using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class Button : GameObject {
        private Texture2D normalTexture;
        private Texture2D selectedTexture;
        private bool isSelected;
        private Action onClick;

        public bool IsSelected {
            get => isSelected;
            set => isSelected = value;
        }

        public Button(Vector2 position, string normalTexturePath, string selectedTexturePath, Action onClick) {
            SetXY(position.x, position.y);
            normalTexture = Texture2D.GetInstance(normalTexturePath, true);
            selectedTexture = Texture2D.GetInstance(selectedTexturePath, true);
            this.onClick = onClick;
        }

        public void Draw(GLContext glContext) {
            if (isSelected) {
                selectedTexture.Bind();
                glContext.DrawQuad(selectedTexture.TextureVertices(position: position, offset:new Vector2(-selectedTexture.width/2f, 0f)), Globals.QUAD_UV);
            } else {
                normalTexture.Bind();
                glContext.DrawQuad(normalTexture.TextureVertices(position: position, offset:new Vector2(-normalTexture.width/2f, 0f)), Globals.QUAD_UV);
            }
        }

        public void Click() {
            onClick?.Invoke();
        }
    }
}