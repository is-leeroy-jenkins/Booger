﻿namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    public static class ScrollViewerUtils
    {
        public static bool IsAtTop(this ScrollViewer scrollViewer, int threshold = 5)
        {
            if (scrollViewer.VerticalOffset <= threshold)
                return true;

            return false;
        }

        public static bool IsAtEnd(this ScrollViewer scrollViewer, int threshold = 5)
        {
            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                return true;

            if (scrollViewer.VerticalOffset + threshold >= scrollViewer.ScrollableHeight)
                return true;

            return false;
        }
    }
}
