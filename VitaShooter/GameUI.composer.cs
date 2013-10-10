// AUTOMATICALLY GENERATED CODE

using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.HighLevel.UI;

namespace VitaShooter
{
    partial class GameUI
    {
        Button Button_1;
        CheckBox CheckBox_1;

        private void InitializeWidget()
        {
            InitializeWidget(LayoutOrientation.Horizontal);
        }

        private void InitializeWidget(LayoutOrientation orientation)
        {
            Button_1 = new Button();
            Button_1.Name = "Button_1";
            CheckBox_1 = new CheckBox();
            CheckBox_1.Name = "CheckBox_1";

            // GameUI
            this.RootWidget.AddChildLast(Button_1);
            this.RootWidget.AddChildLast(CheckBox_1);

            // Button_1
            Button_1.TextColor = new UIColor(0f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
            Button_1.TextFont = new UIFont(FontAlias.System, 25, FontStyle.Regular);
            Button_1.BackgroundFilterColor = new UIColor(0f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);

            // CheckBox_1
            CheckBox_1.Checked = true;

            SetWidgetLayout(orientation);

            UpdateLanguage();
        }

        private LayoutOrientation _currentLayoutOrientation;
        public void SetWidgetLayout(LayoutOrientation orientation)
        {
            switch (orientation)
            {
                case LayoutOrientation.Vertical:
                    this.DesignWidth = 544;
                    this.DesignHeight = 960;

                    Button_1.SetPosition(52, 245);
                    Button_1.SetSize(214, 56);
                    Button_1.Anchors = Anchors.None;
                    Button_1.Visible = true;

                    CheckBox_1.SetPosition(51, 14);
                    CheckBox_1.SetSize(56, 56);
                    CheckBox_1.Anchors = Anchors.Height | Anchors.Width;
                    CheckBox_1.Visible = true;

                    break;

                default:
                    this.DesignWidth = 960;
                    this.DesignHeight = 544;

                    Button_1.SetPosition(51, 285);
                    Button_1.SetSize(214, 56);
                    Button_1.Anchors = Anchors.None;
                    Button_1.Visible = true;

                    CheckBox_1.SetPosition(90, 83);
                    CheckBox_1.SetSize(56, 56);
                    CheckBox_1.Anchors = Anchors.Height | Anchors.Width;
                    CheckBox_1.Visible = true;

                    break;
            }
            _currentLayoutOrientation = orientation;
        }

        public void UpdateLanguage()
        {
            Button_1.Text = "Button";
        }

        private void onShowing(object sender, EventArgs e)
        {
            switch (_currentLayoutOrientation)
            {
                case LayoutOrientation.Vertical:
                    break;

                default:
                    break;
            }
        }

        private void onShown(object sender, EventArgs e)
        {
            switch (_currentLayoutOrientation)
            {
                case LayoutOrientation.Vertical:
                    break;

                default:
                    break;
            }
        }

    }
}
