using System.ComponentModel;
using WpfAppN1.Extension;

namespace WpfAppN1.ViewModel
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum AlgorithmEnum
    {
        [Description("DFS") ]
        DFS,
        [Description("BFS")]
        BFS,
        [Description("Dijkstra")]
        Dijkstra,
        [Description("A_star")]
        A_star
    }


}
