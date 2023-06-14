using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LetterOfOffer
{
    internal class PaddingRichText
    {
        public class PaddedRichTextBox : System.Windows.Forms.RichTextBox
        {
            private const int EM_SETRECT = 0xB3;
            private const int EM_GETRECT = 0xB2;

            [StructLayout(LayoutKind.Sequential)]
            private struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }

            [System.Runtime.InteropServices.DllImport("User32", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

            protected override void OnHandleCreated(EventArgs e)
            {
                base.OnHandleCreated(e);

                RECT rect = new RECT();

                // Get the current formatting rectangle.
                IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(rect));
                Marshal.StructureToPtr(rect, ptr, false);
                SendMessage(this.Handle, EM_GETRECT, 0, ptr);
                rect = (RECT)Marshal.PtrToStructure(ptr, typeof(RECT));

                // Adjust the dimensions of the RECT structure here
                rect.Left += 10;   // 10 pixels padding on the left
                rect.Top += 10;    // 10 pixels padding on the top
                rect.Right -= 10;  // 10 pixels padding on the right
                rect.Bottom -= 10; // 10 pixels padding on the bottom

                // Set the new formatting rectangle.
                Marshal.StructureToPtr(rect, ptr, true);
                SendMessage(this.Handle, EM_SETRECT, 0, ptr);

                Marshal.FreeCoTaskMem(ptr); // Don't forget to free the allocated memory
            }
        }
    }
}
