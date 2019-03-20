using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Geometry
{
    /// <summary>
    /// List of Transformation Types
    /// </summary>
    public enum TransformationTypes
    {
        /// <summary>
        ///  Orientation is perserved.
        /// </summary>
        Translation,
        /// <summary>
        /// Lengths are preserved
        /// </summary>
        Rigid,
        /// <summary>
        /// Angles are preserved.
        /// </summary>
        Similarity,
        /// <summary>
        /// Parallel lines are preserved.
        /// </summary>
        Affine,
        /// <summary>
        /// Straight Lines are preserved.
        /// </summary>
        Projective
    }
}
