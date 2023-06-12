using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LetterOfOffer
{
    public class ExtendedRichTextBox : RichTextBox
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, ref CHARFORMAT2 lParam);

        private const int WM_USER = 0x400;
        private const int EM_GETCHARFORMAT = WM_USER + 58;
        private const int EM_SETCHARFORMAT = WM_USER + 68;
        private const int SCF_SELECTION = 0x1;

        [StructLayout(LayoutKind.Sequential)]
        private struct CHARFORMAT2
        {
            public int cbSize;
            public int dwMask;
            public int dwEffects;
            public int yHeight;
            public int yOffset;
            public int crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szFaceName;
            public short wWeight;
            public short sSpacing;
            public int crBackColor;
            public int LCID;
            public int dwReserved;
            public short sStyle;
            public short wKerning;
            public byte bUnderlineType;
            public byte bAnimation;
            public byte bRevAuthor;
            public byte bUnderlineColor;
            public byte bSpellLangIndex;
        }

        public void InsertLink(string text, string url)
        {
            int start = SelectionStart;
            int length = SelectionLength;
            SelectedText = text;
            Select(start, length);
            SetSelectionLink(true);
            Select(start + length, 0);
        }

        public void SetSelectionLink(bool link)
        {
            CHARFORMAT2 cf = new CHARFORMAT2();
            cf.cbSize = Marshal.SizeOf(cf);
            cf.dwMask = 0x20;
            if (link)
                cf.dwEffects = 0x20;
            else
                cf.dwEffects = 0;
            SendMessage(Handle, EM_SETCHARFORMAT, SCF_SELECTION, ref cf);
        }

        public bool SelectionIsLink()
        {
            CHARFORMAT2 cf = new CHARFORMAT2();
            cf.cbSize = Marshal.SizeOf(cf);
            cf.szFaceName = new char[32];
            SendMessage(Handle, EM_GETCHARFORMAT, SCF_SELECTION, ref cf);
            return (cf.dwEffects == 0x20);
        }

        protected override void OnLinkClicked(LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
            base.OnLinkClicked(e);
        }

    }

}
