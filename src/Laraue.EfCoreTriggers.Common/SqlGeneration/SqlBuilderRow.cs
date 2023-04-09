using System.Text;

namespace Laraue.EfCoreTriggers.Common.SqlGeneration
{
    /// <summary>
    /// One row of <see cref="SqlBuilder"/>.
    /// </summary>
    public record SqlBuilderRow
    {
        /// <summary>
        /// Row ident.
        /// </summary>
        public int Ident { get; set; }
    
        /// <summary>
        /// Row content.
        /// </summary>
        public StringBuilder StringBuilder { get; set; }
    };
}