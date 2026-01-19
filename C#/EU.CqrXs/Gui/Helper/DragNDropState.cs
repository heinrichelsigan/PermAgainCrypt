using System.ComponentModel;

namespace EU.CqrXs.Gui.Helper
{
    [DefaultValue(None)]
    public enum DragNDropState
    {
        None = 0x0,
        DragEnter = 0x1,
        DragOver = 0x2,
        DragLeave = 0x4,
        Drop = 0x8
    }

}
