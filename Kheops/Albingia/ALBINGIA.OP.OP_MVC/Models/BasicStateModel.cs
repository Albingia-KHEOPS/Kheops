using System;

namespace ALBINGIA.OP.OP_MVC.Models
{
    [Serializable]
    public class BasicStateModel
    {
        public BasicStateModel()
        {
            IsVisible = true;
        }

        public bool IsVisible { get; set; }

        public bool IsCurrent { get; set; }

        public bool IsLink { get; set; }

        public bool IsLinkVisited { get; set; }

        public string Link { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsHighlighted { get; set; }

        public bool IsSelected { get; set; }

        public string Label { get; set; }
    }
}