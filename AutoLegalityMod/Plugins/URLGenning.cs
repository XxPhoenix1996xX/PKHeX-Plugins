﻿using System;
using System.Windows.Forms;
using PKHeX.Core.AutoMod;

namespace AutoLegalityMod
{
    public class URLGenning : AutoModPlugin
    {
        public override string Name => "Gen from URL";
        public override int Priority => 1;

        protected override void AddPluginControl(ToolStripDropDownItem modmenu)
        {
            var ctrl = new ToolStripMenuItem(Name);
            modmenu.DropDownItems.Add(ctrl);
            ctrl.Click += URLGen;
            ctrl.Image = Properties.Resources.urlimport;
        }

        private static void URLGen(object sender, EventArgs e)
        {
            string url = Clipboard.GetText().Trim();
            string initURL = url;
            TeamPasteInfo info;
            try
            {
                info = new TeamPasteInfo(url);
            }
            catch (Exception ex)
            {
                WinFormsUtil.Error($"An error occured while trying to obtain the contents of the URL. This is most likely an issue with your Internet Connection. The exact error is as follows: {ex}");
                return;
            }
            if (!info.Valid)
            {
                WinFormsUtil.Error("The text in the clipboard is not a valid URL.");
                return;
            }
            if (info.Source == TeamPasteInfo.PasteSource.None)
            {
                WinFormsUtil.Error("The URL provided is not from a supported website.");
                return;
            }

            try { AutomaticLegality.ImportModded(info.Sets); }
            catch { WinFormsUtil.Error("The data inside the URL are not valid Showdown Sets"); }

            var response = $"All sets genned from the following URL: {info.URL}\n\n{info.Summary}";
            WinFormsUtil.Alert(response);
            Clipboard.SetText(initURL); // restore clipboard
        }
    }
}