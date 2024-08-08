namespace Booger
{
    using System;

    public class MarkdownLinkNavigateEventArgs : EventArgs
    {
        public MarkdownLinkNavigateEventArgs(string link)
        {
            Link = link;
        }

        public string Link { get; set; }
    }
}
